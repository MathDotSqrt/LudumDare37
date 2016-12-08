using MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.IO.Loader {
	public static class FontLoader {
		public static Font LoadFont(string fontname) {
			string filePath = FilePaths.FONTS_RELATIVE + fontname + "\\" + fontname;
			string fontFile = filePath + ".fnt";
			string textureAtlas = filePath + ".png";

			if(!System.IO.File.Exists(fontFile)) {
				Output.Error("FontLoader.LoadFont: Could not load Font File\r\n File Name: " + fontFile + " does not exist");
				return null;
			}
			if(!System.IO.File.Exists(textureAtlas)) {
				Output.Error("FontLoader.LoadFont: Could not load Texture Atlas File\r\n File Name: " + textureAtlas + " does not exist");
				return null;
			}

			string[] lines = System.IO.File.ReadAllLines(fontFile);

			Dictionary<int, Character> charData = new Dictionary<int, Character>();

			char[] split = new char[] { ' ' };
			int padding = Parse(lines[0].Split(split)[10].Split('=')[1].Split(',')[0]);
			int texAtlasWidth = GetAttrValue(lines[1].Split(split)[3]);
			int texAtlasHeight = GetAttrValue(lines[1].Split(split)[4]);

			for(int i = 0; i < lines.Length; i++) {
				string line = lines[i];
				string[] attrs = line.Split(split, StringSplitOptions.RemoveEmptyEntries);

				if(attrs[0] == "char") {
					int id = GetAttrValue(attrs[1]);

					Character c = new Character();
					c.X = GetAttrValue(attrs[2]);
					c.Y = GetAttrValue(attrs[3]);
					c.Width = GetAttrValue(attrs[4]) - padding;
					c.Height = GetAttrValue(attrs[5]);
					c.xOffset = GetAttrValue(attrs[6]);
					c.yOffset = GetAttrValue(attrs[7]);
					c.xAdvance = GetAttrValue(attrs[8]);

					charData.Add(id, c);
				}
			}

			return new Font(charData, TextureLoader.LoadFontTextureAtlas(fontname, true), texAtlasWidth, texAtlasHeight);
		}

		public static int GetAttrValue(string attr) {
			return Parse(attr.Split('=')[1]);
		}
		public static int Parse(string integer) {
			return int.Parse(integer);
		}
	}
}
