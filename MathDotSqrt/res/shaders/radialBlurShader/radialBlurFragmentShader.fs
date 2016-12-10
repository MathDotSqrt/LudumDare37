#version 330 core

in vec2 passTextureUV;

out vec4 out_Color;


const float exposure = .01;
const float decay = 1;
const float density = .9;
const float weight = 1;
const int NUM_SAMPLES = 100 ;

uniform sampler2D textureSampler;
uniform vec2 lightPositionOnScreen;

void main()
{	

	vec2 deltaTextCoord = vec2( passTextureUV - lightPositionOnScreen.xy );
	vec2 textCoo = passTextureUV;
	deltaTextCoord *= 1.0 /  float(NUM_SAMPLES) * density;
	float illuminationDecay = 1.0;


	for(int i=0; i < NUM_SAMPLES ; i++)
    {
             textCoo -= deltaTextCoord;
             vec4 sample = texture2D(textureSampler, textCoo );
		
             sample *= illuminationDecay * weight;

             gl_FragColor += sample;

             illuminationDecay *= decay;
     }
     gl_FragColor *= exposure;
}