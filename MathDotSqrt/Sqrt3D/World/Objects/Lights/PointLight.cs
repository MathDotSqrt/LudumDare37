using MathDotSqrt.Sqrt3D.Util.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World.Objects.Lights {
	public class PointLight : Light {

		public override LightType Type {
			get { return LightType.PointLight; }
		}

		
		public float Distance {
			get;
			set;
		}
		public float Decay {
			get;
			set;
		}

		public PointLight(Color color, float intensity) : base(color, intensity) {
			
		}
		public PointLight(Color color, float intensity, float distance, float decay) : base(color, intensity) {
			this.Distance = distance;
			this.Decay = decay;
		}
	}
}
