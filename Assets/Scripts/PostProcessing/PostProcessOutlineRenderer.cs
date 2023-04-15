using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessOutlineRenderer : PostProcessEffectRenderer<OutlineProcessSetting>
{
    public override void Render(PostProcessRenderContext context)
    {
        PropertySheet property = context.propertySheets.Get(Shader.Find("Hidden/OutlineShader"));
        property.properties.SetFloat("_Thickness", settings.thickness);
        property.properties.SetFloat("_MinDepth", settings.depthMin);
        property.properties.SetFloat("_MaxDepth", settings.depthMax);

        context.command.BlitFullscreenTriangle(context.source, context.destination, property, 0);
    }
}
