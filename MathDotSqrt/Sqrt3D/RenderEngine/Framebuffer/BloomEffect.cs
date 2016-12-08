using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class BloomEffect : Effect{
		private static PPBloomShader bloomShader = new PPBloomShader();

		public BloomEffect(int width, int height) :base(width, height){

		}

		public void Render(int colorTexture, int blurTexture, bool toBuffer = true) {
			bloomShader.Start();
			bloomShader.BindTextures(colorTexture, blurTexture);
			Render(toBuffer);
			bloomShader.Stop();
		}
	}
}
