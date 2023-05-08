void GetAdditionalLights_float(in float3 WorldPos, in float3 ObjectPos, in float3 Normal, out float3 extraLight, out float pixelLightCount, out float extraLightIntensity) {
    #ifdef SHADERGRAPH_PREVIEW
        pixelLightCount = 0;
        extraLight = float3(0, 0, 0);
        extraLightIntensity = 0.0;
        // get the number of point/spot lights
    #else
        pixelLightCount = GetAdditionalLightsCount();

        for (int j = 0; j < pixelLightCount; ++j) {
            Light aLight = GetAdditionalLight(j, WorldPos, float4(1, 1, 1, 1));

            // grab the light, shadows ,and light color
            float3 attenuatedLightColor = aLight.color * (aLight.distanceAttenuation * aLight.shadowAttenuation);

            // dot product for toonramp
            float d = dot(Normal,  aLight.direction - ObjectPos);

            // toonramp in a smoothstep
            float toonRampExtra = smoothstep(0.001, 0.001 + 0, d);

            // add them all together
            extraLight += toonRampExtra * attenuatedLightColor;
            extraLightIntensity = d;
        }
    #endif
}