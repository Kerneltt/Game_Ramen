// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Rod/SimpleDiffuseReflect"
{
	Properties
	{
		_DiffuseColor("DiffuseColor", Color) = (0.8018868,0.6493883,0.2609915,0)
		_Diffuse("Diffuse", 2D) = "white" {}
		_DiffuseMult("DiffuseMult", Int) = 2
		_Shininess("Shininess", Range( 0.01 , 1)) = 0.1
		_LightIntensity("LightIntensity", Float) = 0.5
		_Cubemap("Cubemap", CUBE) = "white" {}
		[Normal]_Normal("Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
	}
	
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		CGINCLUDE
		#pragma target 3.0
		ENDCG
		Blend Off
		Cull Back
		ColorMask RGBA
		ZWrite On
		ZTest LEqual
		Offset 0 , 0
		
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "AutoLight.cginc"
			#include "UnityShaderVariables.cginc"
			#define ASE_SHADOWS 1


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_tangent : TANGENT;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_SHADOW_COORDS(5)
				float4 ase_lmap : TEXCOORD6;
				float4 ase_sh : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				UNITY_VERTEX_OUTPUT_STEREO
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			//This is a late directive
			
			uniform sampler2D _Diffuse;
			uniform float4 _Diffuse_ST;
			uniform sampler2D _Normal;
			uniform float4 _Normal_ST;
			uniform float _Shininess;
			uniform float4 _DiffuseColor;
			uniform samplerCUBE _Cubemap;
			uniform int _DiffuseMult;
			uniform float _LightIntensity;
			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.ase_texcoord1.xyz = ase_worldPos;
				float3 ase_worldTangent = UnityObjectToWorldDir(v.ase_tangent);
				o.ase_texcoord2.xyz = ase_worldTangent;
				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord3.xyz = ase_worldNormal;
				float ase_vertexTangentSign = v.ase_tangent.w * unity_WorldTransformParams.w;
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord4.xyz = ase_worldBitangent;
				#ifdef DYNAMICLIGHTMAP_ON //dynlm
				o.ase_lmap.zw = v.texcoord2.xy * unity_DynamicLightmapST.xy + unity_DynamicLightmapST.zw;
				#endif //dynlm
				#ifdef LIGHTMAP_ON //stalm
				o.ase_lmap.xy = v.texcoord1.xy * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif //stalm
				#ifndef LIGHTMAP_ON //nstalm
				#if UNITY_SHOULD_SAMPLE_SH //sh
				o.ase_sh.xyz = 0;
				#ifdef VERTEXLIGHT_ON //vl
				o.ase_sh.xyz += Shade4PointLights (
				unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
				unity_LightColor[0].rgb, unity_LightColor[1].rgb, unity_LightColor[2].rgb, unity_LightColor[3].rgb,
				unity_4LightAtten0, ase_worldPos, ase_worldNormal);
				#endif //vl
				o.ase_sh.xyz = ShadeSHPerVertex (ase_worldNormal, o.ase_sh.xyz);
				#endif //sh
				#endif //nstalm
				float3 shadeVertexLight25 = ShadeVertexLightsFull(float4( v.vertex.xyz , 0.0 ),v.ase_normal,4,false);
				float3 vertexToFrag28 = shadeVertexLight25;
				o.ase_texcoord8.xyz = vertexToFrag28;
				
				o.ase_texcoord.xyz = v.ase_texcoord.xyz;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord.w = 0;
				o.ase_texcoord1.w = 0;
				o.ase_texcoord2.w = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				o.ase_sh.w = 0;
				o.ase_texcoord8.w = 0;
				
				v.vertex.xyz +=  float3(0,0,0) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				fixed4 finalColor;
				float2 uv_Diffuse = i.ase_texcoord.xyz * _Diffuse_ST.xy + _Diffuse_ST.zw;
				float4 tex2DNode2 = tex2D( _Diffuse, uv_Diffuse );
				float4 temp_cast_0 = (( tex2DNode2.a * tex2DNode2.a )).xxxx;
				float4 temp_output_43_0_g1 = temp_cast_0;
				float3 ase_worldPos = i.ase_texcoord1.xyz;
				float3 ase_worldViewDir = UnityWorldSpaceViewDir(ase_worldPos);
				ase_worldViewDir = normalize(ase_worldViewDir);
				float3 worldSpaceLightDir = UnityWorldSpaceLightDir(ase_worldPos);
				float3 normalizeResult4_g2 = normalize( ( ase_worldViewDir + worldSpaceLightDir ) );
				float2 uv_Normal = i.ase_texcoord.xyz.xy * _Normal_ST.xy + _Normal_ST.zw;
				float3 tex2DNode5 = UnpackNormal( tex2D( _Normal, uv_Normal ) );
				float3 ase_worldTangent = i.ase_texcoord2.xyz;
				float3 ase_worldNormal = i.ase_texcoord3.xyz;
				float3 ase_worldBitangent = i.ase_texcoord4.xyz;
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal12_g1 = tex2DNode5;
				float3 worldNormal12_g1 = float3(dot(tanToWorld0,tanNormal12_g1), dot(tanToWorld1,tanNormal12_g1), dot(tanToWorld2,tanNormal12_g1));
				float3 normalizeResult64_g1 = normalize( worldNormal12_g1 );
				float dotResult19_g1 = dot( normalizeResult4_g2 , normalizeResult64_g1 );
				#if defined(LIGHTMAP_ON) && UNITY_VERSION < 560 //aselc
				float4 ase_lightColor = 0;
				#else //aselc
				float4 ase_lightColor = _LightColor0;
				#endif //aselc
				UNITY_LIGHT_ATTENUATION(ase_atten, i, ase_worldPos)
				float3 temp_output_40_0_g1 = ( ase_lightColor.rgb * ase_atten );
				float dotResult14_g1 = dot( normalizeResult64_g1 , worldSpaceLightDir );
				UnityGIInput data34_g1;
				UNITY_INITIALIZE_OUTPUT( UnityGIInput, data34_g1 );
				#if defined(LIGHTMAP_ON) || defined(DYNAMICLIGHTMAP_ON) //dylm34_g1
				data34_g1.lightmapUV = i.ase_lmap;
				#endif //dylm34_g1
				#if UNITY_SHOULD_SAMPLE_SH //fsh34_g1
				data34_g1.ambient = i.ase_sh;
				#endif //fsh34_g1
				UnityGI gi34_g1 = UnityGI_Base(data34_g1, 1, normalizeResult64_g1);
				float3 worldRefl17 = normalize( reflect( -ase_worldViewDir, float3( dot( tanToWorld0, tex2DNode5 ), dot( tanToWorld1, tex2DNode5 ), dot( tanToWorld2, tex2DNode5 ) ) ) );
				float4 temp_output_42_0_g1 = ( _DiffuseColor * ( tex2DNode2 * ( ( ( tex2DNode2 * 0.5 ) + ( ( 1.0 - 0.5 ) * texCUBE( _Cubemap, worldRefl17 ) ) ) * _DiffuseMult ) ) );
				float3 vertexToFrag28 = i.ase_texcoord8.xyz;
				
				
				finalColor = float4( ( ( ( (temp_output_43_0_g1).rgb * (temp_output_43_0_g1).a * pow( max( dotResult19_g1 , 0.0 ) , ( _Shininess * 128.0 ) ) * temp_output_40_0_g1 ) + ( ( ( temp_output_40_0_g1 * max( dotResult14_g1 , 0.0 ) ) + gi34_g1.indirect.diffuse ) * (temp_output_42_0_g1).rgb ) ) + ( vertexToFrag28 * _LightIntensity ) ) , 0.0 );
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback "Unlit/Texture"
}
/*ASEBEGIN
Version=16200
7;44;1687;906;2423.04;782.1886;1;True;True
Node;AmplifyShaderEditor.SamplerNode;5;-2559.148,-143.2035;Float;True;Property;_Normal;Normal;9;1;[Normal];Create;True;0;0;False;0;e95970a5f2b7f774b92fa93331fa21c5;e95970a5f2b7f774b92fa93331fa21c5;True;0;True;bump;Auto;True;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;14;-1857.874,-315.5455;Float;False;Constant;_Float0;Float 0;-1;0;Create;True;0;0;False;0;0.5;0;0;0.5;0;1;FLOAT;0
Node;AmplifyShaderEditor.WorldReflectionVector;17;-2212.197,62.29214;Float;False;True;1;0;FLOAT3;0,0,0;False;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.OneMinusNode;15;-1747.198,-49.70778;Float;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;2;-1997.775,-660.8837;Float;True;Property;_Diffuse;Diffuse;1;0;Create;True;0;0;False;0;31c8039d245a2044ea3924329a2bbda6;31c8039d245a2044ea3924329a2bbda6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;16;-1878.245,133.614;Float;True;Property;_Cubemap;Cubemap;8;0;Create;True;0;0;False;0;f8f9f6273d0ce3a48a335f2fb79f07d2;f8f9f6273d0ce3a48a335f2fb79f07d2;True;0;False;white;Auto;False;Object;-1;Auto;Cube;6;0;SAMPLER2D;0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT;1;False;3;FLOAT3;0,0,0;False;4;FLOAT3;0,0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;12;-1553.899,-49.70778;Float;False;2;2;0;FLOAT;0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1541.353,-323.6748;Float;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;19;-1305.699,-72.1282;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.IntNode;32;-1247.054,208.9993;Float;False;Property;_DiffuseMult;DiffuseMult;2;0;Create;True;0;0;False;0;2;2;0;1;INT;0
Node;AmplifyShaderEditor.NormalVertexDataNode;26;-1401.268,531.0414;Float;False;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PosVertexDataNode;27;-1417.267,353.4417;Float;False;0;0;5;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1064.997,-80.04201;Float;True;2;2;0;COLOR;0,0,0,0;False;1;INT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;23;-1097.468,-772.9681;Float;False;Property;_DiffuseColor;DiffuseColor;0;0;Create;True;0;0;False;0;0.8018868,0.6493883,0.2609915,0;1,0.8389567,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ShadeVertexLightsHlpNode;25;-1092.664,421.1456;Float;False;4;False;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-858.2276,-286.8758;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.VertexToFragmentNode;28;-747.9981,505.6905;Float;False;1;0;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.RangedFloatNode;31;-435.5572,281.379;Float;False;Property;_LightIntensity;LightIntensity;7;0;Create;True;0;0;False;0;0.5;0.5;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;-754.1926,-516.3817;Float;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;24;-1529.559,-701.6865;Float;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;30;-191.6254,423.6725;Float;False;2;2;0;FLOAT3;0,0,0;False;1;FLOAT;0.5;False;1;FLOAT3;0
Node;AmplifyShaderEditor.FunctionNode;3;-284.1266,-142.7061;Float;True;Blinn-Phong Light;3;;1;cf814dba44d007a4e958d2ddd5813da6;0;3;42;COLOR;0,0,0,0;False;52;FLOAT3;0,0,0;False;43;COLOR;0,0,0,0;False;2;FLOAT3;0;FLOAT;57
Node;AmplifyShaderEditor.SimpleAddOpNode;29;211.3843,168.4714;Float;True;2;2;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;1;FLOAT3;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;1;677.7563,256.9815;Float;False;True;2;Float;ASEMaterialInspector;0;1;Rod/SimpleDiffuseReflect;0770190933193b94aaa3065e307002fa;0;0;Unlit;2;True;0;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;True;0;False;-1;0;False;-1;True;False;True;0;False;-1;True;True;True;True;True;0;False;-1;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;1;RenderType=Opaque=RenderType;True;2;0;False;False;False;False;False;False;False;False;False;False;False;0;Unlit/Texture;0;0;Standard;0;2;0;FLOAT4;0,0,0,0;False;1;FLOAT3;0,0,0;False;0
WireConnection;17;0;5;0
WireConnection;15;0;14;0
WireConnection;16;1;17;0
WireConnection;12;0;15;0
WireConnection;12;1;16;0
WireConnection;13;0;2;0
WireConnection;13;1;14;0
WireConnection;19;0;13;0
WireConnection;19;1;12;0
WireConnection;33;0;19;0
WireConnection;33;1;32;0
WireConnection;25;0;27;0
WireConnection;25;1;26;0
WireConnection;20;0;2;0
WireConnection;20;1;33;0
WireConnection;28;0;25;0
WireConnection;21;0;23;0
WireConnection;21;1;20;0
WireConnection;24;0;2;4
WireConnection;24;1;2;4
WireConnection;30;0;28;0
WireConnection;30;1;31;0
WireConnection;3;42;21;0
WireConnection;3;52;5;0
WireConnection;3;43;24;0
WireConnection;29;0;3;0
WireConnection;29;1;30;0
WireConnection;1;0;29;0
ASEEND*/
//CHKSM=8010CAA1750AF890AF0F4B5E687BCB11FA4E4C89