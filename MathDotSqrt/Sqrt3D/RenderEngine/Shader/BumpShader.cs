using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World.Objects;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class BumpShader : ShaderProgram{
		private const string SHADER_PATH = @"bumpShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"bumpVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"bumpFragmentShader.fs";

		public BumpShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
			base.BindVertexArrayAttr(VAOAttribLocation.Texture_UV, "textureUV");
			base.BindVertexArrayAttr(VAOAttribLocation.Normal, "normal");
			base.BindVertexArrayAttr(VAOAttribLocation.Tangent, "tangent");
		}
		protected override void BindAllUniformLocations() {
			base.AddUniform("transformationMatrix");
			base.AddUniform("viewMatrix");
			base.AddUniform("projectionMatrix");
			//base.AddUniform("transposedTransformation");
			//base.AddUniform("cameraPosition");

			base.AddUniform("textureSampler");
			base.AddUniform("normalSampler");

			base.AddUniform("ambientLightColor");
			base.AddUniform("ambientLightIntensity");

			base.AddUniform("pointLightCount");
			base.AddUniform("pointLightPositions");
			base.AddUniform("pointLightColors");
			base.AddUniform("pointLightIntensity");

			base.AddUniform("specularShininess");
			base.AddUniform("specularColor");

		}

		public void ConnectTextures() {
			base.LoadInt(uniforms["textureSampler"], 0);
			base.LoadInt(uniforms["normalSampler"], 1);
		}

		public void LoadTransformationMatrix(Matrix4 m) {
			base.LoadMatrix4(uniforms["transformationMatrix"], m);
			//Matrix4 inverse = Matrix4.Invert(m);
			//Matrix4 transpose = Matrix4.Transpose(inverse);
			//Matrix3 mat3 = new Matrix3(transpose);
			//base.LoadMatrix3(uniforms["transposedTransformation"], mat3);
		}
		public void LoadCamera(Camera camera) {
			base.LoadMatrix4(uniforms["viewMatrix"], camera.InverseTransformationMatrix);
			base.LoadMatrix4(uniforms["projectionMatrix"], camera.ProjectionMatrix);
			//base.LoadVector3(uniforms["cameraPosition"], camera.WorldSpacePostion);
		}
		
		public void LoadTexture(int texture) {
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, texture);

		}

		public void LoadNormal(int normal) {
			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, normal);
		}

		public void LoadPointLights(List<Light> lights, Camera camera) {
			if(lights == null || lights.Count == 0)
				return;

			float[] positionArray = new float[lights.Count * 3];
			float[] colorArray = new float[lights.Count * 3];
			float[] intensity = new float[lights.Count];

			for(int i = 0; i < lights.Count; i++) {
				Vector3 position = ( lights[i].Parent == null ? lights[i].Position : lights[i].WorldSpacePostion );

				Vector4 eyeSpacePos = new Vector4(position.X, position.Y, position.Z, 1f);
				eyeSpacePos = Vector4.Transform(eyeSpacePos, camera.InverseTransformationMatrix);

				positionArray[i * 3 + 0] = eyeSpacePos.X;
				positionArray[i * 3 + 1] = eyeSpacePos.Y;
				positionArray[i * 3 + 2] = eyeSpacePos.Z;

				colorArray[i * 3 + 0] = lights[i].Color.R;
				colorArray[i * 3 + 1] = lights[i].Color.G;
				colorArray[i * 3 + 2] = lights[i].Color.B;

				intensity[i] = lights[i].Intensity;
			}

			base.LoadInt(uniforms["pointLightCount"], lights.Count);
			base.LoadVector3Array(uniforms["pointLightPositions"], positionArray);
			base.LoadVector3Array(uniforms["pointLightColors"], colorArray);
			base.LoadFloatArray(uniforms["pointLightIntensity"], intensity);
		}
		public void LoadAmbientLights(List<Light> lights) {
			if(lights == null || lights.Count == 0)
				return;

			List<Color> colors = new List<Color>();

			float avgIntensity = 0;
			foreach(Light light in lights) {
				colors.Add(light.Color);
				avgIntensity += light.Intensity;
			}

			Color color = Color.BlendColors(colors);
			avgIntensity /= lights.Count;

			base.LoadVector3(uniforms["ambientLightColor"], color.RGBA.Xyz);
			base.LoadFloat(uniforms["ambientLightIntensity"], avgIntensity);
		}
		public void LoadSpecularShininess(float shininess) {
			if(shininess <= 0)
				return;

			base.LoadFloat(uniforms["specularShininess"], shininess);
		}
		public void LoadSpecularColor(Color color) {
			if(color == null)
				return;

			base.LoadVector3(uniforms["specularColor"], color.RGBA.Xyz);    //RGB
		}
	}
}
