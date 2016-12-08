#version 330 core

in vec2 passTextureUV;
out vec4 out_Color;

uniform sampler2D textureSampler;
uniform int hasTexture = 0;
uniform vec4 color = vec4(-1, -1, -1, -1);

void main(void){
	vec4 textureColor = texture2D(textureSampler, passTextureUV);

	vec4 tempColor;
	if(hasTexture == 0 && color == vec4(-1, -1, -1, -1))
		tempColor = vec4(1, 1, 1, 1);
	else if(hasTexture == 0)
		tempColor = color;
	else if(color == vec4(-1, -1, -1, -1))
		tempColor = textureColor;
	else 
		tempColor = (color + textureColor) / 2;

	if(tempColor.a < .05)
		discard;

	out_Color = tempColor;
}