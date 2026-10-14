Shader "Custom/BlueprintGrid"
{
    Properties
    {
        _BaseColor ("Base Blueprint Blue", Color) = (0.05, 0.15, 0.3, 1)
        _MainGridColor ("Main Grid Line Color", Color) = (0.2, 0.45, 0.7, 0.6)
        _SubGridColor ("Sub Grid Line Color", Color) = (0.15, 0.3, 0.5, 0.3)
        
        _GridSpacing ("Grid Spacing (Main)", Float) = 5.0
        _SubDivisions ("Subdivisions per Cell", Int) = 5
        
        // Увеличенная толщина и расширенный диапазон ползунков
        _MainThickness ("Main Line Thickness (px)", Range(0.5, 6.0)) = 1.8
        _SubThickness ("Sub Line Thickness (px)", Range(0.5, 4.0)) = 1.1
    }
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 100
        ZWrite On

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 worldXZ : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _BaseColor;
            float4 _MainGridColor;
            float4 _SubGridColor;
            float _GridSpacing;
            int _SubDivisions;
            float _MainThickness;
            float _SubThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldXZ = mul(unity_ObjectToWorld, v.vertex).xz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Применяем базовое утолщение, даже если материал хранит старые малые значения
                float mainThick = max(_MainThickness, 0.5) * 1.6;
                float subThick = max(_SubThickness, 0.3) * 1.6;

                // Главная сетка
                float2 mainUV = i.worldXZ / _GridSpacing;
                float2 mainDistPx = abs(frac(mainUV - 0.5) - 0.5) / fwidth(mainUV);
                float mainLine = min(mainDistPx.x, mainDistPx.y);
                float mainMask = 1.0 - smoothstep(mainThick - 0.5, mainThick + 0.5, mainLine);

                // Второстепенная сетка
                float subSpacing = _GridSpacing / max(1, _SubDivisions);
                float2 subUV = i.worldXZ / subSpacing;
                float2 subDistPx = abs(frac(subUV - 0.5) - 0.5) / fwidth(subUV);
                float subLine = min(subDistPx.x, subDistPx.y);
                float subMask = 1.0 - smoothstep(subThick - 0.5, subThick + 0.5, subLine);

                // Смешивание цветов
                float4 finalGridColor = lerp(_SubGridColor, _MainGridColor, mainMask);
                float totalMask = max(mainMask, subMask);

                float4 finalColor = lerp(_BaseColor, finalGridColor, totalMask * finalGridColor.a);

                return finalColor;
            }
            ENDCG
        }
    }
}