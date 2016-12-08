using MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {
	public class FontGeometry : Geometry2d{
		
		private List<Word> wordList;

		private int texWidth;
		private int texHeight;

		private string text;
		private double fontSize;
		private double lineWidth;

		private int appendedChars = 0;

		private Vector2d cursor = new Vector2d();

		public FontGeometry(Font font, string text, double fontSize, double lineWidth, double lineHeight, double spaceWidth = 20) {
			positions = new List<float>();
			textureUVs = new List<float>();
			indicies = new List<int>();
			wordList = new List<Word>();

			this.texWidth = font.Width;
			this.texHeight = font.Height;
			this.text = text;
			this.fontSize = fontSize;
			this.lineWidth = lineWidth;

			string[] words = text.Split(' ');
			foreach(string word in words) {
				wordList.Add(new Word(font, word));
			}

			foreach(Word word in wordList) {
				double screenWordWidth = word.WordWidth / Window.WIDTH * fontSize;

				if(screenWordWidth + cursor.X > lineWidth / Window.WIDTH * 2) {
					cursor.X = 0;
					cursor.Y -= lineHeight / Window.HEIGHT;
				}

				for(int i = 0; i < word.Characters.Count; i++) {
					BuildChar(word[i], (double)word.CharacterX[i] / Window.WIDTH * this.fontSize + cursor.X, cursor.Y);
				}

				cursor.X += screenWordWidth + spaceWidth / Window.WIDTH;
			}

			CenterTextElement();
			this.VAO = VAOLoader.LoadToVAO(positions.ToArray(), textureUVs.ToArray(), indicies.ToArray(), 2);
		}

		public void AddChar(char c) {

		}
		public void Delete() {

		}

		//0--2
		//|  |
		//1--3
		private void BuildChar(Character c, double xOffset, double yOffset) {
			double width = (c.Width * fontSize) / Window.WIDTH;
			double height = ( c.Height * fontSize ) / Window.HEIGHT;

			xOffset += (double)c.xOffset / Window.WIDTH * fontSize / 2;
			yOffset -= (double)c.yOffset / Window.HEIGHT * fontSize;

			//0
			positions.Add((float)xOffset);
			positions.Add((float)yOffset);

			//1
			positions.Add((float)xOffset);
			positions.Add((float)(yOffset - height));

			//2
			positions.Add((float)(xOffset + width));
			positions.Add((float)yOffset);

			//3
			positions.Add((float)(xOffset + width));
			positions.Add((float)(yOffset - height));

			float texX = (float)c.X / texWidth;
			float texY = (float)c.Y / texHeight;
			float charWidth = (float)c.Width / texWidth;
			float charHeight = (float)c.Height / texHeight;

			textureUVs.Add(texX);
			textureUVs.Add(texY);
			textureUVs.Add(texX);
			textureUVs.Add(texY + charHeight);
			textureUVs.Add(texX + charWidth);
			textureUVs.Add(texY);
			textureUVs.Add(texX + charWidth);
			textureUVs.Add(texY + charHeight);

			//Builds indicies of the quads for each additional character
			indicies.Add(appendedChars + 0);
			indicies.Add(appendedChars + 1);
			indicies.Add(appendedChars + 2);
			indicies.Add(appendedChars + 2);
			indicies.Add(appendedChars + 1);
			indicies.Add(appendedChars + 3);

			appendedChars += 4;
		}

		private void CenterTextElement() {
			float elementWidth = BoundingWidth;
			float elementHeight = BoundingHeight;

			for(int i = 0; i < positions.Count / 2; i++) {
				positions[i * 2] -= elementWidth / 2;
				positions[i * 2 + 1] += elementHeight / 2;
			}
		}
	}
}
