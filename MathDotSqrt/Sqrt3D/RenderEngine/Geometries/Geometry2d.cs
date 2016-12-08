using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {
	public class Geometry2d : Geometry{
		//TODO some other nigga shit
		protected List<float> positions;
		protected List<float> textureUVs;
		protected List<int> indicies;

		public bool HasCalculatedBoundingRect {
			get;
			private set;
		}
		public float MinX {
			get;
			private set;
		}
		public float MinY {
			get;
			private set;
		}
		public float MaxX {
			get;
			private set;
		}
		public float MaxY {
			get;
			private set;
		}
		public float BoundingWidth {
			get {
				if(!HasCalculatedBoundingRect)
					CalcBoundingRect();

				return Math.Abs(MaxX - MinX);
			}
		}
		public float BoundingHeight {
			get {
				if(!HasCalculatedBoundingRect)
					CalcBoundingRect();

				return Math.Abs(MaxY - MinY);
			}
		}

		private void CalcBoundingRect() {
			if(positions == null || positions.Count == 0 || positions.Count % 2 == 1) {
				Output.Warning("Geometry2d.CalcBoundingRect: positions are [null/empty set/odd]");
				return;
			}

			MinX = positions[0];
			MinY = positions[1];

			MaxX = positions[0];
			MaxY = positions[1];

			for(int i = 1; i < positions.Count / 2; i++) {
				float x = positions[i * 2];
				float y = positions[i * 2 + 1];

				if(x < MinX)
					MinX = x;
				if(x > MaxX)
					MaxX = x;
				if(y < MinY)
					MinY = y;
				if(y > MaxY)
					MaxY = y;
			}

			HasCalculatedBoundingRect = true;
		}
	}
}
