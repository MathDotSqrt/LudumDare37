using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class InvertColorEffect : Effect{

		private static PPInvertColorShader shader = new PPInvertColorShader();

		public InvertColorEffect(int width, int height) : base(width, height) {

		}

		public void Render(int colorTexture, bool toBuffer = true) {
			shader.Start();
			shader.BindColorTexture(colorTexture);
			Render(toBuffer);
			shader.Stop();
		}
	}
}
