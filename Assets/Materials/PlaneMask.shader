Shader "Custom/PlaneMask"{
    Properties{
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Albedo (RGB)", 2D) = "white" {}
        _Glossiness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" "Queue" = "Geometry+1" }
            LOD 200
            Stencil
            {
                Ref 1
                Comp NotEqual
                Pass keep
            }

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows
            #pragma target 3.0

            sampler2D _MainTex;

            struct Input {
                float2 uv_MainTex;
            };

            half _Glossiness;
            half _Metallic;
            fixed4 _Color;

            void surf(Input IN, inout SurfaceOutputStandard o) {

                fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = c.rgb;

                o.Metallic = _Metallic;
                o.Smoothness = _Glossiness;
                o.Alpha = c.a;
            }
            ENDCG
        }
            FallBack "Diffuse"
}

//    SubShader{
//        Tags { "RenderType" = "Opaque" "Queue" = "Transparent-1"}
//        colormask 0
//        Pass {
//            Stencil {
//                Ref 2
//                Comp Greater
//                Pass Replace
//            }
//
//            CGPROGRAM
//            #pragma vertex vert
//            #pragma fragment frag
//            struct appdata {
//                float4 vertex : POSITION;
//            };
//            struct v2f {
//                float4 pos : SV_POSITION;
//            };
//            v2f vert(appdata v) {
//                v2f o;
//                o.pos = UnityObjectToClipPos(v.vertex);
//                return o;
//            }
//            half4 frag(v2f i) : SV_Target {
//                return half4(1,0,0,1);
//            }
//            ENDCG
//        }
//    }
//}