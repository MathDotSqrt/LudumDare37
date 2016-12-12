using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer;
using OpenTK.Graphics.OpenGL;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public static class PostProcessing{

		public static Geometry2d quad;

		private static NoEffect noEffect;
		private static InvertColorEffect invert;
		private static ContrastEffect contrast;
		private static ExtractBrightnessEffect brightnessFilter;
		private static HorizontalBlurEffect hBlur;
		private static VerticalBlurEffect vBlur;
		private static BloomEffect bloom;
		private static RadialBlurEffect radBlur;

		public static void InitPostProcessing() {
			quad = new QuadGeometry2d();

			int width = Window.WIDTH;
			int height = Window.HEIGHT;

			noEffect = new NoEffect(width, height);
			invert = new InvertColorEffect(width, height);
			contrast = new ContrastEffect(width, height);
			brightnessFilter = new ExtractBrightnessEffect(width, height);
			hBlur = new HorizontalBlurEffect(width / 16, height / 16);
			vBlur = new VerticalBlurEffect(width / 16, height / 16);
			bloom = new BloomEffect(width, height);
			radBlur = new RadialBlurEffect(width, height);
		}

		public static void RenderPostProcessingPipeLine(FBO fbo, Scene scene) {
			StartPostProcessing();

			//brightnessFilter.Render(fbo.ColorTexture);
			//radBlur.Render(brightnessFilter.fbo.ColorTexture, scene.GetSortedLights()[LightType.RadialLight][0], scene.Camera);
			//vBlur.Render(brightnessFilter.fbo.ColorTexture);
			//hBlur.Render(vBlur.fbo.ColorTexture);
			//bloom.Render(hBlur.fbo.ColorTexture, fbo.ColorTexture, false);
			noEffect.Render(fbo.ColorTexture, false);


			StopPostProcessing();
		}

		private static void StartPostProcessing() {
			GL.BindVertexArray(quad.VAO.Id);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.Disable(EnableCap.DepthTest);
		}

		private static void StopPostProcessing() {
			GL.Enable(EnableCap.DepthTest);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.BindVertexArray(0);
		}
	}
}
