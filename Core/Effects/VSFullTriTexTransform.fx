

struct vs2ps
{
    float4 position: SV_POSITION;
    float2 uv : TEXCOORD0;
};

cbuffer cbTexTransform : register(b0)
{
    float4x4 tTex;
}


vs2ps VS( uint vertexID : SV_VertexID )
{
    vs2ps result;

    float2 uv = float2((vertexID << 1) & 2, vertexID & 2);
    result.position = float4(uv * float2(2.0f, -2.0f) + float2(-1.0f, 1.0f), 0.0f, 1.0f);
    result.uv = mul(float4(uv, 0.0f, 1.0f), tTex).xy;
    
	return result;
}

technique10 Apply
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
	}
}
