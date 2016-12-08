using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	public class MeshSkyboxMaterial : Material {

		private static ShaderProgram shader = new SkyboxShader();

		public override MaterialType Type {
			get {
				return MaterialType.MeshSkyboxMaterial;
			}
		}
		public int SkyboxCubeMap {
			get;
			set;
		}

		public MeshSkyboxMaterial() : base(shader) {
			RenderFace = RenderFace.Back;
		}

		
	}
}
