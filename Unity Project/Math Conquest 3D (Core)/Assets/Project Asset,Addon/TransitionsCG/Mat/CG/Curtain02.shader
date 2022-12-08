Shader "Jettelly/Curtain02"
{
    Properties
    {
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
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };   

            float _Cutoff;         

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {                    
                float updown = i.uv.x;

                if(0.5 - abs(updown - 0.5) < abs(_Cutoff) * 0.5) 
                { 
                    return i.color; 
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
