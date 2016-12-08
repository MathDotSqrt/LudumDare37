using MathDotSqrt.Sqrt3D.World.Objects;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class SkyboxShader : ShaderProgram{
		private const string SHADER_PATH = @"skyboxShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"skyboxVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"skyboxFragmentShader.fs";

		public SkyboxShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
		}
		protected override void BindAllUniformLocations() {
			base.AddUniform("transformationMatrix");
			base.AddUniform("viewMatrix");
			base.AddUniform("projectionMatrix");
			base.AddUniform("hasTexture");
		}

		public void LoadSkyboxCubeMap(int cubemap) {
			if(cubemap <= 0) {
				base.LoadBool(uniforms["hasTexture"], false);
				return;
			}

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.TextureCubeMap, cubemap);
			base.LoadBool(uniforms["hasTexture"], true);
		}
		public void LoadTransformationMatrix(Matrix4 m) {
			base.LoadMatrix4(uniforms["transformationMatrix"], m);
		}
		public void LoadCamera(Camera camera) {
			Matrix4 viewMatrix = camera.InverseTransformationMatrix.ClearTranslation();
			base.LoadMatrix4(uniforms["viewMatrix"], viewMatrix);
			base.LoadMatrix4(uniforms["projectionMatrix"], camera.ProjectionMatrix);
		}
		
	}

}
