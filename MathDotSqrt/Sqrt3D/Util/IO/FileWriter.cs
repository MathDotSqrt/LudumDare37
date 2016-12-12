using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.IO {
	public static class FileWriter {
		public static void Clear() {
			System.IO.File.WriteAllText(FilePaths.RES_RELATIVE + "node.txt", string.Empty);
		}
		public static void WriteFile(string text) {
			using(System.IO.StreamWriter file =
			new System.IO.StreamWriter(FilePaths.RES_RELATIVE + "node.txt", true)) {
				file.WriteLine(text);
			}
		}

		public static void DeleteLastLine() {
			var lines = System.IO.File.ReadAllLines(FilePaths.RES_RELATIVE + "node.txt");
			System.IO.File.WriteAllLines(FilePaths.RES_RELATIVE + "node.txt", lines.Take(lines.Length - 1).ToArray());
		}
	}
}
