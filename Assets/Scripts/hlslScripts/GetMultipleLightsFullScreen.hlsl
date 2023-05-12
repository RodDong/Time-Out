static float4 _AdditionalLightsInspectorColor[100];
static float4 _AdditionalLightsInspectorPos[100];



void GetAdditionalLights_float(in float3 WorldPos, in float3 ObjectPos, in float3 Normal, out float3 extraLight, out float pixelLightCount, out float extraLightIntensity, out float3 lightPos) {
#ifdef SHADERGRAPH_PREVIEW
    pixelLightCount = 0;
    extraLight = float3(0, 0, 0);
    lightPos = float3(0, 0, 0);
    extraLightIntensity = 0.0;
    // get the number of point/spot lights
#else
    extraLight = float3(0, 0, 0);
    pixelLightCount = 2;

    for (int j = 0; j < pixelLightCount; ++j) {
        // grab the light, shadows ,and light color
        float3 attenuatedLightColor = _AdditionalLightsInspectorColor[1];

        // dot product for toonramp
        float d = dot(Normal, _AdditionalLightsInspectorPos[0] - ObjectPos);

        // toonramp in a smoothstep
        float toonRampExtra = smoothstep(0.001, 0.001 + 0, d);

        // add them all together
        extraLight += attenuatedLightColor;
        extraLightIntensity = toonRampExtra;
    }

    lightPos = _AdditionalLightsInspectorPos[1];
#endif
}