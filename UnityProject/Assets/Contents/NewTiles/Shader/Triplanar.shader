//basic phong CG shader with spec map


Shader "CG Shaders/Phong/Phong Texture Triplanar"
{
	Properties
	{
		_diffuseColor("Diffuse Color", Color) = (1,1,1,1)
		_diffuseMap("Diffuse X", 2D) = "white" {}
		_diffuseMap2("Diffuse Y", 2D) = "white" {}
		_diffuseMap3("Diffuse Z", 2D) = "white" {}
		_FrenselPower ("Rim Power", Range(1.0, 10.0)) = 2.5
		_FrenselPower (" ", Float) = 2.5
		_rimColor("Rim Color", Color) = (1,1,1,1)
		_specularPower ("Specular Power", Range(1.0, 50.0)) = 10
		_specularPower (" ", Float) = 10
		_specularColor("Specular Color", Color) = (1,1,1,1)
		_normalMap("Normal / Specular (A) X", 2D) = "bump" {}	
		_normalMap2("Normal / Specular (A) Y", 2D) = "bump" {}	
		_normalMap3("Normal / Specular (A) Z", 2D) = "bump" {}			
		_uvBlend("UV Blend", Range(0,1)) = 0.5
		_uvBlend (" ", Float) = 0.5
		_uvScale("UV Scale", Range(0,5)) = 1.0
		_uvScale(" ", Float) = 1.0
	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" }
		Pass
		{

			Tags { "LightMode" = "ForwardBase" } 
            
			CGPROGRAM
			
			#pragma vertex vShader
			#pragma fragment pShader
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase
			//if you MUST compile for flash, you might have to remove some features
			//personally i'm not trying to render on a potato
			#pragma target 3.0
			
			uniform fixed3 _diffuseColor;
			uniform sampler2D _diffuseMap;
			uniform half4 _diffuseMap_ST;	
			uniform sampler2D _diffuseMap2;
			uniform half4 _diffuseMap2_ST;	
			uniform sampler2D _diffuseMap3;
			uniform half4 _diffuseMap3_ST;	
			uniform fixed4 _LightColor0; 
			uniform half _FrenselPower;
			uniform fixed4 _rimColor;
			uniform half _specularPower;
			uniform fixed3 _specularColor;
			uniform sampler2D _normalMap;
			uniform half4 _normalMap_ST;
			uniform sampler2D _normalMap2;
			uniform half4 _normalMap2_ST;
			uniform sampler2D _normalMap3;
			uniform half4 _normalMap3_ST;
			//uv attributes			
			fixed _uvBlend;
			half _uvScale;
			
			struct app2vert {
				float4 vertex 	: 	POSITION;
				fixed2 texCoord : 	TEXCOORD0;
				fixed4 normal 	:	NORMAL;
				fixed4 tangent : TANGENT;
				
			};
			struct vert2Pixel
			{
				float4 pos 						: 	SV_POSITION;
				//modifying the uvs to pass 3 coordinates
				half4 uvsXY							:	TEXCOORD0;
				half4 uvsZplusMask					:	TEXCOORD1;
				fixed3 normalDir					:	TEXCOORD2;	
				fixed3 binormalDir					:	TEXCOORD3;	
				fixed3 tangentDir					:	TEXCOORD4;	
				half3 posWorld						:	TEXCOORD5;	
				fixed3 viewDir						:	TEXCOORD6;
				fixed3 lighting						:	TEXCOORD7;
			};
			
			fixed lambert(fixed3 N, fixed3 L)
			{
				return saturate(dot(N, L));
			}
			fixed frensel(fixed3 V, fixed3 N, half P)
			{	
				return pow(1 - saturate(dot(V,N)), P);
			}
			fixed phong(fixed3 R, fixed3 L)
			{
				return pow(saturate(dot(R, L)), _specularPower);
			}
			vert2Pixel vShader(app2vert IN)
			{
				vert2Pixel OUT;
				float4x4 WorldViewProjection = UNITY_MATRIX_MVP;
				float4x4 WorldInverseTranspose = _World2Object; 
				float4x4 World = _Object2World;
							
				OUT.pos = mul(WorldViewProjection, IN.vertex);
				OUT.normalDir = normalize(mul(IN.normal, WorldInverseTranspose).xyz);
				OUT.tangentDir = normalize(mul(IN.tangent, WorldInverseTranspose).xyz);
				OUT.binormalDir = normalize(cross(OUT.normalDir, OUT.tangentDir)); 
				OUT.posWorld = mul(World, IN.vertex).xyz;
				OUT.viewDir = normalize( OUT.posWorld - _WorldSpaceCameraPos);
				
				//create a mask for the uvs based on world normals		
				half3 mask = saturate(abs(OUT.normalDir) - _uvBlend);
				half mask1D = mask.x +mask.y +mask.z;
				mask = mask / mask1D;
				//create a composite uv map based on vert position and uvScale
				half3 posMap = OUT.posWorld / _uvScale;
				OUT.uvsXY = half4(posMap.z, posMap.y, posMap.z, posMap.x);
				OUT.uvsZplusMask = half4(posMap.x, posMap.y, mask.x, mask.y);

				//vertex lights
				fixed3 vertexLighting = fixed3(0.0, 0.0, 0.0);
				//#ifdef VERTEXLIGHT_ON
				// for (int index = 0; index < 4; index++)
				//	{    						
				//		half3 vertexToLightSource = half3(unity_4LightPosX0[index], unity_4LightPosY0[index], unity_4LightPosZ0[index]) - OUT.posWorld;
				//		fixed attenuation  = (1.0/ length(vertexToLightSource)) *.5;	
				//		fixed3 diffuse = unity_LightColor[index].xyz * lambert(OUT.normalDir, normalize(vertexToLightSource)) * attenuation;
				//		vertexLighting = vertexLighting + diffuse;
				//	}
				//vertexLighting = saturate( vertexLighting );
				//#endif
				OUT.lighting = vertexLighting ;
				
				return OUT;
			}
			
			fixed4 pShader(vert2Pixel IN): COLOR
			{
			//return fixed4(1,1,1,1);
				//reassemble the mask
				fixed3 uvMask = fixed3(IN.uvsZplusMask.z , IN.uvsZplusMask.w, 1- (IN.uvsZplusMask.z + IN.uvsZplusMask.w));
				//x sample
				half2 normalUVs = TRANSFORM_TEX(IN.uvsXY.xy, _normalMap);
				fixed4 normalDX = tex2D(_normalMap, normalUVs);
				//y sample
				normalUVs = TRANSFORM_TEX(IN.uvsXY.zw, _normalMap2);
				fixed4 normalDY = tex2D(_normalMap2, normalUVs);
				//z sample
				normalUVs = TRANSFORM_TEX(IN.uvsZplusMask.xy, _normalMap3);
				fixed4 normalDZ = tex2D(_normalMap3, normalUVs);
				
				fixed4 normalD = (normalDX * uvMask.x) + (normalDY * uvMask.y) + (normalDZ * uvMask.z);
				normalD.xyz = (normalD.xyz * 2) - 1;
				
				
				//half3 normalDir = half3(2.0 * normalSample.xy - float2(1.0), 0.0);
				//deriving the z component
				//normalDir.z = sqrt(1.0 - dot(normalDir, normalDir));
               // alternatively you can approximate deriving the z component without sqrt like so:  
				//normalDir.z = 1.0 - 0.5 * dot(normalDir, normalDir);
				
				
				fixed3 normalDir = normalD.xyz;	
				fixed specMap = normalD.w;
				normalDir = normalize((normalDir.x * IN.tangentDir) + (normalDir.y * IN.binormalDir) + (normalDir.z * IN.normalDir));
	
				
				fixed3 ambientL = UNITY_LIGHTMODEL_AMBIENT.xyz;
	
				//Main Light calculation - includes directional lights
				half3 pixelToLightSource =_WorldSpaceLightPos0.xyz - (IN.posWorld*_WorldSpaceLightPos0.w);
				fixed attenuation  = lerp(1.0, 1.0/ length(pixelToLightSource), _WorldSpaceLightPos0.w);				
				fixed3 lightDirection = normalize(pixelToLightSource);
				fixed diffuseL = lambert(normalDir, lightDirection);				
				
				//rimLight calculation
				fixed rimLight = frensel(normalDir, -IN.viewDir, _FrenselPower);
				rimLight *= saturate(dot(fixed3(0,1,0),normalDir)* 0.5 + 0.5)* saturate(dot(fixed3(0,1,0),-IN.viewDir)+ 1.75);	
				fixed3 diffuse = _LightColor0.xyz * (diffuseL+ (rimLight * diffuseL) )* attenuation;
				rimLight *= (1-diffuseL);
				diffuse = saturate(IN.lighting + ambientL + diffuse+ (rimLight*_rimColor));
		
				fixed specularHighlight = phong(reflect(IN.viewDir , normalDir) ,lightDirection)*attenuation;
				
				fixed4 outColor;							
				half2 diffuseUVs = TRANSFORM_TEX(IN.uvsXY.xy, _diffuseMap);
				fixed3 texSampleX = tex2D(_diffuseMap, diffuseUVs);
				diffuseUVs = TRANSFORM_TEX(IN.uvsXY.zw, _diffuseMap2);
				fixed3 texSampleY = tex2D(_diffuseMap2, diffuseUVs);
				diffuseUVs = TRANSFORM_TEX(IN.uvsZplusMask.xy, _diffuseMap3);
				fixed3 texSampleZ = tex2D(_diffuseMap3, diffuseUVs);
				//mask and composite it
				fixed3 colorMap = (texSampleX * uvMask.x) + (texSampleY * uvMask.y) + (texSampleZ * uvMask.z);
				
				fixed3 diffuseS = (diffuse * colorMap) * _diffuseColor.xyz;
				fixed3 specular = (specularHighlight * _specularColor * specMap);
				outColor = fixed4( diffuseS + specular,1.0);
				return outColor;
			}
			
			ENDCG
		}	
		
		//the second pass for additional lights
		Pass
		{
			Tags { "LightMode" = "ForwardAdd" } 
			Blend One One 
			
			CGPROGRAM
			#pragma vertex vShader
			#pragma fragment pShader
			#include "UnityCG.cginc"
			//#pragma exclude_renderers flash
			
			uniform fixed3 _diffuseColor;
			uniform sampler2D _diffuseMap;
			uniform half4 _diffuseMap_ST;	
			uniform sampler2D _diffuseMap2;
			uniform half4 _diffuseMap2_ST;	
			uniform sampler2D _diffuseMap3;
			uniform half4 _diffuseMap3_ST;	
			uniform fixed4 _LightColor0; 
			uniform half _FrenselPower;
			uniform fixed4 _rimColor;
			uniform half _specularPower;
			uniform fixed3 _specularColor;
			uniform sampler2D _normalMap;
			uniform half4 _normalMap_ST;
			uniform sampler2D _normalMap2;
			uniform half4 _normalMap2_ST;
			uniform sampler2D _normalMap3;
			uniform half4 _normalMap3_ST;
			//uv attributes			
			fixed _uvBlend;
			half _uvScale;
			
			struct app2vert {
				float4 vertex 	: 	POSITION;
				fixed2 texCoord : 	TEXCOORD0;
				fixed4 normal 	:	NORMAL;
				fixed4 tangent : TANGENT;
				
			};
			struct vert2Pixel
			{
				float4 pos 						: 	SV_POSITION;
				//modifying the uvs to pass 3 coordinates
				half4 uvsXY							:	TEXCOORD0;
				half4 uvsZplusMask					:	TEXCOORD1;
				fixed3 normalDir					:	TEXCOORD2;	
				fixed3 binormalDir					:	TEXCOORD3;	
				fixed3 tangentDir					:	TEXCOORD4;	
				half3 posWorld						:	TEXCOORD5;	
				fixed3 viewDir						:	TEXCOORD6;
			};
			
			fixed lambert(fixed3 N, fixed3 L)
			{
				return saturate(dot(N, L));
			}			
			fixed phong(fixed3 R, fixed3 L)
			{
				return pow(saturate(dot(R, L)), _specularPower);
			}
			vert2Pixel vShader(app2vert IN)
			{
				vert2Pixel OUT;
				float4x4 WorldViewProjection = UNITY_MATRIX_MVP;
				float4x4 WorldInverseTranspose = _World2Object; 
				float4x4 World = _Object2World;
				
				OUT.pos = mul(WorldViewProjection, IN.vertex);
				
				//derived vectors
				//construct the derived vectors and pass to the pixel shader
				//Transorm the world Normal into world space
				OUT.normalDir = normalize(mul(IN.normal, WorldInverseTranspose).xyz);
				OUT.tangentDir = normalize(mul(IN.tangent, WorldInverseTranspose).xyz);
				//Unity does not provide biNormals, so we must calculate via crossProduct
				OUT.binormalDir = normalize(cross(OUT.normalDir, OUT.tangentDir)); 
				OUT.posWorld = mul(World, IN.vertex).xyz;
				OUT.viewDir = normalize( OUT.posWorld - _WorldSpaceCameraPos);
				
				//create a mask for the uvs based on world normals		
				half3 mask = saturate(abs(OUT.normalDir) - _uvBlend);
				half mask1D = mask.x +mask.y +mask.z;
				mask = mask / mask1D;
				//create a composite uv map based on vert position and uvScale
				half3 posMap = OUT.posWorld / _uvScale;
				OUT.uvsXY = half4(posMap.z, posMap.y, posMap.z, posMap.x);
				OUT.uvsZplusMask = half4(posMap.x, posMap.y, mask.x, mask.y);

				return OUT;
			}
			fixed4 pShader(vert2Pixel IN): COLOR
			{
				//return fixed4(1,1,1,1);
				//reassemble the mask
				fixed3 uvMask = fixed3(IN.uvsZplusMask.z , IN.uvsZplusMask.w, 1- (IN.uvsZplusMask.z + IN.uvsZplusMask.w));
				//x sample
				half2 normalUVs = TRANSFORM_TEX(IN.uvsXY.xy, _normalMap);
				fixed4 normalDX = tex2D(_normalMap, normalUVs);
				//y sample
				normalUVs = TRANSFORM_TEX(IN.uvsXY.zw, _normalMap2);
				fixed4 normalDY = tex2D(_normalMap2, normalUVs);
				//z sample
				normalUVs = TRANSFORM_TEX(IN.uvsZplusMask.xy, _normalMap3);
				fixed4 normalDZ = tex2D(_normalMap3, normalUVs);
				
				fixed4 normalD = (normalDX * uvMask.x) + (normalDY * uvMask.y) + (normalDZ * uvMask.z);
				normalD.xyz = (normalD.xyz * 2) - 1;
				
				//half3 normalDir = half3(2.0 * normalSample.xy - float2(1.0), 0.0);
				//deriving the z component
				//normalDir.z = sqrt(1.0 - dot(normalDir, normalDir));
      		       // alternatively you can approximate deriving the z component without sqrt like so: 
				//normalDir.z = 1.0 - 0.5 * dot(normalDir, normalDir);
				
				//pull the alpha out for spec before modification
				fixed3 normalDir = normalD.xyz;	
				fixed specMap = normalD.w;
				normalDir = normalize((normalDir.x * IN.tangentDir) + (normalDir.y * IN.binormalDir) + (normalDir.z * IN.normalDir));
				
				//Moving Fill lights to the Pixel Shader				
				//Fill lights
				half3 pixelToLightSource = _WorldSpaceLightPos0.xyz- (IN.posWorld*_WorldSpaceLightPos0.w);
				fixed attenuation  = lerp(1.0, 1.0/ length(pixelToLightSource), _WorldSpaceLightPos0.w);				
				fixed3 lightDirection = normalize(pixelToLightSource);
				
				fixed diffuseL = lambert(normalDir, lightDirection);				
				fixed3 diffuseTotal = _LightColor0.xyz * diffuseL * attenuation;
			
				//Moving specular highlight to the Pixel Shader
				//specular highlight
				fixed specularHighlight = phong(reflect(IN.viewDir , normalDir) ,lightDirection)*attenuation;
				
				fixed4 outColor;							
				half2 diffuseUVs = TRANSFORM_TEX(IN.uvsXY.xy, _diffuseMap);
				fixed3 texSampleX = tex2D(_diffuseMap, diffuseUVs).xyz;
				diffuseUVs = TRANSFORM_TEX(IN.uvsXY.zw, _diffuseMap2);
				fixed3 texSampleY = tex2D(_diffuseMap2, diffuseUVs).xyz;
				diffuseUVs = TRANSFORM_TEX(IN.uvsZplusMask.xy, _diffuseMap3);
				fixed3 texSampleZ = tex2D(_diffuseMap3, diffuseUVs).xyz;
				//mask and composite it
				fixed3 colorMap = (texSampleX * uvMask.x) + (texSampleY * uvMask.y) + (texSampleZ * uvMask.z);
				
				fixed3 diffuseS = (diffuseTotal * colorMap) * _diffuseColor.xyz;
				fixed3 specular = (specularHighlight * _specularColor * specMap);
				outColor = fixed4( diffuseS + specular,1.0);
				return outColor;
			}
			
			ENDCG
		}	
		
	}
}