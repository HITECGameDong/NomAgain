Shader "Custom/ObstacleOutline"
{
    Properties
    {
        // Outline 관련 property 추가
        _OutlineColor("Outline Color", Color) = (1, 0, 0, 1)
        _OutlineThickness("Outline Thickness", Float) = 0.1
    }

    SubShader
    {
        Pass
        {
            Name "Outline"
            Tags
            {
                "LightMode" = "Outline"
            }

            Cull Front

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float3 normalOS     : NORMAL;
            };

            struct Varyings
            {
                float4 positionHCS  : SV_POSITION;
            };

            half4 _OutlineColor;
            half _OutlineThickness;

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                float3 positionWS = TransformObjectToWorld(IN.positionOS.xyz + IN.normalOS.xyz * _OutlineThickness);
                OUT.positionHCS = TransformWorldToHClip(positionWS);

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                return _OutlineColor;
            }
            ENDHLSL
        }
    }
}
