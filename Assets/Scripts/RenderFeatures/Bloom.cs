using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static Unity.VisualScripting.Member;

public class Bloom : ScriptableRendererFeature
{

    private BloomPass bloomPass;
    public Material material;
    public Material copyMaterial;

    public override void Create()
    {
        bloomPass = new BloomPass(material, copyMaterial);
        bloomPass.renderPassEvent = RenderPassEvent.BeforeRenderingTransparents;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material != null)
        {
            renderer.EnqueuePass(bloomPass);
        }
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        if (renderingData.cameraData.cameraType == CameraType.Game)
        {
            bloomPass.ConfigureInput(ScriptableRenderPassInput.Depth);
            bloomPass.ConfigureInput(ScriptableRenderPassInput.Color);
        }
    }
}

class BloomPass : ScriptableRenderPass
{
    private Material _material, _copyMaterial;
    private RTHandle m_RenderTexture;
    private RTHandle m_CopyScreen;
    private RenderTargetHandle tempTextureHandle;

    public BloomPass(Material material, Material copyMaterial)
    {

    }

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        var desc = renderingData.cameraData.cameraTargetDescriptor;
        desc.depthBufferBits = 0;
        desc.msaaSamples = 1;
        desc.graphicsFormat = RenderingUtils.SupportsGraphicsFormat(GraphicsFormat.R8_UNorm, FormatUsage.Linear | FormatUsage.Render)
            ? GraphicsFormat.R8_UNorm
            : GraphicsFormat.B8G8R8A8_UNorm;
    }

    public override void Execute(ScriptableRenderContext context,
        ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get();

        context.ExecuteCommandBuffer(cmd);
        cmd.Clear();

        CommandBufferPool.Release(cmd);
    }
}