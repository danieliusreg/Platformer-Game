Shader "Unlit/NewUnlitShader"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _Glossiness ("Glossiness", Range(0,1)) = 0.5
        _SpecColor ("Specular Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Glossiness;
            float4 _SpecColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                half4 col = tex2D(_MainTex, i.uv);
                col.rgb = lerp(col.rgb, _SpecColor.rgb, _Glossiness);
                return col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

