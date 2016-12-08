using MathDotSqrt.Sqrt3D.World.Objects;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class NormalShader : ShaderProgram {

		private const string SHADER_PATH = @"normalShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"normalVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"normalFragmentShader.fs";

		public NormalShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
			base.BindVertexArrayAttr(VAOAttribLocation.Normal, "normal");
		}
		protected override void BindAllUniformLocations() {
			base.AddUniform("transformationMatrix");
			base.AddUniform("viewMatrix");
			base.AddUniform("projectionMatrix");
		}

		public void LoadTransformationMatrix(Matrix4 m) {
			base.LoadMatrix4(uniforms["transformationMatrix"], m);
		}
		public void LoadCamera(Camera camera) {
			base.LoadMatrix4(uniforms["projectionMatrix"], camera.ProjectionMatrix);
			base.LoadMatrix4(uniforms["viewMatrix"], camera.InverseTransformationMatrix);
		}
	}
}
