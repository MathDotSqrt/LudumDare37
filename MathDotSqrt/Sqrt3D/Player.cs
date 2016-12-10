using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Cameras;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D {
	public class Player {
		public Camera camera;
		public Vector2 Chunk;

		public Player(float x, float y, float z) : this(new Vector3(x, y, z)){

		}
		public Player(Vector3 position) {
			camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 1000);
			camera.Position = position;
		}

		public void Update() {
			Chunk.X = (float)Math.Floor((double)camera.Position.X / 10.0);
			Chunk.Y = (float)Math.Floor((double)camera.Position.Z / 10.0);
		}
	}
}
