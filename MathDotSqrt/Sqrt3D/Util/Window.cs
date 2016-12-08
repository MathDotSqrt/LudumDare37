using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util {
	/// <summary>
	/// This is a helper class that stores all of the useful data about the GameWindow in a single simple static class.
	/// We do this to make accessing possible from everywhere in the code.
	///
	/// I know this is concidered bad code design to have static data
	/// anyone can fuck with floating around in the code but I trust yall niggas wont fuck with this.
	/// </summary>
	public class Window {

		private static int width;
		private static int height;
		private static float aspectRatio;

		public static int WIDTH {
			get { return width; }
		}
		public static int HEIGHT {
			get { return height; }
		}
		public static float ASPECT_RATIO {
			get { return aspectRatio; }
		}

		public Window(int width, int height) {
			Window.width = width;
			Window.height = height;
			aspectRatio = (float)width / (float)height;
		}


		public void OnWindowResize(int width, int height) {
			//adjusting all the values to fit the newly resized window
			Window.width = width;
			Window.height = height;
			aspectRatio = (float)width / (float)height;
		}

	}
}
