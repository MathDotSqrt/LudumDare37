#version 330 core

in vec2 position;
in vec2 textureUV;

out vec2 passTextureUV;

uniform mat4 transformationMatrix;


void main(void){
	gl_Position = transformationMatrix * vec4(position, 0, 1);

	passTextureUV = textureUV;
}
