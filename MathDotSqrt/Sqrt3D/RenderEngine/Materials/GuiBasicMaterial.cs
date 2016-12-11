using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util.Math;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class GuiBasicMaterial : Material {

		private static ShaderProgram shader = new GuiBasicShader();
		public override MaterialType Type {
			get {
				return MaterialType.GuiBasicMaterial;
			}
		}

		public int Texture {
			get;
			set;
		}
		public Color Color {
			get;
			set;
		}

		public GuiBasicMaterial() : base(shader) {

		}

		public static implicit operator GuiBasicMaterial(Color v)
		{
			throw new NotImplementedException();
		}
	}
}
