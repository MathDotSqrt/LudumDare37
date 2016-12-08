using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class PPRadialBlurShader : ShaderProgram {
		private const string SHADER_PATH = @"radialBlurShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"radialBlurVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"radialBlurFragmentShader.fs";

		public PPRadialBlurShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
		}

		protected override void BindAllUniformLocations() {

		}

		public void BindColorTexture(int colorTexture) {
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, colorTexture);
		}
	}
}
