using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class ScreenSpaceCrossHatch : ScriptableRendererFeature
{

    private CrossHatchPass crossHatchPass;
    public Material material;
    public Material copyMaterial;

    public override void Create()
    {
        crossHatchPass = new CrossHatchPass(material, copyMaterial);
        crossHatchPass.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
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
}

class CrossHatchPass : ScriptableRenderPass
{
    private Material _material, _copyMaterial;
    private RTHandle m_RenderTexture;
    private RTHandle m_CopyScreen;
    private RenderTargetHandle tempTextureHandle;

    public CrossHatchPass(Material material, Material copyMaterial)
    {
        _material = material;
        _copyMaterial = copyMaterial;
        renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
        tempTextureHandle.Init("_TempBlitMaterialTexture");
    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0;
        desc.msaaSamples = 1;
        desc.graphicsFormat = RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R8_UNorm, FormatUsage.Linear | FormatUsage.Render)
            ? GraphicsFormat.R8_UNorm
            : GraphicsFormat.B8G8R8A8_UNorm;


        RenderingUtils.ReAllocateIfNeeded(ref m_RenderTexture, desc, FilterMode.Point, TextureWrapMode.Repeat, name: "_ScreenSpaceTexture");
        RenderingUtils.ReAllocateIfNeeded(ref m_CopyScreen, desc, FilterMode.Point, TextureWrapMode.Repeat, name: "_ScreenSpaceTexture");

    }

    public override void Execute(ScriptableRenderContext context,
        ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();
        RenderTextureDescriptor cameraTextureDesc = renderingData.cameraData.cameraTargetDescriptor;
        cameraTextureDesc.depthBufferBits = 0;
        cmd.GetTemporaryRT(tempTextureHandle.id, cameraTextureDesc, FilterMode.Bilinear);

        using (new ProfilingScope(cmd, new ProfilingSampler("ScreenSpaceCrossHatch")))
        {
            var source = renderingData.cameraData.renderer.cameraColorTargetHandle;
            Blitter.BlitCameraTexture(cmd, source, m_RenderTexture, _material, 0);
            cmd.SetGlobalTexture("_CrossHatchTexture", m_RenderTexture);
            cmd.SetGlobalTexture("_CameraTexture", source);
            Blit(cmd, source, tempTextureHandle.Identifier(), _copyMaterial, 0);
            Blit(cmd, tempTextureHandle.Identifier(), source);

        }

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }
}