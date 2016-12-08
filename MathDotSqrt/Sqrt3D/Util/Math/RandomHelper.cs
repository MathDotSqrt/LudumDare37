using MathDotSqrt.Sqrt3D.Util.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.Math {
	public class RandomHelper {
		private static Random random = new Random();

		public int Seed {
			get;
			set;
		}

		private float min = 0, max = 1;

		public float Min {
			get { return min; }
			set {
				if(value > max) {
					Output.Warning("Min: " + min + " is larger than Max: " + max);
					return;
				}

				min = value;
			}

		}
		public float Max {
			get { return max; }
			set {
				if(max < min) {
					Output.Warning("Max: " + max + " is larger than Min: " + min);
					return;
				}

				max = value;
			}
		}

		public RandomHelper() {
			Seed = RandomInt(0, 100000);
		}
		public RandomHelper(int seed) {
			Seed = seed;
		}
		public RandomHelper(float min, float max) {
			Min = min;
			Max = max;
		}
		public RandomHelper(float min, float max, int seed) {
			Min = min;
			Max = max;
			Seed = seed;
		}

		public static int RandomInt(int min, int max) {
			return (int)( random.NextDouble() * ( max - min ) + min );
		}
		public static float RandomFloat(float min, float max) {
			return (float)random.NextDouble() * (max - min) + min;
		}

		public void SetBounds(float min, float max) {
			Min = min;
			Max = max;
		}
		public int NextInt() {
			return RandomInt((int)Min, (int)Max);
		}
		public float NextFloat() {
			return RandomFloat(Min, Max);
		}
	}
}
