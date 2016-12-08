using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class GuiFontShader : ShaderProgram {

		public const string SHADER_PATH = @"guiFontShader\";
		public const string VERTEX_SHADER = SHADER_PATH + "guiFontVertexShader.vs";
		public const string FRAGMENT_SHADER = SHADER_PATH + "guiFontFragmentShader.fs";

		public GuiFontShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			this.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
			this.BindVertexArrayAttr(VAOAttribLocation.Texture_UV, "textureUV");
		}
		protected override void BindAllUniformLocations() {
			this.AddUniform("transformationMatrix");
			this.AddUniform("textureAtlas");
			this.AddUniform("color");
		}

		public void ConnectTextures() {
			base.LoadInt(uniforms["textureAtlas"], 0);
		}

		public void LoadTransformation(Matrix4 m) {
			this.LoadMatrix4(uniforms["transformationMatrix"], m);
		}
		public void LoadTexture(int texture) {
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, texture);
		}
		public void LoadColor(Color color) {
			if(color == null)
				color = Color.White;

			this.LoadVector3(uniforms["color"], color.RGB);
		}

	}
}
