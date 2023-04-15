using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Rendering.PostProcessing;

[SerializeField]
[PostProcess(typeof(PostProcessOutlineRenderer), PostProcessEvent.AfterStack, "Outline")]
public class OutlineProcessSetting : PostProcessEffectSettings
{
    public FloatParameter thickness = new FloatParameter { value = 1.0f };
    public FloatParameter depthMin = new FloatParameter { value = 0.0f };
    public FloatParameter depthMax = new FloatParameter { value = 1.0f };
}


