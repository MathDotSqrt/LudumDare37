#version 330 core

in vec2 passTextureUV;
in vec3 fragPosition;
in vec3 surfaceNormal;
in vec3 toCamera;
in vec3 reflectedVector;
in vec3 refractedVector;

uniform vec4 color = vec4(-1, -1, -1, -1);
uniform sampler2D textureSampler;
uniform samplerCube reflectiveCubeMap;
uniform samplerCube refractiveCubeMap;
uniform int hasTexture;
uniform int hasReflectiveCubeMap;
uniform int hasRefractiveCubeMap;

uniform vec3 ambientLightColor = vec3(0, 0, 0);
uniform float ambientLightIntensity = 0;

uniform int pointLightCount = 0;
uniform vec3 pointLightPositions[4];
uniform vec3 pointLightColors[4];
uniform float pointLightIntensity[4];

uniform float specularShininess = 30;
uniform vec3 specularColor = vec3(1, 1, 1);

out vec4 out_Color;

vec4 meshColor();

void main(void){
	vec3 unitNormal = normalize(surfaceNormal);
	vec3 unitToCameraVector = normalize(toCamera - fragPosition);

	vec3 diffuse = vec3(0, 0, 0);
   
	//ambientLight calculation
	diffuse += (ambientLightColor * ambientLightIntensity);

	for(int i = 0; i < pointLightCount; i++){
		//pointLight calculation
		vec3 unitToLightVector = normalize(pointLightPositions[i] - fragPosition);
		float nDotl = dot(unitNormal, unitToLightVector);
		float brightness = max(nDotl, 0)  * pointLightIntensity[i];
		
		diffuse += brightness * pointLightColors[i];

		//specular calculation
		vec3 reflectedLightVector = reflect(unitToLightVector, unitNormal);
		float cDotl = dot(unitToCameraVector, -reflectedLightVector); 
		float specular = pow(max(cDotl, 0), specularShininess);

		diffuse += specular * specularColor;
	}

	out_Color = vec4(diffuse, 1) * meshColor();
}

vec4 meshColor(){
	if(color == vec4(-1, -1, -1, -1) 
		&& hasTexture == 0 
		&& hasReflectiveCubeMap == 0
		&& hasRefractiveCubeMap == 0){

		return (vec4(1, 1, 1, 1));
	}

	vec4 textureColor = texture2D(textureSampler, passTextureUV);
	vec4 reflectiveColor = texture(reflectiveCubeMap, -reflectedVector);
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