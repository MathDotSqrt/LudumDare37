using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.IO {
	public static class FilePaths {
		public static string RES_RELATIVE = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Substring(6) + @"\..\..\res\";

		public static string AUDIO_RELATIVE = RES_RELATIVE + @"audio\";
		public static string GEOMETRY_RELATIVE = RES_RELATIVE + @"models\";
		public static string SHADER_RELATIVE = RES_RELATIVE + @"shaders\";
		public static string TEXTURE_RELATIVE = RES_RELATIVE + @"maps\";
		public static string MODEL_TEXTURES = TEXTURE_RELATIVE + @"texture_maps\";
		public static string NORMAL_MAPS = TEXTURE_RELATIVE + @"normal_maps\";
		public static string SPECULAR_MAPS = TEXTURE_RELATIVE + @"specular_maps\";
		public static string CUBE_MAPS = TEXTURE_RELATIVE + @"cube_maps\";
		public static string FONTS_RELATIVE = RES_RELATIVE + @"fonts\";

	}
}
