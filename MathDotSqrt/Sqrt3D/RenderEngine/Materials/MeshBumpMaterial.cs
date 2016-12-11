using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util.Math;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class MeshBumpMaterial : Material {
		private static BumpShader shader = new BumpShader();


		public MeshBumpMaterial() : base(shader) {

		}

		public override MaterialType Type {
			get {
				return MaterialType.MeshBumpMaterial;
			}
		}

		public int Texture {
			get;
			set;
		}

		public int NormalMap {
			get;
			set;
		}
		public Color EmissiveColor {
			get;
			set;
		}
		public Color SpecularColor {
			get;
			set;
		}
		public int Shininess {
			get;
			set;
		}
	}
}
