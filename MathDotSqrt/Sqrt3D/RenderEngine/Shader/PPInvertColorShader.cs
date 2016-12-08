using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class PPInvertColorShader : ShaderProgram {
		private const string SHADER_PATH = @"invertColorShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"invertColorVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"invertColorFragmentShader.fs";

		public PPInvertColorShader() : base(VERTEX_SHADER, FRAGMENT_SHADER){

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
