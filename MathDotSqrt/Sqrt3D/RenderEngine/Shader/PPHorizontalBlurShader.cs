using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class PPHorizontalBlurShader : ShaderProgram {
		public const string SHADER_PATH = @"horizontalBlurShader\";
		public const string VERTEX_SHADER = SHADER_PATH + "horizontalBlurVertexShader.vs";
		public const string FRAGMENT_SHADER = SHADER_PATH + "horizontalBlurFragmentShader.fs";

		public PPHorizontalBlurShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Normal, "position");
		}

		protected override void BindAllUniformLocations() {
			base.AddUniform("targetWidth");
		}

		public void BindColorTexture(int colorTexture) {
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, colorTexture);
		}
		public void LoadTargetWidth(float targetWidth) {
			base.LoadFloat(uniforms["targetWidth"], targetWidth);
		}
	}
}
