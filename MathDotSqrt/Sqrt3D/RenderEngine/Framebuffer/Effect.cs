using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using MathDotSqrt.Sqrt3D.Util;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	//TODO remove this class and redo PostProcessing
	public abstract class Effect {
		public FBO fbo {
			get;
			protected set;
		}

		public Effect(int width, int height) {
			fbo = new FBO(width, height);
		}

		protected void Render(bool toBuffer) {
			if(toBuffer)
				fbo.BindFrameBuffer();
			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, PostProcessing.quad.VAO.IndexCount);
			if(toBuffer)
				fbo.UnbindCurrentFramebuffer();
		}

	}
}
