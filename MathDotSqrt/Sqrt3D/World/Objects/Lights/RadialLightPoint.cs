using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.Util.Math;

namespace MathDotSqrt.Sqrt3D.World.Objects.Lights {
	public class RadialLightPoint : Light {
		public RadialLightPoint(Color color, float intensity) : base(color, intensity) {
		}

		public override LightType Type {
			get {
				return LightType.RadialLight;
			}
		}
	}
}
