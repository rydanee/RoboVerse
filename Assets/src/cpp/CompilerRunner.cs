using System;
using System.IO;
using UnityEngine;
using Wacs.Core;
using Wacs.Core.Runtime;
using Wacs.Core.Runtime.Types;
using Wacs.Core.Types;
using Wacs.Core.Types.Defs;

namespace cpp
{
    public static class CompilerRunner
    {
        private static WasmRuntime runtime;
        static string userCleanCode = @"
            void setup() {
            }

            void loop() {
                digitalWrite(13, 1);
            }
            ";

        public static void initWasm()
        {
            string wasmPath = Path.Combine(Application.streamingAssetsPath, "simulation.wasm");
            byte[] wasmBytes = File.ReadAllBytes(wasmPath);
            using var stream = new MemoryStream(wasmBytes);

            var module = BinaryModuleParser.ParseWasm(stream);

            runtime = new WasmRuntime();

            arduinoBridge();

            runtime.InstantiateModule(module);
        }
        private static void arduinoBridge()
        {
            // digitalWrite: (int pin, int value) -> void
            runtime.BindHostFunction(
                ("env", "digitalWrite"),
                new Action<int, int>((pin, value) =>
                {
                    Debug.Log($"[Wasm] digitalWrite({pin}, {value})");
                })
            );

            // pinMode: (int pin, int mode) -> void
            runtime.BindHostFunction(
                ("env", "pinMode"),
                new Action<int, int>((pin, mode) =>
                {
                    Debug.Log($"[Wasm] pinMode({pin}, {mode})");
                })
            );
        }
        public static void update()
        {
            string template = File.ReadAllText("./Assets/src/cpp/template.cpp");
            string finalCode = template.Replace("// {USER_CODE}", userCleanCode);

            File.WriteAllText(Application.temporaryCachePath + "/temp.cpp", finalCode);

            compile();
            initWasm();
        }

        public static void compile()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            process.StartInfo.FileName = Path.Combine(Application.streamingAssetsPath, "compiler/clang.exe");
            process.StartInfo.Arguments = Application.temporaryCachePath + "/temp.cpp" + "-o" + Application.streamingAssetsPath + "/simulation.wasm --target=wasm32 ...";
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
        }
    }
}