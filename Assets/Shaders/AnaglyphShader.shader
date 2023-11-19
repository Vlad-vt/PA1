Shader"Hidden/AnaglyphShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MainTex2 ("2nd Texture", 2D) = "white" {}
        _ConvergenceDistance ("Convergence Distance", float) = 1 

    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform sampler2D _MainTex;
            uniform sampler2D _MainTex2;
            uniform float _ConvergenceDistance;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 sideA = tex2D(_MainTex, i.uv);
                fixed4 sideB = tex2D(_MainTex2, i.uv);
    
    
                // just invert the colors
                fixed3 red = fixed3(sideA.r, 0, 0);
                
                fixed3 cyan = fixed3(0, sideB.g, sideB.b);
                
                fixed4 col = fixed4(red + cyan, sideA.a);
                
    
                col.rgb *= _ConvergenceDistance;
    
                return col;
            }
            ENDCG
        }
    }
}
