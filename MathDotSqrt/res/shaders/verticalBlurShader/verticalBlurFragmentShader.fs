#version 330 core

in vec2 blurTextureCoords[11];
out vec4 out_Color;

uniform sampler2D textureSampler;

void main(void){
	//out_Color = texture2D(textureSampler, passTextureUV);
	//out_Color = vec4(0);
	
	out_Color += texture2D(textureSampler, blurTextureCoords[0]) * 0.0093;
	out_Color += texture2D(textureSampler, blurTextureCoords[1]) * 0.028002;
	out_Color += texture2D(textureSampler, blurTextureCoords[2]) * 0.065984;
	out_Color += texture2D(textureSampler, blurTextureCoords[3]) * 0.121703;
	out_Color += texture2D(textureSampler, blurTextureCoords[4]) * 0.175713;
	out_Color += texture2D(textureSampler, blurTextureCoords[5]) * 0.198596;
	out_Color += texture2D(textureSampler, blurTextureCoords[6]) * 0.175713;
	out_Color += texture2D(textureSampler, blurTextureCoords[7]) * 0.121703;
	out_Color += texture2D(textureSampler, blurTextureCoords[8]) * 0.065984;
	out_Color += texture2D(textureSampler, blurTextureCoords[9]) * 0.028002;
	out_Color += texture2D(textureSampler, blurTextureCoords[10]) * 0.0093;

}
// 0.0093	
// 0.028002	
// 0.065984	
// 0.121703	
// 0.175713	
// 0.198596	
// 0.175713	
// 0.121703	
// 0.065984	
// 0.028002	
// 0.0093
