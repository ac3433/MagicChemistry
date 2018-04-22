Shader "Unlit/Outline"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5, 0.5,0.5)
		_MainTex ("Texture", 2D) = "white" {}
		_OutlineColor("Outline Color", Color) = (0,0,0,1)
		_OutlineWidth("Outline Width", Range(1.0,5.0)) = 1.01 
	}

	CGINCLUDE	
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members POSITION)
#pragma exclude_renderers d3d11
	#include "UnityCG.cginc"

	struct appdata{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 POSITION;
		float4 color : COLOR;
		float3 normal : NROMAL;
	};

	float _OutlineWidth;
	float4 _OutlineColor;

	v2f vert(appdata v)
	{
		v.vertex.xyz *= _OutlineWidth;

		v3f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.color = _OutlineColor;
		return o;
	}

	ENDCG

	SubShader
	{
		Pass
		{
			ZWrite Off

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			half4 frag(v2f i) : Color
			{
				return i.color
			}
			ENDCG
		}

		Pass
		{
			ZWrite On

			Material
			{
				Diffuse[_Color]
				Ambient[_Color]
			}

			Lighting On

			SetTexture[_MainTex]
			{
				ConstantColor[_Color]
			}

			SetTexture[_MainTex]
			{
				Combine previous * primary DOUBLE
			}
		}
	}
}
