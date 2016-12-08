using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.Math {
	/// <summary>
	/// Makes working with colors easy af (Your welcome)
	/// </summary>
	public class Color {

		public static Color NullColor = new Color(-1, -1, -1, -1);

		public static Color White = new Color(0xFFFFFF);
		public static Color Silver = new Color(0xc0c0c0);


		public static Color AliceBlue = new Color(0xF0F8FF);
		public static Color AntiqueWhite = new Color(0xFAEBD7);
		public static Color Aqua = new Color(0x00FFFF);
		public static Color Aquamarine = new Color(0x7FFFD4);
		public static Color Azure = new Color(0xF0FFFF);
		public static Color Bisque = new Color(0xFFE4C4);
		public static Color Black = new Color(0);
		public static Color BlackCock = new Color(0);
		public static Color BlueViolet = new Color(0x8A2BE2);
		public static Color Brown = new Color(0xA52A2A);
		public static Color BurleyWood = new Color(0xDEB887);
		public static Color CadetBlue = new Color(0x5F9EA0);
		public static Color Chartreuse = new Color(0x7FFF00);
		public static Color Chocolate = new Color(0xD2691E);
		public static Color Coral = new Color(0xFF7F50);
		public static Color CornflowerBlue = new Color(0x6495ED);
		public static Color Crimson = new Color(0xDC143C);
		public static Color Cyan = new Color(0x00FFFF);
		public static Color DarkBlue = new Color(0x00008B);
		public static Color DarkCyan = new Color(0x008B8B);
		public static Color DarkGoldenRod = new Color(0xB8860B);
		public static Color DarkGrey = new Color(0xA9A9A9);
		public static Color DarkGreen = new Color(0x006400);
		public static Color DarkKhaki = new Color(0xBDB76B);
		public static Color DarkMagenta = new Color(0x8B008B);
		public static Color DarkOliveGreen = new Color(0x556B2F);
		public static Color DarkOrange = new Color(0xFF8C00);
		public static Color DarkOrchid = new Color(0x9932CC);
		public static Color DarkRed = new Color(0x8B0000);
		public static Color DarkSalmon = new Color(0xE9967A);
		public static Color DarkSeaGreen = new Color(0x8FBC8F);
		public static Color DarkSlateBlue = new Color(0x2F4F4F);
		public static Color DarkTurquoise = new Color(0x00CED1);
		public static Color DarkViolet = new Color(0x9400D3);
		public static Color DeepPink = new Color(0xFF1493);
		public static Color DeepSkyBlue = new Color(0x00BFFF);
		public static Color DimGrey = new Color(0x696969);  //nice
		public static Color DogerBlue = new Color(0x1E90FF);
		public static Color FireBrick = new Color(0xB22222);
		public static Color FloralWhite = new Color(0xFFFAF0);
		public static Color ForestGreen = new Color(0x228B22);
		public static Color Fuchsia = new Color(0xFF00FF);
		public static Color Gainsboro = new Color(0xDCDCDC);
		public static Color Gold = new Color(0xFFD700);
		public static Color GoldenRod = new Color(0xDAFF50);
		public static Color Grey = new Color(0x808080);
		public static Color GreenYellow = new Color(0xADFF2F);
		public static Color HotPink = new Color(0xFF69B4);//marker

		public static Color Red = new Color(1, 0, 0);
		public static Color Maroon = new Color(0x800000);
		public static Color Yellow = new Color(0xFFFF00);
		public static Color Shrek = new Color(0x808000);
		public static Color Lime = new Color(0x00FF00);
		public static Color Green = new Color(0x008000);
		public static Color Teal = new Color(0x008080);
		public static Color Blue = new Color(0x0000FF);
		public static Color Navy = new Color(0x000080);
		public static Color Purple = new Color(0x800080);


		public Vector4 RGBA;
		public Vector3 RGB {
			get {
				return RGBA.Xyz;
			}
			set {
				RGBA.Xyz = value;
			}
		}

		public float R {
			get {
				return RGBA.X;
			}
			set {
				if(value > 0 && value <= 1) {
					RGBA.X = value;
				}
			}
		}
		public float G {
			get {
				return RGBA.Y;
			}
			set {
				if(value > 0 && value <= 1) {
					RGBA.Y = value;
				}
			}
		}
		public float B {
			get {
				return RGBA.Z;
			}
			set {
				if(value > 0 && value <= 1) {
					RGBA.Z = value;
				}
			}
		}
		public float A {
			get {
				return RGBA.W;
			}
			set {
				if(value > 0 && value <= 1) {
					RGBA.W = value;
				}
			}
		}

		public Color() {
			RGBA = new Vector4(0, 0, 0, 1);
		}

		public Color(float r, float g, float b) {
			RGBA = new Vector4(r, g, b, 1);
		}
		public Color(float r, float g, float b, float a) {
			RGBA = new Vector4(r, g, b, a);
		}
		public Color(int hex, bool alpha = false) {
			if(alpha) {
				R = ( hex & 0xFF000000 ) / 0xFF;
				G = ( hex & 0x00FF0000 ) / 0xFF;
				B = ( hex & 0x0000FF00 ) / 0xFF;
				A = ( hex & 0x000000FF ) / 0xFF;
			}
			else {
				int test = ( hex & 0xFF0000 ) >> 16;
				R = (float)( ( hex & 0xFF0000 ) >> 16 ) / 255f;
				G = (float)( ( hex & 0x00FF00 ) >> 8 ) / 255f;
				B = (float)( ( hex & 0x0000FF ) ) / 255f;
				A = 1;
			}
		}

		public static Color BlendColors(Color color1, Color color2) {
			Color mixedColors = new Color(0, 0, 0);
			mixedColors.R = ( color1.R + color2.R ) / 2;
			mixedColors.G = ( color1.G + color2.G ) / 2;
			mixedColors.B = ( color1.B + color2.B ) / 2;
			mixedColors.A = ( color1.A + color2.A ) / 2;

			return mixedColors;
		}
		public static Color BlendColors(List<Color> colors) {
			if(colors == null || colors.Count == 0)
				return new Color(0, 0, 0);

			float r = 0;
			float g = 0;
			float b = 0;
			float a = 0;

			foreach(Color color in colors) {
				r += color.R;
				g += color.G;
				b += color.B;
				a += color.A;
			}

			r /= colors.Count;
			g /= colors.Count;
			b /= colors.Count;
			a /= colors.Count;

			return new Color(r, g, b, a);
		}

		public override string ToString() {
			string output = String.Format("RGBA: ({0}, {1}, {2}, {3})", R.ToString(), G.ToString(), B.ToString(), A.ToString());
			return output;
		}
	}
}
