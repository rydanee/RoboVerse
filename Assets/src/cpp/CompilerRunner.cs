using System;
using System.IO;
using UnityEngine;
using Wacs.Core;
using Wacs.Core.Runtime;
using Wacs.Core.Runtime.Types;
using Wacs.Core.Types;
using Wacs.Core.Types.Defs;
using data;
using System.Runtime.InteropServices;

namespace cpp
{
  public static class CompilerRunner
  {
    private static WasmRuntime runtime;
    private static ModuleInstance modInst;
    private static MemoryInstance memory;
    private static Module module;

    // USER CODE
    static string userCleanCode = @"

            void setup() {
                Serial.print(""aa"");
                Serial.print(""bb"");
                pinMode(13, 1);
            }

            void loop() {
                digitalWrite(13, 0);
            }

            ";


    public static void initWasm()
    {
      string wasmPath = Path.Combine(Application.temporaryCachePath, "simulation.wasm");

      if (!File.Exists(wasmPath))
      {
        Debug.LogError($"WASM file not found: {wasmPath}");
        return;
      }

      byte[] wasmBytes = File.ReadAllBytes(wasmPath);
      using var stream = new MemoryStream(wasmBytes);

      module = BinaryModuleParser.ParseWasm(stream);

      runtime = new WasmRuntime();

      arduinoBridge();

      modInst = runtime.InstantiateModule(module);
      runtime.RegisterModule("arduino", modInst);

      memory = runtime.GetExportedMemory(("arduino", "memory"));

      var setupAddr = runtime.GetExportedFunction(("arduino", "arduino_setup"));
      var loopAddr = runtime.GetExportedFunction(("arduino", "arduino_loop"));

      var arduinoSetupInvoke = runtime.CreateInvoker(setupAddr, new InvokerOptions());
      var arduinoLoopInvoke = runtime.CreateInvoker(loopAddr, new InvokerOptions());

      arduinoSetupInvoke();
    }
    private static void arduinoBridge()
    {
      #region digital

      // -- digitalWrite: (int pin, int value) -> void
      runtime.BindHostFunction(("env", "csharp_digitalWrite"),
          new Action<int, int>((pin, value) =>
          {
            // TODO
            Debug.Log($"[Wasm] digitalWrite({pin}, {(PIN_VALUE)value})");
          }));

      // -- digitalRead: (int pin) -> int
      runtime.BindHostFunction(("env", "csharp_digitalRead"),
      new Func<int, int>((pin) =>
      {
        Debug.Log($"[Wasm] digitalRead({pin})");
        return 1; //TODO placeholder
      }));

      // -- pinMode: (int pin, PIN_MODES mode) -> void
      runtime.BindHostFunction(("env", "csharp_pinMode"),
      new Action<int, int>((pin, mode) =>
      {
        // TODO
        Debug.Log($"[Wasm] pinMode({pin}, {(PIN_MODES)mode})");
      }));

      #endregion

      #region analog

      // -- analogRead: (int pin) -> int 
      runtime.BindHostFunction(("env", "csharp_analogRead"),
          new Func<int, int>((pin) =>
          {
            Debug.Log($"[Wasm] analogRead({pin})");
            return 512; // TODO placeholder
          }));

      #endregion

      #region time

      #endregion

      #region communication

      // -- serialPrintStr: (const char* text) -> void
      runtime.BindHostFunction(("env", "csharp_serialPrintStr"),
          new Action<int>((stringPointer) => //fucking ukasatel
          {
            if (stringPointer == 0) return;

            uint maxLength = 4096;
            string rawString = memory.ReadString((uint)stringPointer, maxLength);

            int nullIndex = rawString.IndexOf('\0');
            if (nullIndex >= 0)
            {
              rawString = rawString.Substring(0, nullIndex);
            }

            Debug.Log($"[Serial] {rawString}");
          }));

      // -- serialPrintInt: (int num) -> void
      runtime.BindHostFunction(("env", "csharp_serialPrintInt"),
          new Action<int>((text) =>
          {
            Debug.Log($"stdout: {text}");
          }));

      #endregion
    }

    public static void update()
    {
      File.Delete(Path.Combine(Application.temporaryCachePath, "temp.cpp"));
      File.Delete(Path.Combine(Application.temporaryCachePath, "simulation.wasm"));

      string template = File.ReadAllText("./Assets/src/cpp/template.cpp");
      string finalCode = template.Replace("// {USER_CODE}", userCleanCode);

      File.WriteAllText(Application.temporaryCachePath + "/temp.cpp", finalCode);

      compile();
      initWasm();
    }

    public static void compile()
    {
      string cppFile = Path.Combine(Application.temporaryCachePath, "temp.cpp");
      string wasmFile = Path.Combine(Application.temporaryCachePath, "simulation.wasm");

      if (!File.Exists(cppFile))
      {
        Debug.LogError($"CPP file not found: {cppFile}");
        return;
      }

      string compiler = Path.GetFullPath(Path.Combine(Application.streamingAssetsPath, "/compiler/clang++.exe"));

      if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
      {
        compiler = "clang++";
      }

      string args = $@"""{cppFile}"" -o ""{wasmFile}"" --target=wasm32 -nostdlib -Wl,--export-memory -Wl,--no-entry -Wl,--allow-undefined -Wl,--export=arduino_setup -Wl,--export-all -O0";

      System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo
      {
        FileName = compiler,
        Arguments = args,
        CreateNoWindow = true,
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        WorkingDirectory = Application.temporaryCachePath
      };

      using (System.Diagnostics.Process process = new System.Diagnostics.Process())
      {
        process.StartInfo = startInfo;

        var outputBuilder = new System.Text.StringBuilder();
        var errorBuilder = new System.Text.StringBuilder();

        process.OutputDataReceived += (sender, e) =>
        {
          if (e.Data != null) outputBuilder.AppendLine(e.Data);
        };
        process.ErrorDataReceived += (sender, e) =>
        {
          if (e.Data != null) errorBuilder.AppendLine(e.Data);
        };

        process.Start();

        process.BeginOutputReadLine();
        process.BeginErrorReadLine();

        process.WaitForExit();

        string output = outputBuilder.ToString();
        string error = errorBuilder.ToString();

        if (process.ExitCode == 0)
        {
          Debug.Log($"Compilation succeeded. WASM saved to: {wasmFile}");
          if (!string.IsNullOrEmpty(output)) Debug.Log($"Compiler output: {output}");
        }
        else
        {
          Debug.LogError($"Compilation failed (exit code {process.ExitCode})");
          if (!string.IsNullOrEmpty(error)) Debug.LogError($"STDERR: {error}");
          if (!string.IsNullOrEmpty(output)) Debug.LogError($"STDOUT: {output}");
        }
      }
    }
  }
}
