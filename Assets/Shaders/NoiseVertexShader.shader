Shader "Custom/NoiseVertexShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NoiseAmp ("NoiseAmp", float) = 0.5
		_NoiseFreq ("NoiseFreq", float) = 0.5
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
			
			v2f vert (appdata v)
			{
				v2f o;
				float4 pos = v.vertex + float4(v.normal, 1.0) * simplexNoise(v.vertex.xyz * _NoiseFreq * 3.0 + _Time.y ) * _NoiseAmp;
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
