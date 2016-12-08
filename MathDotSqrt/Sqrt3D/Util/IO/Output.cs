using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.IO {
	public static class Output {

		public static void Good(object text) {
			Console.ForegroundColor = ConsoleColor.Green;
			WriteLine(text);
		}

		public static void Warning(object text) {
			Console.ForegroundColor = ConsoleColor.Yellow;
			WriteLine(text);
		}
		public static void Error(object text) {
			Console.ForegroundColor = ConsoleColor.Red;
			WriteLine(text);
		}
		public static void WriteLine() {
			Console.WriteLine();
			Console.ForegroundColor = ConsoleColor.White;
		}
		public static void WriteLine(object text) {
			Console.WriteLine(text.ToString());
			Console.ForegroundColor = ConsoleColor.White;
		}
		
	}
}
