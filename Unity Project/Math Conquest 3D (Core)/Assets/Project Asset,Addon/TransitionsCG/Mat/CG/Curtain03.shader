Shader "Jettelly/Curtain03"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _Cutoff ("Cutoff", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Transparent"}
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 textureuv : TEXCOORD1;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 textureuv : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };   

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Cutoff;         

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                o.textureuv = TRANSFORM_TEX(v.uv,_MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                 half4 colortex = tex2D(_MainTex, i.uv)  * i.color;
                float leftCurtain = i.uv.y;
                float rightCurtain = 1 - i.uv.y;
                float color = 0;

                if(i.uv.x < 0.5)
                {
                    color = leftCurtain;
                    
                }
                else
                {
                    color = rightCurtain;
                }

                if(color < _Cutoff)
                {
                    return i.color  *colortex;;
                }
                else
                {
                    return float4(0, 0, 0, 0);
                }
            }
            ENDCG
        }
    }
}
