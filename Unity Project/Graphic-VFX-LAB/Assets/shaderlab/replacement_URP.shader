Shader "USB/USB_replacement_shader_URP"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcBlend ("SrcFactor", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstBlend ("DstFactor", Float) = 1
        _MainTex ("Texture", 2D) = "white" {}
        _BackTex ("Texture", 2D) = "white" {}
        [Enum(UnityEngine.Rendering.CullMode)]
        _Cull ("Cull", Float) = 0
        
        _Color ("Tint", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"
                "RenderPipeline"="UniversalRenderPipeline"
             }
        
        
        Blend DstColor SrcColor
       
        LOD 100

        Pass
        {
            HLSLPROGRAM
// Upgrade NOTE: excluded shader from DX11 because it uses wrong array syntax (type[size] name)

            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
          

   

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

             // #include "HLSLSupport.cginc"
            //  #include "UnityCG.cginc"
         //  #include "UnityCG.cginc"


            #include "HLSLSupport.cginc"
             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			//#include "UnityPBSLighting.cginc"
           
           
			

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
//                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 normal : NORMAL0;
            };

            sampler2D _MainTex;
            sampler2D _BackTex;
            float4 _MainTex_ST;
            void FakeLight_float (in float3 Normal, out float3 Out)
                {
                    float3 operation = Normal;
                    Out = operation;
                }

            half3 FakeLight (float3 Normal)
                {
                 float3 operation = Normal;
                 return operation;
                }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = TransformObjectToHClip(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//                UNITY_TRANSFER_FOG(o,o.vertex);
                 UNITY_INITIALIZE_OUTPUT(v2f,o)
                return o;
            }

           half4 frag (v2f i, bool face : SV_IsFrontFace) : SV_Target
            {
                half4 colFront = tex2D(_MainTex, i.uv);
                half4 colBack = tex2D(_BackTex, i.uv);
                // sample the texture
               
                // declare normals.
                float3 n = i.normal;
                // declare the output.
               
                // apply fog
//                UNITY_APPLY_FOG(i.fogCoord, col);
                half4 red = half4(1, 0, 0, 1);

               float3 col = FakeLight(n);
                //return col * red;
                //return face ? colFront : colBack;
                return float4(col.rgb, 1);
            }
            ENDHLSL
        }
    }
}
