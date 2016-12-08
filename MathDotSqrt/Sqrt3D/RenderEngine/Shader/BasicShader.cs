using MathDotSqrt.Sqrt3D.Util.IO;
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
	public class BasicShader : ShaderProgram {

		private const string SHADER_PATH = @"basicShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"basicVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"basicFragmentShader.fs";

		public BasicShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

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
			base.AddUniform("cameraPosition");
			base.AddUniform("transposedTransformation");


			base.AddUniform("color");
			base.AddUniform("hasTexture");
			base.AddUniform("hasReflectiveCubeMap");
			base.AddUniform("hasRefractiveCubeMap");
			base.AddUniform("refractionIndex");

			base.AddUniform("textureSampler");
			base.AddUniform("reflectiveCubeMap");
			base.AddUniform("refractiveCubeMap");
		}

		public void ConnectTextures() {
			base.LoadInt(uniforms["textureSampler"], 0);
			base.LoadInt(uniforms["reflectiveCubeMap"], 1);
			base.LoadInt(uniforms["refractiveCubeMap"], 2);
		}

		public void LoadColor(Color color) {
			if(color == null) {
				base.LoadVector4(uniforms["color"], Color.NullColor.RGBA);
				return;
			}
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
	}
}
