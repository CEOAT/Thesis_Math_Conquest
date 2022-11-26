Shader "UI/CommonUI"
{
    Properties
    {
                 _MainTex ("_MainTex", 2D) = "white" {}
                // 模板测试属性 和 ColorMask 一般不控制，所以在属性面板上隐藏
                [HideInInspector]_StencilComp ("Stencil Comparison", Float) = 8
		[HideInInspector]_Stencil ("Stencil ID", Float) = 0
		[HideInInspector]_StencilOp ("Stencil Operation", Float) = 0
		[HideInInspector]_StencilWriteMask ("Stencil Write Mask", Float) = 255
		[HideInInspector]_StencilReadMask ("Stencil Read Mask", Float) = 255
    }
    SubShader
    {
		Tags
		{
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
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile _ UNITY_UI_ALPHACLIP
                #include "UnityUI.cginc"
                #include "UnityCG.cginc"

                fixed4 _TextureSampleAdd; // Unity管理：图片格式用Alpha8 
                float4 _ClipRect;// Unity管理：2D剪裁使用
                sampler2D _MainTex;
                struct a2v{
			float4 vertex       : POSITION;
			float4 color        : COLOR;
			float2 texcoord     : TEXCOORD0;
			UNITY_VERTEX_INPUT_INSTANCE_ID
            	};
		struct v2f{
                	float4 vertex       : SV_POSITION;
                	float4 color        : COLOR;
               		float2 texcoord     : TEXCOORD0;
                	UNITY_VERTEX_OUTPUT_STEREO
            	};
            	v2f vert(a2v IN){
                	v2f OUT;
                	UNITY_SETUP_INSTANCE_ID(IN);
			UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);// 实例化处理
                	OUT.vertex = UnityObjectToClipPos(IN.vertex);// 模型空间到裁剪空间
                	OUT.color = IN.color;
                	OUT.texcoord = IN.texcoord;
                	return OUT;
            	}
            	fixed4 frag(v2f IN):SV_Target{
                        // 直接读取纹理颜色 并且乘上 ImageColor
                	half4 color = tex2D(_MainTex,IN.texcoord) * IN.color;
                	return color;
		}
                 ENDCG
        }
    }
}