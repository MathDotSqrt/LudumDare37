﻿using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D {
	public class Node {
		public Vector3 ChunkLocation;
		public Vector3 ChunkTeleport;
		public Orientation LookOrientation;
		public Orientation RotatePlayer;

		public Node(float cx, float cy, float cz, float relX, float relY, float relZ, Orientation o, Orientation p) 
			: this(new Vector3(cx, cy, cz), new Vector3(relX, relY, relZ), o, p){

		}
		public Node(Vector3 chunkLocation, Vector3 relTeleport, Orientation lookOrientation, Orientation rotatePlayer) {
			ChunkLocation = chunkLocation;
			ChunkTeleport = relTeleport;
			this.LookOrientation = lookOrientation;
			this.RotatePlayer = rotatePlayer;
		}

		public bool IsLooking(Player player) {
			if(player.Chunk == ChunkLocation) {
				float rotY = Math.Abs(player.camera.Rotation.Y % 360);
				switch(LookOrientation) {
					case Orientation.PosZ:
					return ( rotY > 180 - 35 && rotY < 180 + 35 );
					case Orientation.NegZ:
					return ( rotY > 0 - 35 && rotY < 0 + 35 );
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
