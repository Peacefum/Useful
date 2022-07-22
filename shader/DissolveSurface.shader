Shader "Custom/DissolveSurface" {
	Properties{
		//_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		//_EmissionColor("EmissionColor", Color) = (1,1,1,1)

	/*	_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0*/

			//Dissolve properties
			_DissolveTexture("Dissolve Texutre", 2D) = "white" {}
			_Amount("Amount", Range(0,1)) = 0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200
			Cull Off //Fast way to turn your material double-sided
		Stencil {
			Ref 1
			Comp always
			Pass replace
		}
			CGPROGRAM
			#pragma surface surf Standard fullforwardshadows

			#pragma target 3.0

			sampler2D _MainTex;

			struct Input {
				float2 uv_MainTex;
			};

			//half _Glossiness;
		//	half _Metallic;
			//fixed4 _Color;
		//	fixed4 _EmissionColor;

			//Dissolve properties
			sampler2D _DissolveTexture;
			half _Amount;

			void surf(Input IN, inout SurfaceOutputStandard o) {

				//Dissolve function
				half dissolve_value = tex2D(_DissolveTexture, IN.uv_MainTex).r;
				clip(dissolve_value - _Amount);

				//Basic shader function
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) /** _Color*/;

				o.Albedo = c.rgb;
				
			//	o.Metallic = _Metallic;
			//	o.Smoothness = _Glossiness;
				o.Alpha = c.a;
			//	o.Emission = fixed3(1, 1, 1); *step(dissolve_value - _Amount, 0.05f);// *_EmissionColor; //emits white color with 0.05 border size

			
			}
			ENDCG
			}
				FallBack "Diffuse"
}