Shader "Custom/BlueprintGrid"
{
    Properties
    {
        _BaseColor ("Base Blueprint Blue", Color) = (0.05, 0.15, 0.3, 1)
        _MainGridColor ("Main Grid Line Color", Color) = (0.2, 0.45, 0.7, 0.6)
        _SubGridColor ("Sub Grid Line Color", Color) = (0.15, 0.3, 0.5, 0.3)
        
        _GridSpacing ("Grid Spacing (Main)", Float) = 5.0
        _SubDivisions ("Subdivisions per Cell", Int) = 5
        
        _MainThickness ("Main Line Thickness", Range(0.01, 0.2)) = 0.04
        _SubThickness ("Sub Line Thickness", Range(0.005, 0.1)) = 0.015
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
                float2 mainGrid = abs(frac(i.worldXZ / _GridSpacing - 0.5) - 0.5) / _MainThickness;
                float mainMask = 1.0 - min(min(mainGrid.x, mainGrid.y), 1.0);

                float subSpacing = _GridSpacing / max(1, _SubDivisions);
                float2 subGrid = abs(frac(i.worldXZ / subSpacing - 0.5) - 0.5) / _SubThickness;
                float subMask = 1.0 - min(min(subGrid.x, subGrid.y), 1.0);

                float4 finalGridColor = lerp(_SubGridColor, _MainGridColor, mainMask);
                float totalMask = max(mainMask, subMask);

                float4 finalColor = lerp(_BaseColor, finalGridColor, totalMask * finalGridColor.a);

                return finalColor;
            }
            ENDCG
        }
    }
}
