Shader "Unlit/ScreenSpaceShadowCrossHatch"
{
    Properties{
       _CrossHatch("CrossHatch", 2D) = "white"
    }
    SubShader
    {
        Tags{ "RenderPipeline" = "UniversalPipeline" "IgnoreProjector" = "True"}

        HLSLINCLUDE

        //Keep compiler quiet about Shadows.hlsl.
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/EntityLighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/ImageBasedLighting.hlsl"
        // Core.hlsl for XR dependencies
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

        TEXTURE2D(_CrossHatch);
        SAMPLER(sampler_CrossHatch);

        half4 Fragment(Varyings input) : SV_Target
        {
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

#if UNITY_REVERSED_Z
            float deviceDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, input.texcoord.xy).r;
#else
            float deviceDepth = SAMPLE_TEXTURE2D_X(_CameraDepthTexture, sampler_CameraDepthTexture, input.texcoord.xy).r;
            deviceDepth = deviceDepth * 2.0 - 1.0;
#endif
            float2 temp = input.texcoord * 6.0f;

            float4 _SampleTexture2D_RGBA = SAMPLE_TEXTURE2D(_CrossHatch, sampler_CrossHatch, temp);
            float _SampleTexture2D_R = _SampleTexture2D_RGBA.r;
            float _SampleTexture2D_G = _SampleTexture2D_RGBA.g;
            float _SampleTexture2D_B = _SampleTexture2D_RGBA.b;
            float _SampleTexture2D_A = _SampleTexture2D_RGBA.a;

            float _SampleTexture2D_Combined = _SampleTexture2D_R + _SampleTexture2D_G + _SampleTexture2D_B;

            //Fetch shadow coordinates for cascade.
            float3 wpos = ComputeWorldSpacePosition(input.texcoord.xy, deviceDepth, unity_MatrixInvVP);
            float4 coords = TransformWorldToShadowCoord(wpos);

            // Screenspace shadowmap is only used for directional lights which use orthogonal projection.
            half realtimeShadow = MainLightRealtimeShadow(coords);

            int intensity = 1 - step(0.6, realtimeShadow * 10);

            return float4(1, 1, 1, 1) - float4(intensity * _SampleTexture2D_B, 0.0f, 0.0f, 0.0f);
        }

        ENDHLSL

        Pass
        {
            Name "ScreenSpaceShadowsCrossHatch"
            ZTest Always
            Cull Off

            HLSLPROGRAM
            #pragma multi_compile _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile_fragment _ _SHADOWS_SOFT

            #pragma vertex   Vert
            #pragma fragment Fragment
            ENDHLSL
        }
    }
}
