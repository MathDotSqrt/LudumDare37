using MathDotSqrt.Sqrt3D.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World.Objects.Lights {
	public class AmbientLight : Light {

		public override LightType Type {
			get { return LightType.AmbientLight; }
		}

		public AmbientLight(Color color, float intensity) : base(color, intensity) {

		}
	}
}
