using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlitMaterialFeature : ScriptableRendererFeature
{
    class RenderPass : ScriptableRenderPass
    {

        private string profilingName;
        private Material material;
        private int materialPassIndex;
        private RenderTargetIdentifier sourceID;
        private RenderTargetHandle tempTextureHandle;

        Light[] m_additionalLights;
        float[] m_additionalLightIntensities;
        Vector4[] m_additionalLightPositions;
        Vector4[] m_additionalLightRawColors;
        int m_lightCount = 0;

        int m_additionalLightsColor = Shader.PropertyToID("_AdditionalLightsInspectorColor");
        int m_additionalLightsIntensity = Shader.PropertyToID("_AdditionalLightsInspectorIntensity");
        int m_additionalLightsPos = Shader.PropertyToID("_AdditionalLightsInspectorPos");
        int m_additionalLightsCount = Shader.PropertyToID("_AdditionalLightsInspectorPos");

        public RenderPass(string profilingName, Material material, int passIndex) : base()
        {
            this.profilingName = profilingName;
            this.material = material;
            this.materialPassIndex = passIndex;
            tempTextureHandle.Init("_TempBlitMaterialTexture");
            m_additionalLights = new Light[UniversalRenderPipeline.maxVisibleAdditionalLights];
            m_additionalLightIntensities = new float[UniversalRenderPipeline.maxVisibleAdditionalLights];
        }

        public void SetSource(RenderTargetIdentifier source)
        {
            this.sourceID = source;
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(profilingName);

            RenderTextureDescriptor cameraTextureDesc = renderingData.cameraData.cameraTargetDescriptor;
            cameraTextureDesc.depthBufferBits = 0;

            var lightData = renderingData.lightData;
            var lights = lightData.visibleLights;

            int lightIter = 0;

            for (int i = 0; i < lights.Length && lightIter < UniversalRenderPipeline.maxVisibleAdditionalLights; i++)
            {
                VisibleLight light = lights[i];

                if (lightData.mainLightIndex != i)
                {
                    m_additionalLights[lightIter] = light.light;
                    m_additionalLightIntensities[lightIter] = light.light.intensity;
                    m_additionalLightRawColors[lightIter] = light.light.color;
                    m_additionalLightPositions[lightIter] = light.light.GetComponent<Transform>().position;
                    lightIter++;
                }
            }

            m_lightCount = lightIter;

            for (int i = 0; i < m_additionalLightsColor && i < lights.Length; i++)
                Debug.Log(m_additionalLightRawColors[i].ToString());

            cmd.SetGlobalVectorArray(m_additionalLightsColor, m_additionalLightRawColors);
            cmd.SetGlobalFloatArray(m_additionalLightsIntensity, m_additionalLightIntensities);
            cmd.SetGlobalVectorArray(m_additionalLightsPos, m_additionalLightPositions);
            cmd.SetGlobalInt(m_additionalLightsCount, m_lightCount);

            cmd.GetTemporaryRT(tempTextureHandle.id, cameraTextureDesc, FilterMode.Bilinear);
            Blit(cmd, sourceID, tempTextureHandle.Identifier(), material, materialPassIndex);
            Blit(cmd, tempTextureHandle.Identifier(), sourceID);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }

        public override void FrameCleanup(CommandBuffer cmd)
        {
            cmd.ReleaseTemporaryRT(tempTextureHandle.id);
        }
    }

    [System.Serializable]
    public class Settings
    {
        public Material material;
        public int materialPassIndex = -1; // -1 means render all passes
        public RenderPassEvent renderEvent = RenderPassEvent.AfterRenderingOpaques;
    }

    [SerializeField]
    private Settings settings = new Settings();

    private RenderPass renderPass;

    public Material Material
    {
        get => settings.material;
    }

    public override void Create()
    {
        this.renderPass = new RenderPass(name, settings.material, settings.materialPassIndex);
        renderPass.renderPassEvent = settings.renderEvent;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderPass.SetSource(renderer.cameraColorTargetHandle);
        renderer.EnqueuePass(renderPass);
    }
}