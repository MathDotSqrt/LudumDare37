using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class ExtractBrightnessEffect : Effect{
		private static PPBrightnessShader brightnessShader = new PPBrightnessShader();

		public ExtractBrightnessEffect(int width, int height) : base(width, height) {

		}

		public void Render(int colorTexture, bool toBuffer = true) {
			brightnessShader.Start();
			brightnessShader.BindColorTexture(colorTexture);
			Render(toBuffer);
			brightnessShader.Stop();
		}
	}
}
