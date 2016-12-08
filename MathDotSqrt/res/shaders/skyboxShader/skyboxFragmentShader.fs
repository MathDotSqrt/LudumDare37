#version 330 core

in vec3 passTexture;

out vec4 out_Color;

uniform samplerCube cubeMap;
uniform int hasTexture;

void main(void){

	if(hasTexture == 0){
		out_Color = vec4(1, .5, .5, 1);
		return;
	}

	out_Color = texture(cubeMap, passTexture);
}