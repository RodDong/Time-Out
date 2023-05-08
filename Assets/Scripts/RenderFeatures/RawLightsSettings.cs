using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.Universal.Internal;
 
public class RawLightsSettings : ScriptableRendererFeature
{
    class RawLightsPass : ScriptableRenderPass
    {
        float[] m_additionalLightIntensities;
        Vector4[] m_additionalLightPositions;
        Vector4[] m_additionalLightRawColors;
        int m_lightCount = 0;

        int m_additionalLightsColor = Shader.PropertyToID("_AdditionalLightsInspectorColor");
        int m_additionalLightsIntensity = Shader.PropertyToID("_AdditionalLightsInspectorIntensity");
        int m_additionalLightsPos = Shader.PropertyToID("_AdditionalLightsInspectorPos");
        int m_additionalLightsCount = Shader.PropertyToID("_AdditionalLightsInspectorCount");

        public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
        {
            base.Configure(cmd, cameraTextureDescriptor);
            m_additionalLightPositions = new Vector4[UniversalRenderPipeline.maxVisibleAdditionalLights];
            m_additionalLightRawColors = new Vector4[UniversalRenderPipeline.maxVisibleAdditionalLights];
            m_additionalLightIntensities = new float[UniversalRenderPipeline.maxVisibleAdditionalLights];
        }
 
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get();
            var lightData = renderingData.lightData;
            var lights = lightData.visibleLights;

            int lightIter = 0;

            for (int i = 0; i < lights.Length && lightIter < UniversalRenderPipeline.maxVisibleAdditionalLights; i++)
            {
                VisibleLight light = lights[i];

                if (lightData.mainLightIndex != i)
                {
                    m_additionalLightIntensities[lightIter] = light.light.intensity;
                    m_additionalLightRawColors[lightIter] = light.light.color;
                    m_additionalLightPositions[lightIter] = light.light.GetComponent<Transform>().position;
                    lightIter++;
                }
            }

            m_lightCount = lights.Length;

            for (int i = 0; i < m_additionalLightsColor && i < m_lightCount; i++)
                Debug.Log(m_additionalLightPositions[0].ToString());
            cmd.SetGlobalVectorArray(m_additionalLightsColor, m_additionalLightRawColors);
            cmd.SetGlobalFloatArray(m_additionalLightsIntensity, m_additionalLightIntensities);
            cmd.SetGlobalVectorArray(m_additionalLightsPos, m_additionalLightPositions);
            cmd.SetGlobalInt(m_additionalLightsCount, m_lightCount);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }
 
    RawLightsPass m_ScriptablePass;
 
    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePass = new RawLightsPass();
 
        // Configures where the render pass should be injected.
        m_ScriptablePass.renderPassEvent = RenderPassEvent.BeforeRenderingOpaques;
    }
 
    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(m_ScriptablePass);
    }
}