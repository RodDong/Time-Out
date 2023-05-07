using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ShadowFeature : ScriptableRendererFeature
{
    class ShadowPass : ScriptableRenderPass
    {
        private Light _light;
        private Material _material;
        private Mesh _mesh;

        public ShadowPass(Material material, Mesh mesh, Light light)
        {
            _material = material;
            _mesh = mesh;
            _light = light;
        }

        public override void Execute(ScriptableRenderContext context,
            ref RenderingData renderingData)
        {
            CommandBuffer cmd = CommandBufferPool.Get(name: "ShadowPass");
            // Get the Camera data from the renderingData argument.
            Camera camera = renderingData.cameraData.camera;
            cmd.SetGlobalTexture("_MyScreenSpaceShadows", BuiltinRenderTextureType.CurrentActive);
            _light.AddCommandBuffer(LightEvent.AfterScreenspaceMask, cmd);
            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);
        }
    }

    private ShadowPass shadowPass;
    public Material material;
    public Mesh mesh;
    public Light light;
    public override void Create()
    {
        shadowPass = new ShadowPass(material, mesh, light);
        // Draw the lens flare effect after the skybox.
        shadowPass.renderPassEvent = RenderPassEvent.AfterRenderingSkybox;
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (material != null && mesh != null)
        {
            renderer.EnqueuePass(shadowPass);
        }
    }
}