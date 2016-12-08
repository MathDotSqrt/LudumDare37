using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	
	public abstract class Material {
		
		
		public abstract MaterialType Type {
			get;
		}
		public ShaderProgram Shader {
			get;
			private set;
		}
		public RenderMode RenderMode {
			get;
			set;
		}
		public RenderFace RenderFace {
			get;
			set;
		}

		public Material(ShaderProgram shader) {
			Shader = shader;
		}

		public override string ToString() {
			return "todo do material output";
		}
		public void Dispose() {
			Shader.Dispose();
		}
	}
}
