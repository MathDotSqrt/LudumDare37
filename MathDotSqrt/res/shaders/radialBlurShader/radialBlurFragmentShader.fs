#version 330 core

in vec2 passTextureUV;
out vec4 out_Color;


const float exposure = .005;
const float decay = 1;
const float density = .8;
const float weight = 5.65;
const vec2 lightPositionOnScreen = vec2(1.1, .77);
uniform sampler2D textureSampler;
const int NUM_SAMPLES = 100 ;

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