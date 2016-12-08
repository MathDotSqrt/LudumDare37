using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World.Objects {
	
	public class Mesh : Object3D {
		
		public Geometry Geometry {
			get;
			private set;
		}
		public Material Material {
			get;
			private set;
		}

		public Mesh(Geometry geometry, Material material) {
			this.Geometry = geometry;
			this.Material = material;
		}

		public override string ToString() {
			string output = base.ToString();
			output += "-----Mesh-----\r\n";
			output += "Geometry set: " + ( Geometry != null ).ToString() + "\r\n";
			output += "Material set: " + ( Material != null ).ToString() + "\r\n";

			return output;
		}
		public override void Dispose() {
			Geometry.Dispose();
			Material.Dispose();
		}
	}
}
