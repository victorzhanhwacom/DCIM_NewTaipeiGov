Shader "VzDev/AirFlowStream"
{
    // 氣流路徑物件專用 Shader：捲動UV流線/箭頭貼圖，兩端自動淡入淡出，
    // 避免硬邊切割看起來像貼紙。捲動完全由 _Time.y 驅動，C# 端只需要
    // 設定一次 MaterialPropertyBlock，不需要逐幀更新任何 Uniform。
    // 顏色支援起訖漸變(_Color→_ColorEnd)，依v(沿路徑0~1進度)插值，
    // 不受Tiling/捲動影響，方便做出「靠機身端較亮、遠端漸淡/變色」的層次感。
    Properties
    {
        [Header(Main)]
        _MainTex ("流線貼圖 (Alpha=形狀, RGB可留白由_Color決定顏色)", 2D) = "white" {}
        [HDR] _Color ("氣流顏色-起點(靠機身端)", Color) = (0.4, 0.9, 1, 1)
        [HDR] _ColorEnd ("氣流顏色-終點(遠離機身端)", Color) = (0.4, 0.9, 1, 1)
        _Opacity ("整體不透明度上限", Range(0,1)) = 0.8

        [Header(Scroll)]
        _TilingY ("沿氣流方向的貼圖重複次數(依物件實際長度調整)", Float) = 4
        _ScrollSpeed ("捲動速度", Float) = 1
        _FlowDirection ("氣流方向 (1=沿UV正方向 / -1=反向)", Float) = 1

        [Header(EdgeFade)]
        _FadeInRange ("起點(靠機身端)淡入範圍 0~1", Range(0.001, 0.5)) = 0.15
        _FadeOutRange ("終點(遠離機身端)淡出範圍 0~1", Range(0.001, 0.5)) = 0.35
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" "RenderPipeline"="UniversalPipeline" }
        Cull Off      // 雙面可見，不用額外做兩片Quad
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "AirFlowUnlit"
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 3.0
            // 多個InRowCooler共用同一顆Material時，讓GPU Instancing生效
            // (Material Inspector上記得也要勾選 Enable GPU Instancing)
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                half4 _Color;
                half4 _ColorEnd;
                float _Opacity;
                float _TilingY;
                float _ScrollSpeed;
                float _FlowDirection;
                float _FadeInRange;
                float _FadeOutRange;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = IN.uv;
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // v 保持在原始 0~1 區間，供邊緣淡出與顏色漸變共用判斷，不能被Tiling影響，
                // 否則Tiling一多，淡出/漸變區間會被切碎成一節一節的。
                float v = IN.uv.y;

                float2 scrollUV = float2(
                    IN.uv.x,
                    IN.uv.y * _TilingY + _Time.y * _ScrollSpeed * _FlowDirection
                );

                half4 tex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, scrollUV);

                // 顏色/透明度起訖漸變：起點用_Color，終點用_ColorEnd，中間線性插值。
                half4 tintColor = lerp(_Color, _ColorEnd, v);

                float fadeIn  = smoothstep(0.0, _FadeInRange, v);
                float fadeOut = 1.0 - smoothstep(1.0 - _FadeOutRange, 1.0, v);
                float edgeFade = fadeIn * fadeOut;

                half alpha = tex.a * tintColor.a * _Opacity * edgeFade;
                half3 color = tex.rgb * tintColor.rgb;

                return half4(color, alpha);
            }
            ENDHLSL
        }
    }
}