//Version of shader that will not factor in Light Mapping
Shader "MWM/Reflective/Reflective Bump Emissive" {
	Properties {
		_MainTex ("Diffuse(RGB), Gloss(A)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_EmitTex ("Emission", 2D) = "black" {}
		_Cube ("Cube Map", CUBE) = "" {}
		_RflPower ("Reflectivity", Float) = 1
		_emitColour ("Emission Colour", Color) = (1,1,1,1)
		_EmitPow ("Emission Power", Float) = 1
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf Lambert noambient
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
			INTERNAL_DATA
		};
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _EmitTex;
		samplerCUBE _Cube;
		fixed _SpecPower;
		fixed _RflPower;
		fixed4 _emitColour;
		fixed _EmitPow;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 eTex = tex2D (_EmitTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Emission = ((_EmitPow * eTex)* _emitColour) + (_RflPower * tex.a * texCUBE (_Cube, WorldReflectionVector(IN, o.Normal)).rgb);
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}