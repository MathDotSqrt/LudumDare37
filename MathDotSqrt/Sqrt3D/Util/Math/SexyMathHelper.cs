using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.Math {
	/// <summary>
	/// Seriously the sole purpose of this class is to provide a degree to rad conversion because 
	/// Im a lasy peice of shit and c#'s math class for what ever reason does not have it
	/// </summary>
	public class SexyMathHelper {
		public static float ToRad(float angle) {
			return ( angle * (float)System.Math.PI ) / 180;
		}
		public static float ToDeg(float rad) {
			return (180 * rad) / (float)System.Math.PI;
		}
	}
}
