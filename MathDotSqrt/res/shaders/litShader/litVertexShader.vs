#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 textureUV;
layout (location = 2) in vec3 normal;

uniform mat4 transformationMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform mat3 transposedTransformation;
uniform vec3 cameraPosition;
uniform float refractionIndex;

out vec2 passTextureUV;
out vec3 fragPosition;
out vec3 surfaceNormal;
out vec3 reflectedVector;
out vec3 refractedVector;

void main(void){
	vec4 worldPosition = transformationMatrix * vec4(position, 1);	
	gl_Position = projectionMatrix * viewMatrix * worldPosition;
	
	passTextureUV = textureUV;
	fragPosition = worldPosition.xyz;
	surfaceNormal = normalize(transposedTransformation * normal);

	vec3 viewVector = normalize(worldPosition.xyz - cameraPosition);
	reflectedVector = reflect(viewVector, surfaceNormal);
	refractedVector = refract(viewVector, surfaceNormal, refractionIndex);
}