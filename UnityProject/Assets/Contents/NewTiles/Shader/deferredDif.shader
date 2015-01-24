Shader "Custom/DeferredDiffuse"
{
Properties 
{
	_AColor ("Diffuse Color", Color) = (1.0, 1.0, 1.0, 1.0)
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_BumpTex ("Bumpmap", 2D) = "white" {}
	_BumpValue ("Bump value", float) = 1
}
SubShader 
{
	Tags { "RenderType"="Opaque" }
	LOD 200
	
	CGPROGRAM
	#pragma surface surf MyDiffuse vertex:vert
    #include "UnityCG.cginc"
    
	float4 _AColor;
	sampler _MainTex; 
	float4 _MainTex_ST;
	sampler _BumpTex;
	float _TexScale;
	float _BumpValue;

	struct Input
	{
	   	float3 myVertex;
	    float3 myNormal;
	};

	void vert(inout appdata_full v, out Input o)
	{
	    o.myNormal = v.normal;
		o.myVertex = mul(_Object2World, v.vertex);
	}

	void surf (Input IN, inout SurfaceOutput o)
	{
		float4 Dcolor = {0.2, 0.2, 0.2, 1.0};

	   	float3 blend_weights = abs(IN.myNormal);
	   	blend_weights = blend_weights - 0.2679f;
	   	blend_weights = max(blend_weights, 0);
	   	//force sum to 1.0
	   	blend_weights /= (blend_weights.x + blend_weights.y + blend_weights.z).xxx;
	
	   	float4 blended_color;
	  	float3 blended_bump_vec;
	
		
	   	float2 coord1 = IN.myVertex.zy * _MainTex_ST.xy + _MainTex_ST.zw;
	   	float2 coord2 = IN.myVertex.zx * _MainTex_ST.xy + _MainTex_ST.zw;
	   	float2 coord3 = IN.myVertex.xy * _MainTex_ST.xy + _MainTex_ST.zw;
	
	   	float2 bumpFetch1 = _BumpValue * tex2D(_BumpTex,coord1).wy * 2 - 1;
	   	float2 bumpFetch2 = _BumpValue * tex2D(_BumpTex,coord2).wy * 2 - 1;
	   	float2 bumpFetch3 = _BumpValue * tex2D(_BumpTex,coord3).wy * 2 - 1;
	
	   	float3 bump1 = float3(0, bumpFetch1.x, bumpFetch1.y); 
	   	float3 bump2 = float3(bumpFetch2.y, 0, bumpFetch2.x); 
	   	float3 bump3 = float3(bumpFetch3.x, bumpFetch3.y, 0);
	
	   	float4 col1 = tex2D(_MainTex, coord1);
	   	float4 col2 = tex2D(_MainTex, coord2);
	   	float4 col3 = tex2D(_MainTex, coord3);
	
	  	blended_color = col1.xyzw * blend_weights.xxxx + 
	    	col2.xyzw * blend_weights.yyyy + 
	    	col3.xyzw * blend_weights.zzzz;

	  	blended_bump_vec = bump1.xyz * blend_weights.xxx + 
	 		bump2.xyz * blend_weights.yyy + 
	   		bump3.xyz * blend_weights.zzz;
	 
	  	o.Albedo = blended_color * _AColor;
	   	//o.Normal = blended_bump_vec;
	}
	
	float4 LightingMyDiffuse_PrePass(SurfaceOutput i, float4 light)
	{
		return float4(i.Albedo * light.rgb, 1.0);
	}
	ENDCG
} 
}