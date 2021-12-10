Shader "Custom/SliceShader" {
    Properties{
      _Color("Color", Color) = (1,1,1,1)
      _MainTex("Albedo (RGB)", 2D) = "white" {}
      _Glossiness("Smoothness", Range(0,1)) = 0.5
      _Metallic("Metallic", Range(0,1)) = 0.0
      _BumpMap("Bumpmap", 2D) = "bump" {}
      _Cube("Cubemap", CUBE) = "" {}
    }
        SubShader{
          Cull Off
          Tags { "RenderType" = "Opaque" }

          CGPROGRAM
          #pragma surface surf Standard fullforwardshadows
          #pragma target 3.0
          struct Input {
              float2 uv_MainTex;
              float2 uv_BumpMap;
              float3 worldRefl;
              float3 worldPos;

              INTERNAL_DATA
          };
          sampler2D _MainTex;
          half _Glossiness;
          half _Metallic;
          fixed4 _Color;
          sampler2D _BumpMap;
          samplerCUBE _Cube;

          uniform fixed3 g_PlanePositions[6];
          uniform fixed3 g_PlaneNormals[6];
          uniform float g_PlaneSliceFlag[6];

          fixed3 g_MaxXPos;
          fixed3 g_MinXPos;
          fixed3 g_MaxYPos;
          fixed3 g_MinYPos;
          fixed3 g_MaxZPos;
          fixed3 g_MinZPos;

          fixed3 g_MaxXNor;
          fixed3 g_MinXNor;
          fixed3 g_MaxYNor;
          fixed3 g_MinYNor;
          fixed3 g_MaxZNor;
          fixed3 g_MinZNor;

          UNITY_INSTANCING_BUFFER_START(Props)
          UNITY_INSTANCING_BUFFER_END(Props)

          bool checkBoxVisability(fixed3 worldPos, fixed3 pos, fixed3 nor, int index)
          {
              float dotProd = dot(worldPos - pos, nor);
              return dotProd > 0;
          }

          bool checkPlaneVisability(fixed3 worldPos, int index)
          {
              float dotProd = dot(worldPos - g_PlanePositions[index], g_PlaneNormals[index]);

              if (g_PlaneSliceFlag[index] == 0.0)
                  return false;
              else
                  return dotProd > 0;
          }

          void surf(Input IN, inout SurfaceOutputStandard o)
          {
              fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
              o.Albedo = c.rgb;
              o.Metallic = _Metallic;
              o.Smoothness = _Glossiness;
              o.Alpha = c.a;
              o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));

              // Box
              if (checkBoxVisability(IN.worldPos, g_MaxXPos, g_MaxXNor, 0))
                  discard;
              if (checkBoxVisability(IN.worldPos, g_MaxYPos, g_MaxYNor, 1))
                  discard;
              if (checkBoxVisability(IN.worldPos, g_MaxZPos, g_MaxZNor, 2))
                  discard;
              if (checkBoxVisability(IN.worldPos, g_MinXPos, g_MinXNor, 3))
                  discard;
              if (checkBoxVisability(IN.worldPos, g_MinYPos, g_MinYNor, 4))
                  discard;
              if (checkBoxVisability(IN.worldPos, g_MinZPos, g_MinZNor, 5))
                  discard;

              // Plane
              if (checkPlaneVisability(IN.worldPos, 0))
                  discard;
              if (checkPlaneVisability(IN.worldPos, 1))
                  discard;
              if (checkPlaneVisability(IN.worldPos, 2))
                  discard;
              if (checkPlaneVisability(IN.worldPos, 3))
                  discard;
              if (checkPlaneVisability(IN.worldPos, 4))
                  discard;
              if (checkPlaneVisability(IN.worldPos, 5))
                  discard;
          }
          ENDCG
    }
        Fallback "Diffuse"
}