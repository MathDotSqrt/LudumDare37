#version 330 core

in vec2 passTextureUV;
out vec4 out_Color;

uniform sampler2D textureSampler;

void main(void){
	vec4 color = texture2D(textureSampler, passTextureUV);
	float brightness = (color.r * .2126) + (color.g * .7152) + (color.b * .0722) / 2;
	
	//out_Color = color * brightness;
	if(brightness > .7){
		out_Color = color * brightness;
	}
	else{
		out_Color = vec4(0);
	}
}