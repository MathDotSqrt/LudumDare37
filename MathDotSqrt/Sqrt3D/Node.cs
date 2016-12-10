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
		Orientation O;

		public Node(float cx, float cy, float relX, float relY, float relZ, Orientation o) 
			: this(new Vector2(cx, cy), new Vector3(relX, relY, relZ), o){

		}
		public Node(Vector2 chunkLocation, Vector3 relTeleport, Orientation o) {
			ChunkLocation = chunkLocation;
			RelTeleport = relTeleport;
			this.O = o;
		}

		public bool IsLooking(Player player) {
			if(player.Chunk == ChunkLocation) {
				float rotY = player.camera.Rotation.Y;
				switch(O) {
					case Orientation.NegZ:
					return ( rotY > 180 - 35 && rotY < 180 + 35 );
				}
			}
			return false;
		}
	}
}
