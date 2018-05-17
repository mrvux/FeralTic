Texture2D tex : TEXTURE; 

SamplerState linSamp
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Clamp;
    AddressV = Clamp;
};


struct vs2ps
{
    float4 position: SV_POSITION;
    float2 uv : TEXCOORD0;
};


vs2ps VSFullTri( uint vertexID : SV_VertexID )
{
    vs2ps result;
    result.uv = float2((vertexID << 1) & 2, vertexID & 2);
    result.position = float4(result.uv * float2(2.0f, -2.0f) + float2(-1.0f, 1.0f), 0.0f, 1.0f);
	return result;
}

float4 PS(vs2ps input): SV_Target
{
    return tex.Sample( linSamp, input.uv);
}


technique10 FullScreenTriangleVSOnly
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VSFullTri() ) );
	}
}

technique10 FullScreenTriangle
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_4_0, VSFullTri() ) );
		SetPixelShader( CompileShader(ps_4_0, PS()));
	}
}