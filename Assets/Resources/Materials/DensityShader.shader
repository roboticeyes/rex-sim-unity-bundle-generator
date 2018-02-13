Shader "Unlit/DensityShader"
{
	Properties
	{
		[PerRenderer]_LowColor("Low Color", Color) = (1, 1, 1, 1)
		_HighColor("High Color", Color) = (1, 1, 1, 1)
		_Density("Density", Range(0.0,1.0)) = 0.0
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
			#pragma multi_compile_instancing
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			UNITY_INSTANCING_BUFFER_START(Props)
           		UNITY_DEFINE_INSTANCED_PROP(float, _Density)
       		UNITY_INSTANCING_BUFFER_END(Props)
			fixed4 _LowColor;
			fixed4 _HighColor;
			
			v2f vert (appdata v)
			{
				v2f o;

				UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				return lerp(_LowColor, _HighColor, UNITY_ACCESS_INSTANCED_PROP(Props, _Density));
			}
			ENDCG
		}
	}
	FallBack "Diffuse"
}
