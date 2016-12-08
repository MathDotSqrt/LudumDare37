using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using MathDotSqrt.Sqrt3D.Util;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer {
	public class FBO {

		public int Width {
			get;
			private set;
		}
		public int Height {
			get;
			private set;
		}
		public int Framebuffer {
			get;
			private set;
		}
		public int ColorTexture {
			get;
			private set;
		}
		public int Colorbuffer {
			get;
			private set;
		}
		public int DepthTexture {
			get;
			private set;
		}
		public int Depthbuffer {
			get;
			private set;
		}

		public bool MultiSample {
			get;
			private set;
		}

		public FBO(int width, int height, bool multiSample = false) {
			Width = width;
			Height = height;
			MultiSample = multiSample;

			CreateFramebuffer();
			CreateColorTextureAttachment();
			CreateDepthTextureAttachment();
			if(multiSample)
				CreateMultiSampleColorbufferAttatchment();
			CreateDepthbufferAttachment();
			UnbindCurrentFramebuffer();
		}

		private void CreateFramebuffer() {
			Framebuffer = GL.GenFramebuffer();
			GL.BindFramebuffer(FramebufferTarget.Framebuffer, Framebuffer);
			//which color buffer attachment to render to
			GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
		}
		public void BindFrameBuffer() {
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, Framebuffer);
			GL.Viewport(0, 0, Width, Height);
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
		}
		public void UnbindCurrentFramebuffer() {
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
			GL.Viewport(0, 0, Window.WIDTH, Window.HEIGHT); //Reset to window size
		}
		public void ReadFrameBuffer() {
			GL.BindTexture(TextureTarget.Texture2D, 0);
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, Framebuffer);
			GL.ReadBuffer(ReadBufferMode.ColorAttachment0);
		}

		public void ResolveToFBO(FBO outputFBO) {
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, outputFBO.Framebuffer);
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, this.Framebuffer);
			GL.BlitFramebuffer(0, 0, Width, Height, 0, 0, outputFBO.Width, outputFBO.Height, 
				ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit, BlitFramebufferFilter.Nearest);

			this.UnbindCurrentFramebuffer();
		}
		public void ResolveToScreen(FBO outputFBO) {
			GL.BindFramebuffer(FramebufferTarget.DrawFramebuffer, 0);
			GL.BindFramebuffer(FramebufferTarget.ReadFramebuffer, this.Framebuffer);
			GL.DrawBuffer(DrawBufferMode.Back);
			GL.BlitFramebuffer(0, 0, Width, Height, 0, 0, Window.WIDTH, Window.HEIGHT,
				ClearBufferMask.ColorBufferBit, BlitFramebufferFilter.Nearest);

			this.UnbindCurrentFramebuffer();
		}

		private void CreateColorTextureAttachment() {
			ColorTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, ColorTexture);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb,
				Width, Height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, (IntPtr)null);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, ColorTexture, 0);

		}
		private void CreateDepthTextureAttachment() {
			DepthTexture = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, DepthTexture);
			GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent24,
				Width, Height, 0, PixelFormat.DepthComponent, PixelType.Float, (IntPtr)null);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, DepthTexture, 0);
		}

		private void CreateMultiSampleColorbufferAttatchment() {
			Colorbuffer = GL.GenRenderbuffer();
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Colorbuffer);
			GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.Rgba8, Width, Height);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, RenderbufferTarget.Renderbuffer, Colorbuffer);
		}

		private void CreateDepthbufferAttachment() {
			Depthbuffer = GL.GenRenderbuffer();
			GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, Depthbuffer);
			if(!MultiSample) 
				GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent24, Width, Height);
			else
				GL.RenderbufferStorageMultisample(RenderbufferTarget.Renderbuffer, 4, RenderbufferStorage.DepthComponent24, Width, Height);
			GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, Depthbuffer);
		}
	}
}
