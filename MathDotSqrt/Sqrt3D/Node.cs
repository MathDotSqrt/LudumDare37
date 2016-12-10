using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D {
	public class Node {
		public Vector2 ChunkLocation;
		public Vector3 RelTeleport;
		float Angle;

		public Node(Vector2 chunkLocation, Vector3 relTeleport, float angle) {
			ChunkLocation = chunkLocation;
			RelTeleport = relTeleport;
			Angle = angle;
		}

		public bool IsLooking(Player player) {
			if(player.Chunk == ChunkLocation) {
				float rotY = player.camera.Rotation.Y;
				if(rotY > Angle + 35 && rotY < Angle - 35) {
					return true;
				}
			}
			return false;
		}
	}
}
