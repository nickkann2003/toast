// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Jam"
{
    Properties
    {
        [IntRange] _StencilID ("Stencil ID", Range(0, 255)) = 0
        _Tint ("Tint", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Opaque"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Geometry"
        }

        Pass 
        {
            CGPROGRAM

            #pragma vertex MyVertexProgram
			#pragma fragment MyFragmentProgram
            
            #include "UnityCG.cginc"

            float4 _Tint;
            sampler2D _MainTex;

            struct Interpolators {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
//				float3 localPosition : TEXCOORD0;
			};

            struct VertexData {
				float4 position : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			Interpolators MyVertexProgram (
				float4 position : POSITION,
				float2 uv : TEXCOORD0
			) {
				Interpolators i;
				//i.localPosition = position.xyz;
				i.position = UnityObjectToClipPos(position);
				return i;
			}

			Interpolators MyVertexProgram (VertexData v) {
				Interpolators i;
//				i.localPosition = v.position.xyz;
				i.position = UnityObjectToClipPos(v.position);
				i.uv = v.uv;
				return i;
			}


            float4 MyFragmentProgram (Interpolators i) : SV_TARGET {
				return tex2D(_MainTex, i.uv);
			}


            //Blend Zero One
            //ZWrite Off
            //
            //Stencil 
            //{
            //    Ref [_StencilID]
            //    Comp Always
            //    Pass Replace
            //    Fail Keep
            //}
			ENDCG
        }
    }
}
