Shader "Hidden/LOLButtonBlue (SoftMaskable)"
{
   Properties
    {
         _MainTex ("Sprite Texture", 2D) = "white" {}
        _MaskTex("_MaskTex", 2D) = "white" {}
        _Glowpower("_Glowpower", Float) = 0
        [HDR]_Emission("_Emission", Color) = (1, 1, 1, 1)
        _Lerp("_Lerp", Range(0,1)) = 0
        _Speed("_Speed", Float) = 0.1
        _Tiling("_Tiling", Vector) = (1, 1, 0, 0)
        _NoiseTex("_NoiseTex", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)

        
        
        _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
     SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalRenderPipeline"
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP

           
            #include  "Packages/com.coffee.softmask-for-ugui/Shaders/SoftMask.cginc"	// Add for soft mask
            #pragma shader_feature __ SOFTMASK_EDITOR	// Add for soft mask

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 uv : TEXCOORD0;
                float2 uv_SecondTexture : TEXCOORD2;
                float2 uv_NoiseMap : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv  : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                float2 uv_SecondTexture : TEXCOORD2;
                float2 uv_NoiseMap : TEXCOORD3;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            sampler2D _MaskTex;
            sampler2D _NoiseTex;
            fixed4 _Color;
            fixed4 _Emission;
            fixed4 _TextureSampleAdd;
            float _Lerp;
            float _Speed;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float4 _MaskTex_ST;
            float4 _NoiseTex_ST;
            

            v2f vert(appdata_t v)
            {
                v2f OUT;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
                OUT.worldPosition = v.vertex;
                OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

                OUT.uv = TRANSFORM_TEX(v.uv, _MainTex);
                OUT.uv_SecondTexture = TRANSFORM_TEX(v.uv, _MaskTex);
                OUT.uv_NoiseMap = TRANSFORM_TEX(v.uv,_NoiseTex);

                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
              
               
                half4 color = (tex2D(_MainTex, IN.uv) + _TextureSampleAdd) * IN.color;
                half4 colorSecond = (tex2D(_MaskTex ,IN.uv)+_TextureSampleAdd) * IN.color;

               // IN.NoiseMap.x += _Time.x*_Speed;
                 IN.uv.x -=  _Time.y *_Speed ;
                half4 NoiseTexSample = (tex2D(_NoiseTex,IN.uv)+_TextureSampleAdd)* IN.color;
                
                color *= IN.color;
                _Emission.w = 1;
                color *= _Emission;
                NoiseTexSample.w = 1;
                color = lerp(color,color*NoiseTexSample,_Lerp);
                
                

            //   #ifdef UNITY_UI_CLIP_RECT
                color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
                colorSecond.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
             //   #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                clip (colorSecond.a - 0.001);
                #endif
                
                 color.a *= SoftMask(IN.vertex, IN.worldPosition);	// Add for soft mask// Add for soft mask
                 
                return color;
            }
        ENDCG
        }
    }
}
