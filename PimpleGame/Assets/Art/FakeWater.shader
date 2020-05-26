Shader "Unlit/SpecialFX/FakeWater"
{
    Properties
    {
        _Tint ("Tint", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _FillMultiplier ("Fill Multiplier", Float) = 1
        _FillAmount ("Fill Amount", Range(0,1)) = 0.0
        [HideInInspector] _WobbleX ("WobbleX", Range(-1,1)) = 0.0
        [HideInInspector] _WobbleZ ("WobbleZ", Range(-1,1)) = 0.0
        _TopColor ("Top Color", Color) = (1,1,1,1)
        _FoamColor ("Foam Line Color", Color) = (1,1,1,1)
        _FoamLineWidth ("Foam Line Width", Range(0,0.1)) = 0.0    
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0,10)) = 0.0
    }
 
    SubShader
    {
        Tags {"Queue"="Transparent"  "DisableBatching" = "True" }
 
                Pass
        {
         Zwrite On
         Cull Off // we want the front and back faces
         AlphaToMask On // transparency
         Blend SrcAlpha OneMinusSrcAlpha
 
         CGPROGRAM
 
 
         #pragma vertex vert
         #pragma fragment frag
         // make fog work
         #pragma multi_compile_fog
           
         #include "UnityCG.cginc"
 
         struct appdata
         {
           float4 vertex : POSITION;
           float2 uv : TEXCOORD0;
           float3 normal : NORMAL; 
         };
 
         struct v2f
         {
            float2 uv : TEXCOORD0;
            UNITY_FOG_COORDS(1)
            float4 vertex : SV_POSITION;
            float3 viewDir : COLOR;
            float3 normal : COLOR2;
         };
 
         sampler2D _MainTex;
         float4 _MainTex_ST;
         float _FillAmount, _WobbleX, _WobbleZ;
         float4 _TopColor, _RimColor, _FoamColor, _Tint;
         float _FoamLineWidth, _RimPower;
         float _FillMultiplier;
           
         float4 RotateAroundYInDegrees (float4 vertex, float degrees)
         {
            float alpha = degrees * UNITY_PI / 180;
            float sina, cosa;
            sincos(alpha, sina, cosa);
            float2x2 m = float2x2(cosa, sina, -sina, cosa);
            return float4(vertex.yz , mul(m, vertex.xz)).xzyw ;            
         }
     
 
         v2f vert (appdata v)
         {
            v2f o;
 
            o.vertex = UnityObjectToClipPos(v.vertex);
            o.uv = TRANSFORM_TEX(v.uv, _MainTex);
            UNITY_TRANSFER_FOG(o,o.vertex);        
 
            o.viewDir = normalize(ObjSpaceViewDir(v.vertex));
            o.normal = v.normal;
            return o;
         }
           
         float4 frag (v2f i, fixed facing : VFACE) : SV_Target
         {
           // sample the texture
           fixed4 col = tex2D(_MainTex, i.uv) * _Tint;
           // apply fog
           UNITY_APPLY_FOG(i.fogCoord, col);
           
           float yPortion = (1 - i.uv.y) + (_WobbleX * ((i.uv.x - 0.5) * 2));
           float fillEdge = step(yPortion, _FillAmount * _FillMultiplier);
           float foamEdge = step(_FillAmount, yPortion) - step(_FoamLineWidth, yPortion - _FillAmount);
           
           // rim light
           float dotProduct = 1 - pow(dot(i.normal, i.viewDir), _RimPower);
           float4 RimResult = smoothstep(0.5, 1.0, dotProduct);
           RimResult *= _RimColor;
 
           // foam edge
           float4 foamColored = _FoamColor * step(0.5, foamEdge);
           // rest of the liquid
           float4 resultColored = col * step(0.5, fillEdge);
           // both together, with the texture
           float4 finalResult = resultColored + foamColored;
           finalResult.rgb += RimResult;
 
           // color of backfaces/ top
           float4 topColor = _TopColor * (fillEdge + foamEdge);
           //VFACE returns positive for front facing, negative for backfacing
           return facing > 0 ? finalResult: topColor;
           
               
         }
         ENDCG
        }
 
    }
}