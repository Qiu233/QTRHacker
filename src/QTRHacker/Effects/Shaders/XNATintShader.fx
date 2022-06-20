sampler2D input : register(S0);
float4 color : register(C0);

float4 main(float2 uv : TEXCOORD) : COLOR{
  return tex2D(input, uv) * color;
}
