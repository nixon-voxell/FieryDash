Shader "Transitions/NoiseWarp"
{
  Properties
  {
    _Tex0 ("Texture0", 2D) = "black" {}
    _Tex1 ("Texture1", 2D) = "black" {}
    _Transition ("Transition", Float) = 0.0
  }

  SubShader
  {
    Tags
    {
      "Queue" = "Transparent"
      "IgnoreProjector" = "True"
      "RenderType"="Transparent"
    }

    LOD 100
    ZWrite Off
    Blend SrcAlpha OneMinusSrcAlpha
    Cull Off

    Pass
    {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      struct appdata
      {
        float4 vertex : POSITION;
        float2 uv : TEXCOORD0;
      };

      struct v2f
      {
        float2 uv0 : TEXCOORD0;
        float2 uv1 : TEXCOORD1;
        float4 vertex : SV_POSITION;
      };

      sampler2D _Tex0;
      float4 _Tex0_ST;

      sampler2D _Tex1;
      float4 _Tex1_ST;

      float _Transition;

      float hash(float n) { return frac(sin(n) * 1e4); }
      float hash(float2 p) { return frac(1e4 * sin(17.0 * p.x + p.y * 0.1) * (0.1 + abs(sin(p.y * 13.0 + p.x)))); }

      float hnoise(float2 x)
      {
        float2 i = floor(x);
        float2 f = frac(x);

        float a = hash(i);
        float b = hash(i + float2(1.0, 0.0));
        float c = hash(i + float2(0.0, 1.0));
        float d = hash(i + float2(1.0, 1.0));

        float2 u = f * f * (3.0 - 2.0 * f);
        return lerp(a, b, u.x) + (c - a) * u.y * (1.0 - u.x) + (d - b) * u.x * u.y;
      }

      v2f vert (appdata v)
      {
        v2f o;
        o.vertex = UnityObjectToClipPos(v.vertex);
        o.uv0 = TRANSFORM_TEX(v.uv, _Tex0);
        o.uv1 = TRANSFORM_TEX(v.uv, _Tex1);
        return o;
      }

      float4 frag (v2f i) : SV_Target
      {
        float hn = hnoise(i.uv0 / 100.0);
        float2 d = float2(0.0, normalize(float2(0.5, 0.5) - i.uv0).y);

        i.uv0 = i.uv0 + d * _Transition / 5.0 * (1.0 + hn / 2.0);
        i.uv1 = i.uv1 - d * (1.0 - _Transition) / 5.0 * (1.0 + hn / 2.0);

        float4 col0 = tex2D(_Tex0, i.uv0);
        float4 col1 = tex2D(_Tex1, i.uv1);
        float4 col = lerp(col0, col1, _Transition);

        return col;
      }
      ENDCG
    }
  }
}