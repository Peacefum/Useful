// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DepthMasker"
{
	Properties{

	 _BaseMap("Base Map", 2D) = "white"{} // 허수아비
	_PaintMap("PaintMap", 2D) = "white"  {}// texture to paint on
   _BaseColor("Base Color", Color) = (1, 1, 1, 1)

	   //   _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
	}

		SubShader{
	   // Render the mask after regular geometry, but before masked geometry and
	   // transparent things.

	   // Don't draw in the RGBA channels; just the depth buffer


	   // Do nothing specific in the pass:
	   Pass {


	   Tags {"RenderType" = "Transparent" "RenderPipeline" = "UniversalRenderPipeline" "Queue" = "Geometry-1" }

				 ColorMask 0 // 아무것도 그리지 않는다는 뜻
	   ZWrite On
			   HLSLPROGRAM
#pragma vertex vert
#pragma fragment frag
				 #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl" 
		struct v2f {
		float4 pos : SV_POSITION;
		float2 uv0 : TEXCOORD0;
		float2 uv1 : TEXCOORD1;

	};
	struct appdata {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0; // uv0
		float2 texcoord1 : TEXCOORD1; // uv1

	};

	TEXTURE2D(_BaseMap);
	// This macro declares the sampler for the _BaseMap texture.
	SAMPLER(sampler_BaseMap);

	TEXTURE2D(_PaintMap);
	SAMPLER(sampler_PaintMap);

	CBUFFER_START(UnityPerMaterial)

	float4 _BaseMap_ST;
	float4 _PaintMap_ST;

	float4 _BaseColor;
	CBUFFER_END



	v2f vert(appdata v) {
		v2f o;

		o.pos = TransformObjectToHClip(v.vertex); // hit된 포지션을 받아오는곳
		o.uv0 = TRANSFORM_TEX(v.texcoord, _BaseMap);

		o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw; // lightmap uvs

		return o;
	}

	half4 frag(v2f o) : SV_Target{
		half4 main_color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap,o.uv0); // main texture
		half4 paint = (SAMPLE_TEXTURE2D(_PaintMap, sampler_PaintMap,o.uv1)); // painted on texture
		main_color *= paint; // add paint to main;
		return main_color;
	}
		ENDHLSL
	}


	   //  ///////// 멀티패스~~~~~~!! 하려고 했지만 다른설명을 보니 그냥 머테리얼 하나를 추가해야 배칭이 줄어드는 현상이있는듯하다

	   // Pass
		  //{

	   //  Tags {"RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }


		  //	HLSLPROGRAM
		  //	#pragma vertex vert
		  //	#pragma fragment frag

		  //	#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"            

		  //	struct Attributes
		  //	{
		  //		float4 positionOS   : POSITION;
		  //		float2 uv           : TEXCOORD0;
		  //	};

		  //	struct Varyings
		  //	{
		  //		float4 positionHCS  : SV_POSITION;
		  //		float2 uv           : TEXCOORD0;
		  //	};

		  //	TEXTURE2D(_BaseMap);
		  //	SAMPLER(sampler_BaseMap);
		  //	TEXTURE2D(_PaintMap);
		  //	SAMPLER(sampler_PaintMap);


		  //	CBUFFER_START(UnityPerMaterial)
		  //		float4 _BaseMap_ST;
		  //	float4 _PaintMap_ST;
		  //	CBUFFER_END

		  //	Varyings vert(Attributes IN)
		  //	{
		  //		Varyings OUT;
		  //		OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
		  //		OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
		  //		return OUT;
		  //	}

		  //	half4 frag(Varyings IN) : SV_Target
		  //	{
		  //		half4 color = SAMPLE_TEXTURE2D(_BaseMap, sampler_BaseMap, IN.uv);
		  //		return color;
		  //	}
		  //	ENDHLSL
		  //}
	// }


   }

}