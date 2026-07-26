Shader "Custom/InfiniteGrid"
{
    Properties
    {
        _GridColor ("Grid Color", Color) = (0.5, 0.5, 0.5, 1)
        _GridSpacing ("Grid Spacing", Float) = 1.0
        _LineThickness ("Line Thickness", Range(0.01, 0.1)) = 0.02
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

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

            float4 _GridColor;
            float _GridSpacing;
            float _LineThickness;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldXZ = mul(unity_ObjectToWorld, v.vertex).xz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 grid = abs(frac(i.worldXZ / _GridSpacing - 0.5) - 0.5) / (_LineThickness);
                float lineMask = min(grid.x, grid.y);
                float alpha = 1.0 - min(lineMask, 1.0);

                if (alpha <= 0.0) discard;

                return fixed4(_GridColor.rgb, _GridColor.a * alpha);
            }
            ENDCG
        }
    }
}
