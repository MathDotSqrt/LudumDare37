#version 330 core

in vec2 passTextureUV;
out vec4 out_Color;

uniform sampler2D textureAtlas;
uniform vec3 color = vec3(1, 1, 1);

const float width = .5;
const float edge = .1;

void main(void){
    float distance = 1.0 -  texture(textureAtlas, passTextureUV).a;
	float alpha = 1.0 - smoothstep(width, width + edge, distance);
    
    out_Color = vec4(color, alpha);
}