#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
uniform sampler2D ColorTexture;

sampler2D SpriteTextureSampler = sampler_state {
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput {
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR {
	float4 texCol = tex2D(SpriteTextureSampler, input.TextureCoordinates);
	float4 colCol = tex2D(ColorTexture, input.TextureCoordinates);
	float4 inCol = input.Color;
	//if (colCol.a > 0.95f) {
		return texCol * colCol * inCol;
	//} else {
	//	return texCol * inCol;
	//}
}

technique SpriteDrawing {
	pass P0 {
		PixelShader = compile PS_SHADERMODEL MainPS( );
	}
};
