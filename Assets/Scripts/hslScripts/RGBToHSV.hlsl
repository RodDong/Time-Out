#ifndef RGBTOHSV
#define RGBTOHSV

#define EPSILON 1e-10

float3 RGBtoHCV(in float3 RGB)
{
    // Based on work by Sam Hocevar and Emil Persson
    float4 P = (RGB.g < RGB.b) ? float4(RGB.bg, -1.0, 2.0 / 3.0) : float4(RGB.gb, 0.0, -1.0 / 3.0);
    float4 Q = (RGB.r < P.x) ? float4(P.xyw, RGB.r) : float4(RGB.r, P.yzx);
    float C = Q.x - min(Q.w, Q.y);
    float H = abs((Q.w - Q.y) / (6 * C + EPSILON) + Q.z);
    return float3(H, C, Q.x);
}


float3 RGBtoHSV(in float3 RGB)
{
    float3 HCV = RGBtoHCV(RGB);
    float S = HCV.y / (HCV.z + EPSILON);
    return float3(HCV.x, S, HCV.z);
}

float3 HUEtoRGB(in float H)
{
    float R = abs(H * 6 - 3) - 1;
    float G = 2 - abs(H * 6 - 2);
    float B = 2 - abs(H * 6 - 4);
    return saturate(float3(R, G, B));
}

float3 HSVtoRGB(in float3 HSV)
{
    float3 RGB = HUEtoRGB(HSV.x);
    return ((RGB - 1) * HSV.y + 1) * HSV.z;
}

void HSVLerp_float(in float4 rgba1, in float4 rgba2, in float t, out float4 Out)
{
    rgba1.xyz = RGBtoHCV(rgba1.xyz);
    rgba2.xyz = RGBtoHCV(rgba2.xyz);

    float T = t;

    float hue;
    float dist = rgba2.x - rgba1.x;

    if (rgba1.x > rgba2.x) {
        float temp = rgba2.x;
        rgba2.x = rgba1.x;
        rgba1.x = temp;

        dist = -dist;
        T = 1 - T;
    }

    if (dist > 0.5)
    {
        rgba1.x = rgba1.x + 1;
        hue = (rgba1.x + T * (rgba2.x - rgba1.x)) % 1;
    }

    if (dist <= 0.5) hue = rgba1.x + T * dist;

    float sat = rgba1.y + T * (rgba2.y - rgba1.y);
    float val = rgba1.z + T * (rgba2.z - rgba1.z);
    float alpha = rgba1.w + t * (rgba1.w - rgba1.w);

    half3 rgb = HSVtoRGB(half3(hue, sat, val));

    Out = half4(rgb, alpha);
}
#endif 