using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class MeshNormalMaterial : Material {

		private static ShaderProgram shader = new NormalShader();

		public override MaterialType Type {
			get { return MaterialType.MeshNormalMaterial; }
		}

		public MeshNormalMaterial() : base(shader) {

		}
	}
}
