using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenSpaceCrossHatch : ScriptableRendererFeature
{
    class CrossHatch : ScriptableRenderPass
    {
        private Material _material;
        private RTHandle m_RenderTarget;
        public CrossHatch(Material material)
        {
            _material = material;
            renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
        }

        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            var desc = renderingData.cameraData.cameraTargetDescriptor;
            desc.depthBufferBits = 0;
            desc.msaaSamples = 1;
            desc.graphicsFormat = RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R8_UNorm, FormatUsage.Linear | FormatUsage.Render)
                ? GraphicsFormat.R8_UNorm
                : GraphicsFormat.B8G8R8A8_UNorm;

            RenderingUtils.ReAllocateIfNeeded(ref m_RenderTarget, desc, FilterMode.Point, TextureWrapMode.Clamp, name: "_ScreenSpaceShadowmapTexture");
            cmd.SetGlobalTexture(m_RenderTarget.name, m_RenderTarget.nameID);

            ConfigureTarget(m_RenderTarget);
            ConfigureClear(ClearFlag.None, Color.white);
        }

        public override void Execute(ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get();
            using (new ProfilingScope(cmd, new ProfilingSampler("ScreenSpaceCrossHatch")))
            {
                Blitter.BlitCameraTexture(cmd, m_RenderTarget, m_RenderTarget, _material, 0);
                CoreUtils.SetKeyword(cmd, ShaderKeywordStrings.MainLightShadows, false);
                CoreUtils.SetKeyword(cmd, ShaderKeywordStrings.MainLightShadowCascades, false);
                CoreUtils.SetKeyword(cmd, ShaderKeywordStrings.MainLightShadowScreen, true);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();

            CommandBufferPool.Release(cmd);
        }
    }

    private CrossHatch crossHatchPass;
    public Material material;

    public override void Create()
    {
        crossHatchPass = new CrossHatch(material);
        // Draw the lens flare effect after the skybox.
        crossHatchPass.renderPassEvent = RenderPassEvent.AfterRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material != null)
        {
            renderer.EnqueuePass(crossHatchPass);
        }
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            crossHatchPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            crossHatchPass.ConfigureInput(ScriptableRenderPassInput.Color);
            crossHatchPass.ConfigureTarget(renderer.cameraColorTargetHandle, renderer.cameraDepthTargetHandle);
        }
    }
}