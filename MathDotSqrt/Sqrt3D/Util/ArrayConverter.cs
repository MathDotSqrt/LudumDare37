using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util {
	public static class ArrayConverter {

		public static float[] ConvertVectorListToFloatArray(List<Vector2> vectorList) {
			float[] vectorArray = new float[vectorList.Count * 2];

			for(int i = 0; i < vectorList.Count; i++) {
				vectorArray[i * 2 + 0] = vectorList[i].X;
				vectorArray[i * 2 + 1] = vectorList[i].Y;
			}

			return vectorArray;
		}

		public static float[] ConvertVectorListToFloatArray(List<Vector3> vectorList) {
			float[] vectorArray = new float[vectorList.Count * 3];

			for(int i = 0; i < vectorList.Count; i++) {
				vectorArray[i * 3 + 0] = vectorList[i].X;
				vectorArray[i * 3 + 1] = vectorList[i].Y;
				vectorArray[i * 3 + 2] = vectorList[i].Z;
			}

			return vectorArray;
		}

		public static float[] ConvertVectorArrayToFloatArray(Vector2[] vectors) {
			float[] vectorArray = new float[vectors.Length * 2];

			for(int i = 0; i < vectors.Length; i++) {
				vectorArray[i * 2 + 0] = vectors[i].X;
				vectorArray[i * 2 + 1] = vectors[i].Y;
			}

			return vectorArray;
		}

		public static float[] ConvertVectorArrayToFloatArray(Vector3[] vectors) {
			float[] vectorArray = new float[vectors.Length * 3];

			for(int i = 0; i < vectors.Length; i++) {
				vectorArray[i * 3 + 0] = vectors[i].X;
				vectorArray[i * 3 + 1] = vectors[i].Y;
				vectorArray[i * 3 + 2] = vectors[i].Z;
			}

			return vectorArray;
		}
	}
}
