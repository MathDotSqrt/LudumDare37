using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class PPVerticalBlurShader : ShaderProgram {
		public const string SHADER_PATH = @"verticalBlurShader\";
		public const string VERTEX_SHADER = SHADER_PATH + "verticalBlurVertexShader.vs";
		public const string FRAGMENT_SHADER = SHADER_PATH + "verticalBlurFragmentShader.fs";

		public PPVerticalBlurShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
		}

		protected override void BindAllUniformLocations() {
			base.AddUniform("targetHeight");
		}

		public void BindColorTexture(int colorTexture) {
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, colorTexture);
		}

		public void LoadTargetHeight(float targetHeight) {
			base.LoadFloat(uniforms["targetHeight"], targetHeight);
		}
	}
}
