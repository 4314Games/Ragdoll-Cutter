//Version of the shader that will not factor in the Lightmapper
Shader "MWM/Specular/Specular Bump Emissive" {
	Properties {
		_MainTex ("Diffuse(RGB), Gloss(A)", 2D) = "white" {}
		_BumpMap ("Normal", 2D) = "bump" {}
		_EmitTex ("Emission", 2D) = "black" {}
		_SpecColor ("Specular Colour", Color) = (1,1,1,1)
		_Shininess ("Shininess",Range (0.05, 2.0) ) = 0.25
		_emitColour ("Emission Colour", Color) = (1,1,1,1)
		_EmitPow ("Emission Power", Float) = 1
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		#pragma surface surf BlinnPhong
		//#pragma surface surf BlinnPhong dualforward
		//halfasview = include this directive for performance boost!
		#pragma exclude_renderers flash
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
		};
		
		sampler2D _MainTex;
		sampler2D _BumpMap;
		sampler2D _EmitTex;
		fixed _Shininess;
		fixed4 _emitColour;
		fixed _EmitPow;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 eTex = tex2D (_EmitTex, IN.uv_MainTex);
			o.Albedo = tex.rgb;
			o.Normal = UnpackNormal (tex2D (_BumpMap, IN.uv_MainTex));
			o.Specular = _Shininess;
			o.Gloss = tex.a;
			o.Emission = ((_EmitPow * eTex) * _emitColour).rgb;
		}
		
		ENDCG
	}
	Fallback "Specular"
}