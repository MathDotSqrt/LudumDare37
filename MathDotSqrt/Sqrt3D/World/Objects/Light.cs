using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World.Objects {
	public abstract class Light : Object3D {

		public abstract LightType Type {
			get;
		}
		public Color Color {
			get;
			set;
		}
		public float Intensity {
			get;
			set;
		}

		public Light(Color color, float intensity) {
			this.Color = color;
			this.Intensity = intensity;
		}

		public override string ToString() {
			string output = base.ToString();
			output += "-----Light-----" + "\r\n";
			output += "Color: " + Color.ToString() + "\r\n";
			output += "Intensity: " + Intensity + "\r\n";
			return output;
		}
		public override void Dispose() {

		}
	}
}
