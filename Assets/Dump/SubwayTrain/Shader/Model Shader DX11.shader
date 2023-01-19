Shader "Model Shader" 

{

Properties 

{

  //_Color ("Color", Color) = (1,1,1)

  _MainTex ("Diffuse Map", 2D) = "white" {}

  _BumpMap ("Normal Map", 2D) = "bump" {}

  _SpecMap ("Specular Color Map", 2D) = "white" {}

  _Detail ("Detail Map", 2D) = "gray" {}

  _RimColor ("Rim Color", Color) = (0.26,0.19,0.16,0.0)

  _RimPower ("Rim Power", Range(0.5,8.0)) = 3.0

}





SubShader 

{



 

    

    Tags { "RenderType" = "Opaque" }



CGPROGRAM

#pragma surface surf ColoredSpecular 

#pragma target 3.0 

#pragma only_renderers d3d11   

		

//#pragma surface surf Lambert

         

 

struct MySurfaceOutput 



{

	half3 Albedo;

	half3 Normal;

	half3 Emission;

	half Specular;

	half3 GlossColor;

	half Alpha;

	

};





inline half4 LightingColoredSpecular (MySurfaceOutput s, half3 lightDir, half3 viewDir, half atten)



{

  half3 h = normalize (lightDir + viewDir);



  half diff = max (0, dot (s.Normal, lightDir));



  float nh = max (0, dot (s.Normal, h));

  float spec = pow (nh, 32.0);

  half3 specCol = spec * s.GlossColor;



  half4 c;

  c.rgb = (s.Albedo * _LightColor0.rgb * diff + _LightColor0.rgb * specCol) * (atten * 2);

  c.a = s.Alpha;

  return c;

}





inline half4 LightingColoredSpecular_PrePass (MySurfaceOutput s, half4 light)



{

	half3 spec = light.a * s.GlossColor;

	

	half4 c;

	c.rgb = (s.Albedo * light.rgb + light.rgb * spec);

	c.a = s.Alpha + spec * _SpecColor.a;

	return c;

}









//fake sss not working

 inline half4 LightingWrapLambert (SurfaceOutput s, half3 lightDir, half atten) 



{

          half NdotL = dot (s.Normal, lightDir);

          half diff = NdotL * 0.5 + 0.5;

          half4 c;

          c.rgb = s.Albedo * _LightColor0.rgb * (diff * atten * 2);

          c.a = s.Alpha;

          return c;

}



struct Input 



{

  float2 uv_MainTex;

  float2 uv_SpecMap;

  float2 uv_BumpMap;

  float2 uv_Detail;

  float3 viewDir;  // Needed For Rim Lighting  

};







sampler2D _MainTex;

sampler2D _SpecMap;

sampler2D _BumpMap;

sampler2D _Detail;

float4 _RimColor;

float _RimPower;







  void surf (Input IN, inout MySurfaceOutput o)

  

{



  o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;

  //o.Alpha = tex2D(_MainTex, IN.uv_MainTex).a;

  o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * 0.4;

  half4 spec = tex2D (_SpecMap, IN.uv_SpecMap);

  o.GlossColor = spec.rgb;

  

  o.Specular = 32.0/128.0;

  o.Albedo *= tex2D (_Detail, IN.uv_Detail).rgb * 2.5;

  o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));  

  //rim lighting settings

  half rim = 1.0 - saturate(dot (normalize(IN.viewDir), o.Normal));

  o.Emission = _RimColor.rgb * pow (rim, _RimPower);

}





ENDCG

}

Fallback "Transparent/Cutout/Bumped Specular"

}
