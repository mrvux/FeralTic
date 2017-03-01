
cbuffer cbCamera : register(b0)
{
    float4x4 tVP : VIEWPROJECTION;
};

cbuffer cbObject : register(b1)
{
    float4x4 tW : WORLD;
};

cbuffer cbObject : register(b0)
{
    float4 color;
};

void VS(float4 posObject : POSITION, out float4 posScreen : SV_Position)
{
    posScreen = mul(posObject, mul(tW, tVP));

}

float4 PS(float4 posScreen : SV_Position) : SV_Target
{
    return color;
}

technique10 Render
{
	pass P0
	{
		SetVertexShader( CompileShader( vs_5_0, VS() ) );
		SetPixelShader( CompileShader(ps_5_0, PS()));
	}
}