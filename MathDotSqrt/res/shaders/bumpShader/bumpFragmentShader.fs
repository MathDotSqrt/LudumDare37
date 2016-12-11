#version 330 core

in vec2 passTextureUV;
in vec3 toCamera;
in vec3 toLightVector[4];

uniform sampler2D textureSampler;
uniform sampler2D normalSampler;

uniform vec3 ambientLightColor = vec3(0, 0, 0);
uniform float ambientLightIntensity = 0;

uniform int pointLightCount = 0;
uniform vec3 pointLightColors[4];
uniform float pointLightIntensity[4];

uniform float specularShininess = 30;
uniform vec3 specularColor = vec3(1, 1, 1);

out vec4 out_Color;

vec4 meshColor();

void main(void){
	vec4 normalMapValue = 2 * texture2D(normalSampler, passTextureUV) - 1;

	vec3 unitNormal = normalize(normalMapValue.rgb);
	vec3 unitToCameraVector = normalize(toCamera);

	vec3 diffuse = vec3(0, 0, 0);
   
	//ambientLight calculation
	diffuse += (ambientLightColor * ambientLightIntensity);

	for(int i = 0; i < pointLightCount; i++){
		//pointLight calculation
		vec3 unitToLightVector = normalize(toLightVector[i]);//normalize(pointLightPositions[i] - fragPosition);
		float nDotl = dot(unitNormal, unitToLightVector);
		float brightness = max(nDotl, 0)  * pointLightIntensity[i];
		
		diffuse += brightness * pointLightColors[i];

		//specular calculation
		vec3 reflectedLightVector = reflect(unitToLightVector, unitNormal);
		float cDotl = dot(unitToCameraVector, -reflectedLightVector); 
		float specular = pow(max(cDotl, 0), specularShininess);

		diffuse += specular * specularColor;
	}

	out_Color = vec4(diffuse, 1) * texture2D(textureSampler, passTextureUV);
	//out_Color = normalMapValue;
}