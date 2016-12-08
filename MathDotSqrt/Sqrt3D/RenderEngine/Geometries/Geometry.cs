using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {
	public abstract class Geometry {

		public VAO VAO {
			get;
			protected set;
		}
		public override string ToString() {
			string output = "-----Geometry-----\r\n";
			output += "VAO ID: " + VAO.Id + "\r\n";
			output += "IndexCount: " + VAO.IndexCount + "\r\n";

			return output;
		}
		public void Dispose() {
		}
	}
}
