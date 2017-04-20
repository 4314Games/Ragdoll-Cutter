Shader "MWM/Reflective/Reflective Bump Emissive Ambient Cube Cutout" {
	Properties {
		//_MainTex ("Diffuse(RGB), Cutout(A)", 2D) = "white" {}
		//_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) TransGloss (A)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_EmitTex ("Self-Illumin", 2D) = "black" {}
		_AmbCube ("Ambient Cube Map", CUBE) = "" {}
		_RefCube ("Reflection Cube Map", CUBE) = "" {}
		_AmbPower ("Ambient Power", Float) = 0
		_RflPower ("Reflectivity", Float) = 1
		_emitColour ("Emission Colour", Color) = (1,1,1,1)
		_EmitPow ("Emission Power", Float) = 0
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf Lambert noambient alphatest:_Cutoff
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
			fixed3 worldNormal;
			INTERNAL_DATA
		};
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _EmitTex;
		samplerCUBE _AmbCube;
		samplerCUBE _RefCube;
		fixed _SpecPower;
		fixed _RflPower;
		fixed _AmbPower;
		fixed4 _emitColour;
		fixed _EmitPow;
		//fixed4 _Color;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 eTex = tex2D (_EmitTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			//o.Gloss = tex.a;
			o.Gloss = tex.a;
			//o.Alpha = tex.a * _Color.a;
			o.Alpha = tex.a;
			o.Emission = tex.rgb * (_AmbPower * texCUBE (_AmbCube, WorldNormalVector (IN, o.Normal)).rgb) + _RflPower * tex.a * texCUBE (_RefCube, WorldReflectionVector(IN, o.Normal)).rgb + ((_EmitPow * eTex)* _emitColour);
		}
		
		ENDCG
	}
	Fallback "Diffuse"
}