using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts {
	public class Font {
		
		public Dictionary<int, Character> charData {
			get;
			private set;
		}

		public int Texture {
			get;
			private set;
		}

		public int Width {
			get;
			private set;
		}

		public int Height {
			get;
			private set;
		}

		public Font(Dictionary<int, Character> charData, int texture, int width, int height) {
			this.charData = charData;
			this.Texture = texture;
			this.Width = width;
			this.Height = height;
		}
	}
}
