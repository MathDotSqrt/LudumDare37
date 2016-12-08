using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class LitShader : ShaderProgram {
		private const string SHADER_PATH = @"litShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"litVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"litFragmentShader.fs";

		public LitShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
			base.BindVertexArrayAttr(VAOAttribLocation.Texture_UV, "textureUV");
			base.BindVertexArrayAttr(VAOAttribLocation.Normal, "normal");
		}
		protected override void BindAllUniformLocations() {
			base.AddUniform("transformationMatrix");
			base.AddUniform("viewMatrix");
			base.AddUniform("projectionMatrix");
			base.AddUniform("transposedTransformation");
			base.AddUniform("cameraPosition");

			base.AddUniform("color");
			base.AddUniform("hasTexture");
			base.AddUniform("hasReflectiveCubeMap");
			base.AddUniform("hasRefractiveCubeMap");
			base.AddUniform("refractionIndex");

			base.AddUniform("textureSampler");
			base.AddUniform("reflectiveCubeMap");
			base.AddUniform("refractiveCubeMap");

			base.AddUniform("ambientLightColor");
			base.AddUniform("ambientLightIntensity");

			base.AddUniform("pointLightCount");
			base.AddUniform("pointLightPositions");
			base.AddUniform("pointLightColors");
			base.AddUniform("pointLightIntensity");
		}

		public void ConnectTextures() {
			base.LoadInt(uniforms["textureSampler"], 0);
			base.LoadInt(uniforms["reflectiveCubeMap"], 1);
			base.LoadInt(uniforms["refractiveCubeMap"], 2);
		}

		public void LoadTransformationMatrix(Matrix4 m) {
			base.LoadMatrix4(uniforms["transformationMatrix"], m);

			Matrix4 inverse = Matrix4.Invert(m);
			Matrix4 transpose = Matrix4.Transpose(inverse);
			Matrix3 mat3 = new Matrix3(transpose);
			base.LoadMatrix3(uniforms["transposedTransformation"], mat3);
		}
		public void LoadCamera(Camera camera) {
			base.LoadMatrix4(uniforms["viewMatrix"], camera.InverseTransformationMatrix);
			base.LoadMatrix4(uniforms["projectionMatrix"], camera.ProjectionMatrix);
			base.LoadVector3(uniforms["cameraPosition"], camera.WorldSpacePostion);
		}

		public void LoadColor(Color color) {
			if(color == null)
				return;
			base.LoadVector4(uniforms["color"], color.RGBA);
		}
		public void LoadTexture(int texture) {
			if(texture <= 0) {
				base.LoadBool(uniforms["hasTexture"], false);
				return;
			}

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, texture);
			base.LoadBool(uniforms["hasTexture"], true);
		}
		public void LoadReflectiveCubeMap(int cubemap) {
			if(cubemap <= 0) {
				base.LoadBool(uniforms["hasReflectiveCubeMap"], false);
				return;
			}

			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.TextureCubeMap, cubemap);
			base.LoadBool(uniforms["hasReflectiveCubeMap"], true);
		}
		public void LoadRefractiveCubeMap(int cubemap, float index) {
			if(cubemap <= 0) {
				base.LoadBool(uniforms["hasRefractiveCubeMap"], false);
				return;
			}

			GL.ActiveTexture(TextureUnit.Texture2);
			GL.BindTexture(TextureTarget.TextureCubeMap, cubemap);
			base.LoadBool(uniforms["hasRefractiveCubeMap"], true);
			base.LoadFloat(uniforms["refractionIndex"], index);
		}
		public void LoadPointLights(List<Light> lights) {
			if(lights == null || lights.Count == 0)
				return;

			float[] positionArray = new float[lights.Count * 3];
			float[] colorArray = new float[lights.Count * 3];
			float[] intensity = new float[lights.Count];

			for(int i = 0; i < lights.Count; i++) {
				Vector3 position = ( lights[i].Parent == null ? lights[i].Position : lights[i].WorldSpacePostion );

				positionArray[i * 3 + 0] = position.X;
				positionArray[i * 3 + 1] = position.Y;
				positionArray[i * 3 + 2] = position.Z;

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
	}
}
