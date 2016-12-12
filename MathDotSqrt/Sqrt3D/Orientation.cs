using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D {
	[Flags]
	public enum Orientation {
		None = -1,
		PosX = 0,
		PosY = 1,
		PosZ = 2,
		NegX = 3,
		NegY = 4,
		NegZ = 5,
	}
}
