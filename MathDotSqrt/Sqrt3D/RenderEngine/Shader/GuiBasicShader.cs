using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class GuiBasicShader : ShaderProgram {

		public const string SHADER_PATH = @"guiBasicShader\";
		public const string VERTEX_SHADER = SHADER_PATH + "guiBasicVertexShader.vs";
		public const string FRAGMENT_SHADER = SHADER_PATH + "guiBasicFragmentShader.fs";

		public GuiBasicShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
		}

		protected override void BindAllUniformLocations() {
			base.AddUniform("transformationMatrix");
			base.AddUniform("textureSampler");
			base.AddUniform("hasTexture");
			base.AddUniform("color");
		}

		public void ConnectTextures() {
			base.LoadInt(uniforms["textureSampler"], 0);
		}

		public void LoadTransformation(Matrix4 m) {
			base.LoadMatrix4(uniforms["transformationMatrix"], m);
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
		public void LoadColor(Color color) {
			if(color == null)
				color = Color.NullColor;

			base.LoadVector4(uniforms["color"], color.RGBA);
		}
	}
}
