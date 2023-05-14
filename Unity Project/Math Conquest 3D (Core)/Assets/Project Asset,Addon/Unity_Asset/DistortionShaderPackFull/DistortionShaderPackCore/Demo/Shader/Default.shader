Shader "DistortionShaderPack/Default"
{
	Properties
	{
		_MainColor("Main color", Color) = (0,0,0,1)
		_StrengthColor("Color strength", Float) = 1
		_DistortionStrength ("Distortion strength", Range(-2,2)) = 0.1
		_DistortionCircle ("Distortion circle", Range(0,1)) = 0
		_NormalTexture("Normal", 2D) = "blue" { }
		_NormalTexStrength("Normal strength", Range(0,1)) = 0.5
		_NormalTexFrameless("Normal circle", Range(0,1)) = 0.5
		_UVOffset("UVOffset XY, ignore ZW", Vector) = (0,0.01,0,0)
	}
	Category
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		SubShader
		{
			GrabPass
			{
				Name "BASE"
				Tags { "LightMode" = "Always" }
			}
			Pass
			{
				Name "BASE"
				Tags { "LightMode" = "Always" }
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				
				sampler2D _GrabTexture;
				float _DistortionStrength;
				float _DistortionCircle;
				float4 _MainColor;
				float _StrengthColor;
				sampler2D _NormalTexture;
				float4 _NormalTexture_ST;
				float _NormalTexStrength;
				float _NormalTexFrameless;
				float4 _UVOffset;
				
				struct VertexInput
				{
					float4 vertex : POSITION;
					float2 texcoord0 : TEXCOORD0;
				};
				
				struct Vert2Frag
				{
					float4 position : SV_POSITION;
					float4 uv_grab : TEXCOORD0;
					float2 uv : TEXCOORD1;
					float2 uv_normal : TEXCOORD2;
					float2 movement: TEXCOORD3;
				};
				Vert2Frag vert (VertexInput vertIn)
				{
					Vert2Frag output;
					output.position = UnityObjectToClipPos(vertIn.vertex);
					output.uv_grab = ComputeGrabScreenPos(output.position);
					output.uv = vertIn.texcoord0;
					output.uv_normal = vertIn.texcoord0.xy * _NormalTexture_ST.xy + _NormalTexture_ST.zw;
					output.movement = _UVOffset.xy*_Time.y;
					return output;
				}
				float2 getVectorFromCenter(float2 uv)
				{
					float factor = _ScreenParams.y / _ScreenParams.x;
					float2 direction = float2((uv.x-0.5), (uv.y-0.5)) * factor;
					return (direction);
				}
				float getDistortionStrength(float2 uv)
				{
					float2 diff = float2(distance(0.5, uv.x), distance(0.5, uv.y)) * 2.0;
					float dist = saturate(length(diff));
					return 1-dist;
				}
				float2 getNormal(sampler2D _NormalTexture, float2 normalUv, float2 uv, float2 uvOffset, float frameless, float strength)
				{
					float2 normal = tex2D( _NormalTexture, normalUv+uvOffset ).zy;
					float length = getDistortionStrength(uv);
					float normalTexStrength = ((1-frameless) + frameless*length) * strength;
					normal.x = ((normal.x-.5)*2) * normalTexStrength;
					normal.y = ((normal.y-.5)*2) * normalTexStrength;
					return normal;
				}
				half4 frag (Vert2Frag fragIn) : SV_Target
				{
					float4 uvScreen = UNITY_PROJ_COORD(fragIn.uv_grab);
					float2 direction = getVectorFromCenter(fragIn.uv);
					float strength = getDistortionStrength(fragIn.uv);
					strength = (_DistortionCircle*strength + (1-_DistortionCircle)) * _DistortionStrength;
					direction *= strength;
					uvScreen += float4(direction.x, direction.y, 0, 0);
					float2 influence = normalize(direction) * strength;
					float2 offset = fragIn.movement;
					float2 normal = getNormal(_NormalTexture, fragIn.uv_normal, fragIn.uv, offset, _NormalTexFrameless, _NormalTexStrength);
					uvScreen += float4(normal.x, normal.y, 0, 0);
					influence += normal.xy;
					float4 final = tex2Dproj(_GrabTexture, uvScreen);
					float alpha = 1;
					final = float4(final.xyz, alpha);
					strength = saturate(sqrt(pow(abs(influence.x), 2.0) + pow(abs(influence.y), 2.0)) * _StrengthColor);
					final = final + (_MainColor*strength);
					final.w = saturate(final.w*_MainColor.w);
					return final;
				}
				ENDCG
			}
		}
	}
}
