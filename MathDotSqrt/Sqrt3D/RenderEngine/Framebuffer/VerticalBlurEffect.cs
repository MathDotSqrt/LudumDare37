using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class VerticalBlurEffect : Effect{
		private static PPVerticalBlurShader vShader = new PPVerticalBlurShader();

		public VerticalBlurEffect(int width, int height) : base(width, height) {

		}

		public void Render(int colorTexture, bool toBuffer = true) {
			vShader.Start();
			vShader.BindColorTexture(colorTexture);
			vShader.LoadTargetHeight(fbo.Height);
			Render(toBuffer);
			vShader.Stop();
		}
	}
}
