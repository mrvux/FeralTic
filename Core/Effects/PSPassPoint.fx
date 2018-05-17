Texture2D inputTexture : TEXTURE; 

SamplerState pointSampler
{
    Filter = MIN_MAG_MIP_POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};

float4 PS(float4 position: SV_POSITION, float2 uv : TEXCOORD0): SV_Target
{
    return inputTexture.Sample(pointSampler, uv);
}


technique10 Apply
{
	pass P0
	{
        SetPixelShader(CompileShader(ps_4_0, PS()));
	}
}
