using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class ContrastEffect : Effect{
		private static PPContrastShader contrastShader = new PPContrastShader();

		public ContrastEffect(int width, int height) : base(width, height) {

		}

		public void Render(int colorTexture, bool toBuffer =  true) {
			contrastShader.Start();
			contrastShader.BindColorTexture(colorTexture);
			Render(toBuffer);
			contrastShader.Stop();
		}
	}
}
