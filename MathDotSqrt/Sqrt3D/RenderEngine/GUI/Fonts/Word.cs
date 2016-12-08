using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts {
	public class Word {
		public Character this[int index] {
			get {
				if(index < 0)
					return null;
				if(index >= Characters.Count)
					return null;

				return Characters[index];
			}
		}

		public List<Character> Characters {
			get;
			private set;
		}
		public List<int> CharacterX {
			get;
			private set;
		}
		public float WordWidth {
			get;
			private set;
		}
		

		public Word(Font font, string word) {
			Characters = new List<Character>();

			foreach(char c in word) {
				Characters.Add(font.charData[c]);
			}

			CalcWordWidth();
			CalcCharacterPositions();
		}
		public Word(List<Character> characters) {
			this.Characters = characters;

			CalcWordWidth();
			CalcCharacterPositions();
		}

		private void CalcWordWidth() {
			int width = 0;
			foreach(Character c in Characters) {
				width += c.Width;
			}

			WordWidth = width;
		}
		private void CalcCharacterPositions() {
			CharacterX = new List<int>();
			int currentX = 0;

			foreach(Character c in Characters) {
				CharacterX.Add(currentX);

				currentX += c.Width;
			}
		}
	}
}
