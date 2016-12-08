#version 330 core

in vec3 surfaceNormal;

out vec4 out_Color;

void main(void){
	vec3 unitNormal = normalize(surfaceNormal);
	out_Color = vec4(abs(unitNormal), 1);
}