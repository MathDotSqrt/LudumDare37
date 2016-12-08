#version 330 core

in vec3 position;
in vec2 textureUV;
in vec3 normal;

out vec2 passTextureUV;
out vec3 reflectedVector;
out vec3 refractedVector;

uniform mat4 transformationMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
uniform vec3 cameraPosition;
uniform mat3 transposedTransformation;
uniform float refractionIndex;

void main(void){
	vec4 worldPosition = transformationMatrix * vec4(position, 1);
	gl_Position = projectionMatrix * viewMatrix * worldPosition;

	passTextureUV = textureUV;

	vec3 viewVector = normalize(worldPosition.xyz - cameraPosition);
	vec3 surfaceNormal = normalize(transposedTransformation * normal);
	reflectedVector = reflect(viewVector, surfaceNormal);
	refractedVector = refract(viewVector, surfaceNormal, refractionIndex);
}