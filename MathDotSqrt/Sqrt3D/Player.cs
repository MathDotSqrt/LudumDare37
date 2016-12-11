using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Cameras;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D {
	public class Player {
		public Camera camera;
		public Vector3 Chunk;
		public Vector3 ChunkPosition;

		public Vector3 vel;
		public float gravity = -.05f;
		public float drag = .01f;
		public float moveSpeed = .05f;

		public bool canMovePosX;
		public bool canMoveNegX;
		public bool canMovePosY;
		public bool canMoveNegY;
		public bool canMovePosZ;
		public bool canMoveNegZ;

		public float radX = 2f;
		public float radZ = 2;

		public float radNegY = 20;
		public float radPosY = 1;

		public Vector3 minBounds = new Vector3(-1.5f, -7, -1.5f);
		public Vector3 maxBounds = new Vector3(1.5f, 2, 1.5f);

		public Player(float x, float y, float z) : this(new Vector3(x, y, z)){

		}
		public Player(Vector3 position) {
			camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 1000);
			camera.Position = position;
			Chunk = new Vector3();
			ChunkPosition = new Vector3();

		}

		public void TestCollision(List<Wall> walls) {
			

			Vector3 min = minBounds + camera.Position;
			Vector3 max = maxBounds + camera.Position;
			canMovePosX = true;
			canMoveNegX = true;
			canMovePosY = true;
			canMoveNegY = true;
			canMovePosZ = true;
			canMoveNegZ = true;

			foreach(Wall wall in walls) {
				if(( wall.position - camera.Position ).LengthSquared < 121 && PlaneCollision(wall.normal, wall.offset, min, max)) {
					switch(wall.normalOrientation) {
						case Orientation.PosZ:
						if(!PlanePos(new Vector3(1, 0, 0), 10 + wall.position.X - 5, min, max))
							if(!PlanePos(new Vector3(-1, 0, 0), -wall.position.X + 5, min, max))
								if(!PlanePos(new Vector3(0, 1, 0), wall.position.Y + 5, min, max))
									if(!PlanePos(new Vector3(0, -1, 0), -wall.position.Y + 5, min, max))
										canMoveNegZ = false;
						break;
						case Orientation.NegZ:
						if(!PlanePos(new Vector3(1, 0, 0), 10 + wall.position.X - 5, min, max))
							if(!PlanePos(new Vector3(-1, 0, 0), -wall.position.X + 5, min, max))
								if(!PlanePos(new Vector3(0, 1, 0), wall.position.Y + 5, min, max))
									if(!PlanePos(new Vector3(0, -1, 0), -wall.position.Y + 5, min, max))
										canMovePosZ = false;
						break;
						case Orientation.NegX:
						if(!PlanePos(new Vector3(0, 0, 1), 10 + wall.position.Z - 5, min, max))
							if(!PlanePos(new Vector3(0, 0, -1), -wall.position.Z + 5, min, max))
								if(!PlanePos(new Vector3(0, 1, 0), wall.position.Y + 5, min, max))
									if(!PlanePos(new Vector3(0, -1, 0), -wall.position.Y + 5, min, max))
										canMovePosX = false;
						break;
						case Orientation.PosX:
						if(!PlanePos(new Vector3(0, 0, 1), 10 + wall.position.Z - 5, min, max))
							if(!PlanePos(new Vector3(0, 0, -1), -wall.position.Z + 5, min, max))
								if(!PlanePos(new Vector3(0, 1, 0), wall.position.Y + 5, min, max))
									if(!PlanePos(new Vector3(0, -1, 0), -wall.position.Y + 5, min, max))
										canMoveNegX = false;
						break;
						case Orientation.PosY:
						if(!PlanePos(new Vector3(0, 0, 1), 10 + wall.position.Z - 5, min, max))
							if(!PlanePos(new Vector3(0, 0, -1), -wall.position.Z + 5, min, max))
								if(!PlanePos(new Vector3(0, 1, 0), wall.position.Y + 5, min, max))
									if(!PlanePos(new Vector3(0, -1, 0), -wall.position.Y + 5, min, max))
										canMoveNegY = false;
						break;
						case Orientation.NegY:
						if(!PlanePos(new Vector3(0, 0, 1), 10 + wall.position.Z - 5, min, max))
							if(!PlanePos(new Vector3(0, 0, -1), -wall.position.Z + 5, min, max))
								if(!PlanePos(new Vector3(0, 1, 0), wall.position.Y + 5, min, max))
									if(!PlanePos(new Vector3(0, -1, 0), -wall.position.Y + 5, min, max))
										canMovePosY = false;
						break;

					}
				}
			}
			
		}

		public bool PlaneCollision(Vector3 normal, float offset, Vector3 min, Vector3 max) {
			bool isNeg = normal.X == 1 | normal.Y == 1 | normal.Z == 1;
			offset = !isNeg ? -offset : offset;

			Vector3 vec1;
			Vector3 vec2;

			if(normal.X >= 0) {
				vec1.X = min.X;
				vec2.X = max.X;
			}
			else {
				vec1.X = max.X;
				vec2.X = min.X;
			}
			if(normal.Y >= 0) {
				vec1.Y = min.Y;
				vec2.Y = max.Y;
			}
			else {
				vec1.Y = max.Y;
				vec2.Y = min.Y;
			}
			if(normal.Z >= 0) {
				vec1.Z = min.Z;
				vec2.Z = max.Z;
			}
			else {
				vec1.Z = max.Z;
				vec2.Z = min.Z;
			}

			float posSide = ( normal.X * vec1.X ) + ( normal.Y * vec1.Y ) + ( normal.Z * vec1.Z ) - offset;
			if(posSide > 0) {
				return false;
			}
			float negSide = ( normal.X * vec2.X ) + ( normal.Y * vec2.Y ) + ( normal.Z * vec2.Z ) - offset;
			if(negSide < 0) {
				return false;
			}

			return true;
		}
		public bool PlanePos(Vector3 normal, float offset, Vector3 min, Vector3 max) {
			Vector3 vec1;
			Vector3 vec2;

			if(normal.X >= 0) {
				vec1.X = min.X;
				vec2.X = max.X;
			}
			else {
				vec1.X = max.X;
				vec2.X = min.X;
			}
			if(normal.Y >= 0) {
				vec1.Y = min.Y;
				vec2.Y = max.Y;
			}
			else {
				vec1.Y = max.Y;
				vec2.Y = min.Y;
			}
			if(normal.Z >= 0) {
				vec1.Z = min.Z;
				vec2.Z = max.Z;
			}
			else {
				vec1.Z = max.Z;
				vec2.Z = min.Z;
			}

			bool pos = false;
			bool neg = false;
			float posSide = ( normal.X * vec1.X ) + ( normal.Y * vec1.Y ) + ( normal.Z * vec1.Z ) -(offset);
			if(posSide > 0) {
				pos = true;
			}
			float negSide = ( normal.X * vec2.X ) + ( normal.Y * vec2.Y ) + ( normal.Z * vec2.Z ) - (offset);
			if(negSide < 0) {
				neg = true;
			}

			return pos && !neg;
		}

		public void Update() {
			Chunk.X = (float)Math.Floor((double)camera.Position.X / 10.0);
			Chunk.Y = (float)Math.Floor((double)camera.Position.Y / 10.0);
			Chunk.Z = (float)Math.Floor((double)camera.Position.Z / 10.0);
			ChunkPosition.X = camera.Position.X - Chunk.X * 10;
			ChunkPosition.Y = camera.Position.Y - Chunk.Y * 10;
			ChunkPosition.Z = camera.Position.Z - Chunk.Z * 10;

			vel.Y += gravity;
			if((vel.Y < 0 && !canMoveNegY) || (vel.Y > 0 && !canMovePosY))
				vel.Y = 0;

			//camera.Position.Y += vel.Y;
		}

		

		public void Jump() {
			vel.Y = 1.3f;
		}

		public void Teleport(Node node) {
			//Output.Good("looking");
			//camera.Position = node.ChunkTeleport * 10;
			//camera.Position.X += ChunkPosition.Y;
			//camera.Position.Y += ChunkPosition.X;
			//camera.Position.Z += ChunkPosition.Z;
			float rot = Math.Abs(camera.Rotation.Y % 90);

			switch(node.RotatePlayer) {
				case Orientation.PosX:
				//camera.Rotation.Y = rot;
				break;
				case Orientation.NegX:
				//camera.Rotation.Y = rot + 180;
				break;
				case Orientation.PosZ:
				//camera.Rotation.Y = rot + 270;
				break;
				case Orientation.NegZ:
				if(rot > 45)
					rot = 90 - rot;
					camera.Rotation.Y = rot + 180;
				break;
			}
		}

		public void MoveForward(float vel) {
			float rad = SexyMathHelper.ToRad(camera.Rotation.Y);
			Vector2 vec = new Vector2();

			vec.X += vel * (float)Math.Sin(rad);
			vec.Y += vel * -(float)Math.Cos(rad);

			if((vec.X > 0 && canMovePosX) || ( vec.X < 0 && canMoveNegX))
				camera.Position.X += vec.X;
			if((vec.Y < 0 && canMoveNegZ) || (vec.Y > 0 && canMovePosZ))
				camera.Position.Z += vec.Y;
		}
		public void MoveLeft(float vel) {
			float rad = SexyMathHelper.ToRad(camera.Rotation.Y) + (float)Math.PI / 2;
			Vector2 vec = new Vector2();

			vec.X += vel * (float)Math.Sin(rad);
			vec.Y += vel * -(float)Math.Cos(rad);
			if(( vec.X > 0 && canMovePosX ) || ( vec.X < 0 && canMoveNegX ))
				camera.Position.X += vec.X;
			if(( vec.Y < 0 && canMoveNegZ ) || ( vec.Y > 0 && canMovePosZ ))
				camera.Position.Z += vec.Y;
		}

		public void MoveUp(float vel) {
			if((vel > 0 && canMovePosY) || (vel < 0 && canMoveNegY))
				camera.Position.Y += vel;
		
		}
	}
}
