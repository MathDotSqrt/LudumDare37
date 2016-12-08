using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine{
	public enum VAOAttribLocation {
		Position = 0,
		Texture_UV = 1,
		Normal = 2,
	}

	public class VAO {
		public int Id {
			get;
			private set;
		}
		public int IndexCount {
			get;
			private set;
		}

		public VAO(int vao_ID, int vertexCount) {
			Id = vao_ID;
			IndexCount = vertexCount;
		}
	}
}
