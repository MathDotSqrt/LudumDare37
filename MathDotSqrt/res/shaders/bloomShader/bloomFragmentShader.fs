#version 330 core

in vec2 passTextureUV;
out vec4 out_Color;

uniform sampler2D textureSampler;
uniform sampler2D blurTextureSampler;

void main(void){
	vec4 sceneColor = texture2D(textureSampler, passTextureUV);
	vec4 blurColor = texture2D(blurTextureSampler, passTextureUV);

	out_Color = sceneColor + blurColor * 1;
}
