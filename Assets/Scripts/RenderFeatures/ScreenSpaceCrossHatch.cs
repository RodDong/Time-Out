using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ScreenSpaceCrossHatch : ScriptableRendererFeature
{

    private CrossHatchPass crossHatchPass;
    public Material material;
    private Material copyMaterial;
    public Shader copyShader;

    public override void Create()
    {
        copyMaterial = CoreUtils.CreateEngineMaterial(copyShader);
        crossHatchPass = new CrossHatchPass(material, copyMaterial);
        crossHatchPass.renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
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
        }
    }

    protected override void Dispose(bool disposing)
    {
        CoreUtils.Destroy(copyMaterial);
    }
}

class CrossHatchPass : ScriptableRenderPass
{
    private Material _material, _copyMaterial;
    private RTHandle m_RenderTexture;

    public CrossHatchPass(Material material, Material copyMaterial)
    {
        _material = material;
        _copyMaterial = copyMaterial;
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

        RenderingUtils.ReAllocateIfNeeded(ref m_RenderTexture, desc, FilterMode.Point, TextureWrapMode.Clamp, name: "_ScreenSpaceTexture");
    }

    public override void Execute(ScriptableRenderContext context,
        ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        using (new ProfilingScope(cmd, new ProfilingSampler("ScreenSpaceCrossHatch")))
        {
            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;

            Blitter.BlitCameraTexture(cmd, source, m_RenderTexture, _material, 0);

            cmd.SetGlobalTexture("_CrossHatchTexture", source);

            //Blitter.BlitCameraTexture(cmd, source, source, _copyMaterial, 0);
        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }
}