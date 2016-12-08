using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class HorizontalBlurEffect : Effect{
		private static PPHorizontalBlurShader hShader = new PPHorizontalBlurShader();

		public HorizontalBlurEffect(int width, int height) : base(width, height) {

		}

		public void Render(int colorTexture, bool toBuffer = true) {
			hShader.Start();
			hShader.BindColorTexture(colorTexture);
			hShader.LoadTargetWidth(fbo.Width);
			Render(toBuffer);
			hShader.Stop();
		}
	}
}
