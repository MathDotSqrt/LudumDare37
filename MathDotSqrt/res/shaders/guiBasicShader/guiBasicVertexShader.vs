#version 330 core

in vec2 position;
out vec2 passTextureUV;

uniform mat4 transformationMatrix;

void main(void){
	gl_Position = transformationMatrix * vec4(position, 0, 1);

	passTextureUV = vec2((position.x + 1) / 2, 1 - (position.y + 1) / 2);
}