Shader "Custom/NoiseAndSinVertexShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseAmp ("NoiseAmp", float) = 0.5
		_NoiseFreq ("NoiseFreq", float) = 0.5
		_SinAmp ("SinAmp", float) = 0.5
		_SinFreq ("SinFreq", float) = 0.5
		_AnimationMode ("AnimationMode", float) = 0.0
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
			#include "SimplexNoise3D.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float3 normal : TEXCOORD1;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			uniform float _NoiseAmp;
			uniform float _NoiseFreq;

			uniform float _SinAmp;
			uniform float _SinFreq;

			uniform float _AnimationMode;

			float usin(float t){
				return sin(t)*0.5 + 0.5;
			}
			
			v2f vert (appdata v)
			{
				v2f o;

				float4 pos = v.vertex;

				//noise animation
				//_AnimationMode == 1.0 のとき動く
				pos += ( float4(v.normal, 1.0) * simplexNoise(v.vertex.xyz * _NoiseFreq * 3.0 + _Time.y ) * _NoiseAmp ) * step(0.1, _AnimationMode);

				//sin animation
				//_AnimationMode == 0.0 のとき動く
				pos.x += ( v.normal.x * usin(v.vertex.y * _SinFreq * 3.0 + _Time.y * 2.0 ) * _SinAmp ) * step(0.1, 1.0 - _AnimationMode);

				o.vertex = UnityObjectToClipPos(pos);
				o.normal = v.normal;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}
	}
}
