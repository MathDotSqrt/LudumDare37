using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class RadialBlurEffect : Effect {
		private static PPRadialBlurShader shader = new PPRadialBlurShader();

		public RadialBlurEffect(int width, int height) :base(width, height){

		}

		public void Render(int colorTexture, Object3D obj, Camera camera, bool toBuffer = true) {
			shader.Start();
			shader.BindColorTexture(colorTexture);
			shader.BindLightPositionOnScreen(obj, camera);
			Render(toBuffer);
			shader.Stop();
		}
	}
}
