Shader "Unlit/NewShader"
{
    Properties
    {
        
       // _CrossHatchTexture("Texture", 2D) = "white" {}
       // _CameraTexture("CameraTexture", 2D) = "white" {}
    
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
           // #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _CrossHatchTexture;
            sampler2D _CameraTexture;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
               // o.uv = TRANSFORM_TEX(v.uv, _CrossHatchTexture);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_CrossHatchTexture, i.uv);
                fixed4 col2 = tex2D(_CameraTexture, i.uv);

                // apply fog
                /*UNITY_APPLY_FOG(i.fogCoord, col);*/


               // return float4(1.0f,0.0f,0.0f,1.0f);
                float4 finalColor;

                // Check if col2 is red
                if (col.r > 0.5f && col.g < 0.5f && col.b < 0.5f)
                {
                    // If col2 is red, final color is col
                    finalColor = col2;
                }
                else
                {
                    // If col2 is not red, final color is black
                    finalColor = float4(0.0, 0.0, 0.0, 1.0);
                }

                return finalColor;
                //return col+col2-float4(1.0f, 0.0f, 0.0f, 0.0f);
            }
            ENDCG
        }
    }
}
