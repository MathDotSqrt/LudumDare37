using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class MeshLitMaterial : Material {

		private static ShaderProgram litShader = new LitShader();
		public override MaterialType Type {
			get { return MaterialType.MeshLitMaterial; }
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

		public MeshLitMaterial() : base(litShader) {

		}



		public override string ToString() {
			string output = base.ToString();

			return output;
		}
	}
}
