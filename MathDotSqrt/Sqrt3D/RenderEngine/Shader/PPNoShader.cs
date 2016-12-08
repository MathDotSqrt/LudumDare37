using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class PPNoShader : ShaderProgram{
		private const string SHADER_PATH = @"noShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"noVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"noFragmentShader.fs";

		public PPNoShader() : base(VERTEX_SHADER, FRAGMENT_SHADER){
			
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
