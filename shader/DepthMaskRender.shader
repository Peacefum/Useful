// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/DepthMasker"
{
    Properties{

        _MainTex("Main Texture", 2D) = "white" {}

    _PaintMap("PaintMap", 2D) = "white" {} // texture to paint on

    _Color ("Main Color", Color) = (1,1,1,1)
       
     //   _Cutoff("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader{
        // Render the mask after regular geometry, but before masked geometry and
        // transparent things.
        Tags {"RenderType" = "Transparent" "LightMode" = "Always" "Queue" = "Geometry-1" }

        // Don't draw in the RGBA channels; just the depth buffer
       ColorMask 0 // 아무것도 그리지 않는다는 뜻
        ZWrite On

        // Do nothing specific in the pass:
        Pass {
                Lighting Off
        CGPROGRAM
         

#pragma vertex vert
#pragma fragment frag


#include "UnityCG.cginc"
#include "AutoLight.cginc"

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
    sampler2D _PaintMap;
    sampler2D _MainTex;
    float4 _MainTex_ST;

    float4 _Color;


    v2f vert(appdata v) {
        v2f o;

        o.pos = UnityObjectToClipPos(v.vertex); // hit된 포지션을 받아오는곳
        o.uv0 = TRANSFORM_TEX(v.texcoord, _MainTex);

        o.uv1 = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw; // lightmap uvs

        return o;
    }

    half4 frag(v2f o) : COLOR{
        half4 main_color = tex2D(_MainTex, o.uv0); // main texture
        half4 paint = (tex2D(_PaintMap, o.uv1)) ; // painted on texture
        main_color *= paint ; // add paint to main;
        return main_color;
    }
        ENDCG
    }
        ////////////////////////////////////////////
        ////////////////////////////////////////////
    }
        Fallback "Diffuse"
}
