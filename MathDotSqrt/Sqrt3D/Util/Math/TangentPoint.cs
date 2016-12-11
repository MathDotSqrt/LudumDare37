using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.Math {
	public class TangentPoint {
		public List<Vector3> Tangents;
		public Vector3 AveragedTangent;

		public TangentPoint() {
			Tangents = new List<Vector3>();
		}

		public void AddTangant(Vector3 tangant) {
			Tangents.Add(tangant);
		}

		public void AverageTangents() {
			if(Tangents.Count == 0) {
				return;
			}
			for(int i = 0; i < Tangents.Count; i++) {
				Vector3 tangent = Tangents[i];
				Vector3.Add(ref AveragedTangent, ref tangent, out AveragedTangent);
			}
			AveragedTangent.Normalize();
		}
	}
}
