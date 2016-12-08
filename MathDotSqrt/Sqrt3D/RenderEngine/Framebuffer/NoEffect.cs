using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class NoEffect : Effect {
		private static PPNoShader shader = new PPNoShader();

		public NoEffect(int width, int height) :base(width, height){

		}

		public void Render(int colorTexture, bool toBuffer = true) {
			shader.Start();
			shader.BindColorTexture(colorTexture);
			Render(toBuffer);
			shader.Stop();
		}
	}
}
