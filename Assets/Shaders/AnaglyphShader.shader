Shader"Hidden/AnaglyphShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}       // Main texture for the left eye
        _MainTex2 ("2nd Texture", 2D) = "white" {} // Texture for the right eye
        _ConvergenceDistance ("Convergence Distance", float) = 1  // Convergence distance for anaglyph effect
    }

    SubShader
    {
        // No culling or depth
Cull Off
ZWrite Off
ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

#include "UnityCG.cginc"

uniform sampler2D _MainTex; // Sampler for the left eye texture
uniform sampler2D _MainTex2; // Sampler for the right eye texture
uniform float _ConvergenceDistance; // Convergence distance value

            // Input structure for vertex shader
struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

            // Output structure for vertex shader
struct v2f
{
    float2 uv : TEXCOORD0;
    float4 vertex : SV_POSITION;
};

            // Vertex shader function
v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

            // Fragment (pixel) shader function
fixed4 frag(v2f i) : SV_Target
{
                // Sample colors from the left and right eye textures
    fixed4 sideA = tex2D(_MainTex, i.uv);
    fixed4 sideB = tex2D(_MainTex2, i.uv);
    
                // Invert the colors for anaglyph effect
    fixed3 red = fixed3(sideA.r, 0, 0);
    fixed3 cyan = fixed3(0, sideB.g, sideB.b);
                
                // Combine the inverted colors and apply convergence distance
    fixed4 col = fixed4(red + cyan, sideA.a);
    
    col.rgb *= _ConvergenceDistance;
    
    return col;
}
            ENDCG
        }
    }
}
