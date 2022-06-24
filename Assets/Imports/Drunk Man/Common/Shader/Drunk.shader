Shader "Drunk Man/Drunk" {
	Properties {
		[HideInInspector]_MainTex  ("Main", 2D) = "white" {}
		_RGBShiftFactor   ("RGB Shift Factor", Float) = 0
		_RGBShiftPower    ("RGB Shift Power", Float) = 3
		_GhostSeeRadius   ("Ghost See Radius", Float) = 0.01
		_GhostSeeMix      ("Ghost See Mix", Float) = 0.5
		_GhostSeeAmplitude("Ghost See Amplitude", Float) = 0.05
		_Dimensions       ("Dark Dimensions", Vector) = (0.5, 0.5, 0, 0)
		_Frequency        ("Refraction Frequency", Float) = 10
		_Period           ("Refraction Period", Float) = 1.5
		_RandomNumber     ("Refraction Random", Float) = 1
		_Amplitude        ("Refraction Amplitude", Float) = 40
	}
	CGINCLUDE
		#include "UnityCG.cginc"
		#include "Utils.cginc"
		sampler2D _MainTex, _Global_OrigScene;
		float4 _MainTex_TexelSize;
		float _Global_Fade;
	ENDCG
	SubShader {
		ZTest Off Cull Off ZWrite Off Blend Off Fog { Mode Off }
		Pass {   // pass 0
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float _RGBShiftFactor, _RGBShiftPower;

			float4 frag (v2f_img input) : SV_TARGET
			{
				float2 dist = 0.5 - input.uv;
				float2 unit = dist / length(dist);

				float ol = length(dist) * _RGBShiftFactor;
				ol = 1.0 - pow(1.0 - ol, _RGBShiftPower);
				float2 offset = unit * ol;
 			
				float4 cr  = tex2D(_MainTex, input.uv + offset);
				float4 cga = tex2D(_MainTex, input.uv);
				float4 cb  = tex2D(_MainTex, input.uv - offset);
				return float4(cr.r, cga.g, cb.b, cga.a);
			}
			ENDCG
		}
		Pass {   // pass 1
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float _GhostSeeRadius, _GhostSeeMix;

			float4 frag (v2f_img i) : SV_TARGET
			{
				float angle = _Time.y;
				float2 offset = float2(cos(angle), sin(angle * 2.0)) * _GhostSeeRadius;
				float4 shifted = tex2D(_MainTex, i.uv + offset);
				float4 orig = tex2D(_MainTex, i.uv);
				return lerp(orig, shifted, _GhostSeeMix);
			}
			ENDCG
		}
		Pass {   // pass 2
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float _GhostSeeAmplitude;

			float4 frag (v2f_img i) : SV_TARGET
			{
				float sq = sin(_Time.y) * _GhostSeeAmplitude;
				float4 tc = tex2D(_MainTex, i.uv);
				float4 tl = tex2D(_MainTex, i.uv - float2(sin(sq), 0));
				float4 tR = tex2D(_MainTex, i.uv + float2(sin(sq), 0));
				float4 tD = tex2D(_MainTex, i.uv - float2(0, sin(sq)));
				float4 tU = tex2D(_MainTex, i.uv + float2(0, sin(sq)));
				return (tc + tl + tR + tD + tU) / 5;
			}
			ENDCG
		}
		Pass {   // pass 3
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float2 _Dimensions;

			half4 frag (v2f_img input) : SV_TARGET
			{
				float2 gradient = 0.5 - input.uv;
				gradient.x = gradient.x * (1.0 / _Dimensions.x);
				gradient.y = gradient.y * (1.0 / _Dimensions.y);
				float dist = length(gradient);
				half4 tc = tex2D(_MainTex, input.uv);
				tc = lerp(tc, 0.0, dist);

				half4 orig = tex2D(_Global_OrigScene, input.uv);
				return lerp(orig, tc, _Global_Fade);
			}
			ENDCG
		}
		Pass {   // pass 4
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float _Frequency, _Period, _RandomNumber, _Amplitude;

			float4 frag (v2f_img i) : SV_TARGET
			{
				const float PI = 3.141592;
				float n = snoise(float3((i.uv * _Frequency), _RandomNumber * _Period + _Time.y)) * PI;
				float2 offset = float2(cos(n), sin(n)) * _Amplitude * _MainTex_TexelSize.xy;
				return tex2D(_MainTex, i.uv + offset);
			}
			ENDCG
		}
		Pass {   // pass 5
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			float _BlurMin, _BlurMax, _BlurSpeed;

			half4 frag (v2f_img input) : SV_TARGET
			{
				half4 c = 0;
				float t = lerp(_BlurMin, _BlurMax, (sin(_Time.y * _BlurSpeed) + 1) / 2);
				for (int n = 0; n <= 25; n++)
				{
					float q = n / 25.0;
					c += tex2D(_MainTex, input.uv + (0.5 - input.uv) * q * t) / 25.0;
				}

				half4 orig = tex2D(_Global_OrigScene, input.uv);
				return lerp(orig, c, _Global_Fade);
			}
			ENDCG
		}
		Pass {   // pass 6
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			half4 frag (v2f_img input) : SV_TARGET
			{
				float2 uv = input.uv;
				float t = _Time.y;
				float2 o = float2(
					sin(t * 1.25 + 75.0 + uv.y * 0.5) + sin(t * 2.75 - 18.0 - uv.x * 0.25),
					sin(t * 1.75 - 125.0 + uv.x * 0.25) + sin(t * 2.25 + 4.0 - uv.y * 0.5)) * 0.25 + 0.5;

				float z = sin((t + 234.5) * 3.0) * 0.05 + 0.75;
				float2 uv2 = ((uv - o) * z + o);
				half4 c = tex2D(_MainTex, uv2);

				half4 orig = tex2D(_Global_OrigScene, input.uv);
				return lerp(orig, c, _Global_Fade);
			}
			ENDCG
		}
	}
	FallBack Off
}