#version 330 core

in vec2 passTextureUV;
in vec3 reflectedVector;
in vec3 refractedVector;

out vec4 out_Color;

uniform vec4 color = vec4(-1, -1, -1, -1);
uniform sampler2D textureSampler;
uniform samplerCube reflectiveCubeMap;
uniform samplerCube refractiveCubeMap;
uniform int hasTexture;
uniform int hasReflectiveCubeMap;
uniform int hasRefractiveCubeMap;

vec4 meshColor();

void main(void){
	out_Color = meshColor();
}

vec4 meshColor(){
	if(color == vec4(-1, -1, -1, -1) 
		&& hasTexture == 0 
		&& hasReflectiveCubeMap == 0
		&& hasRefractiveCubeMap == 0){

		return (vec4(1, 1, 1, 1));
	}

	vec4 textureColor = texture2D(textureSampler, passTextureUV);
	vec4 reflectiveColor = texture(reflectiveCubeMap, reflectedVector);
	vec4 refractiveColor = texture(refractiveCubeMap, refractedVector);

	vec4 combinedColor = vec4(0, 0, 0, 0);
	int totalColors = hasTexture + hasReflectiveCubeMap + hasRefractiveCubeMap;

	if(color != vec4(-1, -1, -1, -1)){
		combinedColor += color;
		totalColors++;
	}
	//Textures
	if(hasTexture != 0)
		combinedColor += textureColor;
	if(hasReflectiveCubeMap != 0)
		combinedColor += reflectiveColor;
	if(hasRefractiveCubeMap != 0)
		combinedColor += refractiveColor;

	return combinedColor / totalColors;
}

