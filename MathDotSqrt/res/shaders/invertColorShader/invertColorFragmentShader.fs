#version 330 core

in vec2 passTextureUV;
out vec4 out_Color;

uniform sampler2D textureSampler;

void main(void){
	out_Color = 1 - texture2D(textureSampler, passTextureUV);
}
