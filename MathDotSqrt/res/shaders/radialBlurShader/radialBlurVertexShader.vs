#version 330 core

in vec2 position;
out vec2 passTextureUV;

void main(void){
	gl_Position = vec4(position, 0, 1);


	passTextureUV = vec2((position.x + 1) / 2, (position.y + 1) / 2);
}