using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class MeshBasicMaterial : Material {

		private static ShaderProgram shader = new BasicShader();
		public override MaterialType Type {
			get { return MaterialType.MeshBasicMaterial; }
		}
		
		public Color Color {
			get;
			set;
		}
		public int Texture {
			get;
			set;
		}
		public int CubeMapReflection {
			get;
			set;
		}
		public int CubeMapRefraction {
			get;
			set;
		}
		public float RefractionIndex {
			get;
			set;
		}

		public MeshBasicMaterial() : base(shader) {

			RefractionIndex = 1;
		}

	}
}
