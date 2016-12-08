#version 330 core

in vec2 passTextureUV;
out vec4 out_Color;

uniform sampler2D textureSampler;

const float contrast = .3;

void main(void){
	vec4 color = texture2D(textureSampler, passTextureUV);

	out_Color = vec4((color.rgb - .5) * (1 + contrast) + .5, 1);
}
