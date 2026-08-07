Shader "VzDev/ScrollingWarningStrip"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        [HDR]_Color ("Tint", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "Queue" = "Transparent"
            "RenderPipeline" = "UniversalPipeline"
        }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // SRP Batcher 相容：所有 per-material 屬性必須放進這個 CBUFFER，
            // 名稱固定為 UnityPerMaterial，否則會被 Batcher 排除、變回逐物件 Draw Call
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _ScrollSpeed;
                float4 _Color;
            CBUFFER_END

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv          : TEXCOORD0;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 scrolledUV = input.uv + float2(_Time.y * _ScrollSpeed, 0);
                half4 texColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrolledUV);
                return texColor * _Color;
            }
            ENDHLSL
        }
    }
}