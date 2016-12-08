using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {

	public class QuadGeometry2d : Geometry2d{

		//Quad model for OpenGL.DrawArrays BeginMode.TriangleStrips rendering

		public QuadGeometry2d() {
			positions = new List<float>(){ -1f, 1f, -1f, -1f, 1f, 1f, 1f, -1f };
			this.VAO = VAOLoader.LoadToVAO(positions.ToArray(), 2);
		}
	}
}
