using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {
	public class Face {

		public Vector3[] Verticies;
		public Vector2[] TextureCoords;
		public Vector3[] Normals;

		public Face() {
			Verticies = new Vector3[3];
			TextureCoords = new Vector2[3];
			Normals = new Vector3[3];
		}
	}
}
