using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts {
	public static class TextElementGenerator {
		public static GuiElement GenTextElement(Font font, string text, float fontSize, float lineWidth, float lineHeight, float spaceWidth = 20) {
			Geometry2d geometry = new FontGeometry(font, text, fontSize, lineWidth, lineHeight, spaceWidth);
			Material material = new GuiFontMaterial(font.Texture);
			return new GuiElement(geometry, material);
		}
	}
}
