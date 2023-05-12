Shader "Unlit/LightMap"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            
            static float4 _AdditionalLightsInspectorColor[100];
            static float4 _AdditionalLightsInspectorPos[100];
            static int _AdditionalLightsInspectorCount;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldNormal : NORMAL0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                
                float3 extraLight = float3(0, 0, 0);
                int pixelLightCount = _AdditionalLightsInspectorCount;
                float extraLightIntensity = 0.0;

                for (int j = 0; j < pixelLightCount; ++j) {
                    // grab the light, shadows ,and light color
                    float3 attenuatedLightColor = _AdditionalLightsInspectorColor[j];

                    // dot product for toonramp
                    float d = dot(i.worldNormal, _AdditionalLightsInspectorPos[j] - i.vertex);

                    // toonramp in a smoothstep
                    float toonRampExtra = smoothstep(0.001, 0.001 + 1, d);

                    // add them all together
                    extraLight += attenuatedLightColor * toonRampExtra;
                    extraLightIntensity = toonRampExtra;
                }
                return float4(1.0, 1.0, 1.0, 1.0);
            }
            ENDCG
        }
    }
}
