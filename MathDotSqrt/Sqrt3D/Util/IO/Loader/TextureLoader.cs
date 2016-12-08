using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace MathDotSqrt.Sqrt3D.Util.IO.Loader {

	/// <summary>
	/// Statically loads textures.
	/// </summary>
	public static class TextureLoader {

		private static List<int> textureID = new List<int>();

		private static string[] cubeTexturePaths = { @"\pos_x.png", @"\neg_x.png", @"\pos_y.png", @"\neg_y.png", @"\pos_z.png", @"\neg_z.png" };
		
		public static int Load2DTexture(string filePath, bool linearInterpolation = false) {
			if(!System.IO.File.Exists(filePath)) {
				Output.Error("TextureLoader.Load2DTexture: Could not load texture\r\n File path: " + filePath + " does not exist");
				return -1;
			}

			int texID = GL.GenTexture();
			GL.BindTexture(TextureTarget.Texture2D, texID);

			Bitmap bmp = null;
			try {
				bmp = new Bitmap(filePath);
			}
			catch(Exception e) {
				Output.Error("TextureLoader.Load2DTexture: Fatal error has occured in loading your texture bud\r\nProbably the image format is not supported");
				Output.Error(e.Message);
				return -1;
			}
			BitmapData bmpData = bmp.LockBits(
				new Rectangle(0, 0, bmp.Width, bmp.Height),
				ImageLockMode.ReadOnly,
				System.Drawing.Imaging.PixelFormat.Format32bppArgb
			);

			GL.TexImage2D(
				TextureTarget.Texture2D,
				0,
				PixelInternalFormat.Rgba,	//TODO figure out Gamma correction
				bmpData.Width,
				bmpData.Height,
				0,
				OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
				PixelType.UnsignedByte,
				bmpData.Scan0
			);
			bmp.UnlockBits(bmpData);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
			GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

			if(linearInterpolation) {
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			}
			else {
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
				GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			}

			textureID.Add(texID);

			return texID;
		}

		public static int LoadFontTextureAtlas(string fontname, bool linearInterpolation = true) {
			return Load2DTexture(FilePaths.FONTS_RELATIVE + fontname + "\\" + fontname + ".png" , linearInterpolation);
		}
		public static int LoadModelTexture(string filePath, bool linearInterpolation = false) {
			return Load2DTexture(FilePaths.MODEL_TEXTURES + filePath, linearInterpolation);
		}
		public static int LoadNormalTexture(string filePath, bool linearInterpolation = false) {
			return Load2DTexture(FilePaths.TEXTURE_RELATIVE + FilePaths.NORMAL_MAPS + filePath, linearInterpolation);
		}
		public static int LoadSpecularTexture(string filePath, bool linearInterpolation = false) {
			return Load2DTexture(FilePaths.TEXTURE_RELATIVE + FilePaths.SPECULAR_MAPS + filePath, linearInterpolation);
		}

		public static int LoadCubeMap(string folderPath, bool linearInterpolation = false) {

			int texID = GL.GenTexture();
			GL.BindTexture(TextureTarget.TextureCubeMap, texID);

			for(int i = 0; i < 6; i++) {
				string filePath = FilePaths.CUBE_MAPS + folderPath + cubeTexturePaths[i];

				if(!System.IO.File.Exists(filePath)) {
					Output.Error("[CubeMap] File path: " + filePath + " does not exist");
					return -1;
				}

				Bitmap bmp = new Bitmap(filePath);
				BitmapData bmpData = bmp.LockBits(
					new Rectangle(0, 0, bmp.Width, bmp.Height),
					ImageLockMode.ReadOnly,
					System.Drawing.Imaging.PixelFormat.Format32bppArgb
				);

				GL.TexImage2D(
					TextureTarget.TextureCubeMapPositiveX + i,
					0,
					PixelInternalFormat.Rgb,    //TODO figure out Gamma correction
					bmpData.Width,
					bmpData.Height,
					0,
					OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
					PixelType.UnsignedByte,
					bmpData.Scan0
				);
				bmp.UnlockBits(bmpData);

				GL.TexParameter(TextureTarget.TextureCubeMapPositiveX + i, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Clamp);
				GL.TexParameter(TextureTarget.TextureCubeMapPositiveX + i, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Clamp);

				
			}

			if(linearInterpolation) {
				GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
				GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
			}
			else {
				GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
				GL.TexParameter(TextureTarget.TextureCubeMap, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
			}

			return texID;
		}

		public static void Dispose() {
			foreach(int texID in textureID) {
				GL.DeleteTexture(texID);
			}
		}
	}
}
