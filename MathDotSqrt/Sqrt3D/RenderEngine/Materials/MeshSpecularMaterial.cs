 using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class MeshSpecularMaterial : Material {

		private static ShaderProgram litShader = new SpecularShader();
		public override MaterialType Type {
			get { return MaterialType.MeshSpecularMaterial; }
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

		public MeshSpecularMaterial() : base(litShader) {

		}

		public override string ToString() {
			string output = base.ToString();
			output += "EmissiveColor: " + EmissiveColor + "\r\n";
			output += "SpecularColor: " + SpecularColor + "\r\n";
			output += "Shininess: " + Shininess + "\r\n";

			return output;
		}
	}
}
