#version 330 core

layout (location = 0) in vec3 position;
layout (location = 1) in vec2 textureUV;
layout (location = 2) in vec3 normal;
layout (location = 3) in vec3 tangent;


uniform mat4 transformationMatrix;
uniform mat4 viewMatrix;
uniform mat4 projectionMatrix;
//uniform mat3 transposedTransformation;
//uniform vec3 cameraPosition;
uniform float refractionIndex;
uniform int pointLightCount = 0;
uniform vec3 pointLightPositions[4];


out vec2 passTextureUV;
out vec3 fragPosition;
out vec3 toCamera;
out vec3 toLightVector[4];

void main(void){
	vec4 worldPosition = transformationMatrix * vec4(position, 1);	

	mat4 modelViewMatrix = viewMatrix * transformationMatrix;
	vec4 positionRelativeToCam = modelViewMatrix * vec4(position, 1);
	vec3 surfaceNormal = (modelViewMatrix * vec4(normal, 0.0)).xyz;  
	

	vec3 norm = normalize(surfaceNormal);
	vec3 tang = normalize((modelViewMatrix * vec4(tangent, 0)).xyz);
	vec3 bitang = normalize(cross(norm, tang));

	mat3 toTangentSpace = mat3(
		tang.x, bitang.x, norm.x,
		tang.y, bitang.y, norm.y,
		tang.z, bitang.z, norm.z
	);

	for(int i = 0; i < pointLightCount; i++){
		toLightVector[i] = toTangentSpace * (pointLightPositions[i] - positionRelativeToCam.xyz);
	}

	passTextureUV = textureUV;
	toCamera = toTangentSpace * -positionRelativeToCam.xyz;

	gl_Position = projectionMatrix * positionRelativeToCam;
}