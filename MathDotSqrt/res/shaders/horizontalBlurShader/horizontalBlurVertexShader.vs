#version 330 core

in vec2 position;
out vec2 blurTextureCoords[11];

uniform float targetWidth;

void main(void){
	gl_Position = vec4(position, 0, 1);

	vec2 centerTexture = position / 2 + .5;
	float pixelSize = 1 / targetWidth;

	for(int i = -5; i <= 5; i++){
		blurTextureCoords[i + 5] = centerTexture + vec2(pixelSize * i, 0);
	}
}