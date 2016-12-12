using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D {
	public class Node {
		public int levelAt;
		public int levelTo;
		public Vector3 ChunkLocation;
		public Vector3 ChunkTeleport;
		public Orientation LookOrientation;
		public Orientation RotatePlayer;

		public Node(int levelAt, float cx, float cy, float cz, Orientation lookAt, int levelTo, float relX, float relY, float relZ, Orientation lookTo)
			: this(levelAt, new Vector3(cx, cy, cz), lookAt, levelTo, new Vector3(relX, relY, relZ), lookTo) {

		}

		public Node(int levelAt, Vector3 pos, Orientation lookAt, int levelTo, Vector3 rel, Orientation lookTo) {
			this.levelAt = levelAt;
			ChunkLocation = pos;
			LookOrientation = lookAt;
			this.levelTo = levelTo;
			ChunkTeleport = rel;
			RotatePlayer = lookTo;
		}

		public bool IsLooking(Player player) {
			if(player.Chunk == ChunkLocation) {
				float rotY;
				if(player.camera.Rotation.Y > 0) {
					rotY = player.camera.Rotation.Y % 360;
				}
				else {
					rotY = 360 - Math.Abs(player.camera.Rotation.Y) % 360;
				}

				switch(LookOrientation) {
					case Orientation.PosZ:
					return ( rotY > 180 - 35 && rotY < 180 + 35 );
					case Orientation.NegZ:
					return ( rotY < 35 || rotY > 360 - 35 );
					case Orientation.PosX:
					return ( rotY > 90 - 35 && rotY < 90 + 35 );
					case Orientation.NegX:
					return ( rotY > 270 - 35 && rotY < 270 + 35 );

				}

			}
			return false;
		}


	}
}
