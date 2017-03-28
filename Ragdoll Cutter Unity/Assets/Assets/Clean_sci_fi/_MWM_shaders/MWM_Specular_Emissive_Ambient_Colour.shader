Shader "MWM/Specular/Specular Emissive Ambient Colour" {
	Properties {
		_MainTex ("Diffuse(RGB), Gloss(A)", 2D) = "white" {}
		_EmitTex ("Self-Illumin", 2D) = "black" {}
		_AmbColor ("Ambient Value", Color) = (0,0,0,0)
		_SpecColor ("Specular Colour", Color) = (1,1,1,1)
		_Shininess ("Shininess",Range (0.05, 2.0) ) = 0.25
		_emitColour ("Emission Colour", Color) = (1,1,1,1)
		_EmitPow ("Emission Power", Float) = 0
	}
	SubShader {
		Tags {"Rendertype" = "Opaque"}
		CGPROGRAM
		
		#pragma surface surf BlinnPhong noambient
		#pragma exclude_renderers flash
		
		struct Input {
			fixed2 uv_MainTex;
			fixed3 worldRefl;
		};
		
		sampler2D _MainTex;
		sampler2D _EmitTex;
		fixed _Shininess;
		fixed4 _emitColour;
		fixed4 _AmbColor;
		fixed _EmitPow;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 tex = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 eTex = tex2D (_EmitTex, IN.uv_MainTex);
			//o.Albedo = tex.rgb + (eTex * _emitColour).rgb;
			o.Albedo = tex.rgb;
			o.Specular = _Shininess;
			o.Gloss = tex.a;
			o.Emission = (_AmbColor *tex.rgb) + ((_EmitPow * eTex)* _emitColour);
		}
		
		ENDCG
	}
	Fallback "Specular"
}