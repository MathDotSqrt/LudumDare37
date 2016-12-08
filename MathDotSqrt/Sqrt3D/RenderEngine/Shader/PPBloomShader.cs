using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class PPBloomShader : ShaderProgram{
		private const string SHADER_PATH = @"bloomShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"bloomVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"bloomFragmentShader.fs";

		public PPBloomShader() : base(VERTEX_SHADER, FRAGMENT_SHADER){

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
		}

		protected override void BindAllUniformLocations() {
			base.AddUniform("textureSampler");
			base.AddUniform("blurTextureSampler");
		}

		public void BindTextures(int colorTexture, int blurTexture) {
			base.LoadInt(uniforms["textureSampler"], 0);
			base.LoadInt(uniforms["blurTextureSampler"], 1);

			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, colorTexture);

			GL.ActiveTexture(TextureUnit.Texture1);
			GL.BindTexture(TextureTarget.Texture2D, blurTexture);
		}
	}
}
