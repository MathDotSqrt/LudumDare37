using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util.Math;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class GuiFontMaterial : Material {
		private static ShaderProgram shader = new GuiFontShader();

		public override MaterialType Type {
			get {
				return MaterialType.GuiFontMaterial;
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

		public GuiFontMaterial(int texture) : base(shader) {
			this.Texture = texture;
		}
		
	}
}
