struct vs2ps
{
    float4 position: SV_POSITION;
};


vs2ps VS(uint vertexID : SV_VertexID )
{
    vs2ps result;
    float2 uv = float2((vertexID << 1) & 2, vertexID & 2);
    result.position = float4(uv * float2(2.0f, -2.0f) + float2(-1.0f, 1.0f), 0.0f, 1.0f);
	return result;
}

technique10 FullScreenTriangleVSOnly
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
	}
}