Shader "Custom/CellShading"
{Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Main Texture", 2D) = "white" {}
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)
		_RampTex("Ramp Texture", 2D) = "white" {}
	}
SubShader
{
	Pass
	{
		// Setup our pass to use Forward rendering, and only receive
		// data on the main directional light and ambient light.
		Tags
		{
			"LightMode" = "ForwardBase"
			"PassFlags" = "OnlyDirectional"
		}

		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
	// Compile multiple versions of this shader depending on lighting settings.
	#pragma multi_compile_fwdbase

	#include "UnityCG.cginc"
	// Files below include macros and functions to assist
	// with lighting and shadows.
	#include "Lighting.cginc"
	#include "AutoLight.cginc"

	struct appdata
	{
		float4 vertex : POSITION;
		float4 uv : TEXCOORD0;
		float3 normal : NORMAL;
	};

	struct v2f
	{
		float4 pos : SV_POSITION;
		float3 worldNormal : NORMAL;
		float2 uv : TEXCOORD0;
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float4 _AmbientColor;
	sampler2D _RampTex;
	v2f vert(appdata v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.worldNormal = UnityObjectToWorldNormal(v.normal);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);
		return o;
	}

	float4 _Color;

	float4 frag(v2f i) : SV_Target
	{
		float3 normal = normalize(i.worldNormal);
		float NdotL = dot(_WorldSpaceLightPos0, normal);
		float2 uv = float2(1 - (NdotL * 0.5 + 0.5), 0.5);
		float3 lightIntensity = tex2D(_RampTex, uv).rgb;

		
		float4 sample = tex2D(_MainTex, i.uv);
		float3 rgb = sample.rgb * _Color.rgb;
		if (uv.x > 0) {
			return float4(rgb * (lightIntensity),1.0);
		}
		else {
			return float4(rgb * (_AmbientColor * lightIntensity),1.0);
		}
		
	}
	ENDCG
}

// Shadow casting support.
UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
}
}
