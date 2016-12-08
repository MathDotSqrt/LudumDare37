using MathDotSqrt.Sqrt3D.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {
	public class VAOGeometry : Geometry{
		public VAOGeometry(VAO vao) {
			base.VAO = vao;
		}
	}
}
