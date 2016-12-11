using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.Math;
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
		public Vector3 Chunk;
		public Vector3 ChunkPosition;

		public bool canMovePosX;
		public bool canMoveNegX;
		public bool canMovePosY;
		public bool canMoveNegY;
		public bool canMovePosZ;
		public bool canMoveNegZ;

		public float radX = 2f;
		public float radZ = 2;

		public float radY = 4;
		public float radPosY = 1;
		public Player(float x, float y, float z) : this(new Vector3(x, y, z)){

		}
		public Player(Vector3 position) {
			camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 1000);
			camera.Position = position;
			Chunk = new Vector3();
			ChunkPosition = new Vector3();

		}

		public void Update() {
			Chunk.X = (float)Math.Floor((double)camera.Position.X / 10.0);
			Chunk.Y = (float)Math.Floor((double)camera.Position.Y / 10.0);
			Chunk.Z = (float)Math.Floor((double)camera.Position.Z / 10.0);
			ChunkPosition.X = camera.Position.X - Chunk.X * 10;
			ChunkPosition.Y = camera.Position.Y - Chunk.Y * 10;
			ChunkPosition.Z = camera.Position.Z - Chunk.Z * 10;
		}

		public void TestCollision(List<Wall> walls) {
			canMoveNegZ = true;
			canMovePosZ = true;
			canMovePosX = true;
			canMoveNegX = true;
			canMovePosY = true;
			canMoveNegY = true;
			foreach(Wall wall in walls) {
				if(wall.O == Orientation.PosZ) {
					if(camera.Position.Z - radZ < wall.z1) {
						if(wall.x1 < camera.Position.X && camera.Position.X < wall.x2) {
							if(wall.y1 < camera.Position.Y && camera.Position.Y < wall.y2) {
								canMoveNegZ = false;
							}
						}
					}
				}
				else if(wall.O == Orientation.NegZ) {
					if(camera.Position.Z + radZ > wall.z1) {
						if(wall.x1 < camera.Position.X && camera.Position.X < wall.x2) {
							if(wall.y1 < camera.Position.Y && camera.Position.Y < wall.y2) {
								canMovePosZ = false;
							}
						}
					}
				}
				else if(wall.O == Orientation.NegX) {
					if(camera.Position.X + radX > wall.x1) {
						if(wall.z1 < camera.Position.Z && camera.Position.Z < wall.z2) {
							if(wall.y1 < camera.Position.Y && camera.Position.Y < wall.y2) {
								canMovePosX = false;
							}
						}
					}
				}
				else if(wall.O == Orientation.PosX) {
					if(camera.Position.X - radX < wall.x1) {
						if(wall.z1 < camera.Position.Z && camera.Position.Z < wall.z2) {
							if(wall.y1 < camera.Position.Y && camera.Position.Y < wall.y2) {
								canMoveNegX = false;
							}
						}
					}
				}
				else if(wall.O == Orientation.NegY) {
					if(camera.Position.Y + radPosY > wall.y1) {
						if(wall.z1 < camera.Position.Z && camera.Position.Z < wall.z2) {
							if(wall.x1 < camera.Position.X && camera.Position.X < wall.x2) {
								canMovePosY = false;
							}
						}
					}
				}
				else if(wall.O == Orientation.PosY) {
					if(camera.Position.Y - radY < wall.y1) {
						if(wall.z1 < camera.Position.Z && camera.Position.Z < wall.z2) {
							if(wall.x1 < camera.Position.X && camera.Position.X < wall.x2) {
								canMoveNegY = false;
							}
						}
					}
				}
			}
		}

		public void Teleport(Node node) {
			//Output.Good("looking");
			camera.Position = node.ChunkTeleport * 10;
			camera.Position += ChunkPosition;
		}

		public void MoveForward(float vel) {
			float rad = SexyMathHelper.ToRad(camera.Rotation.Y);
			Vector2 vec = new Vector2();

			vec.X += vel * (float)Math.Sin(rad);
			vec.Y += vel * -(float)Math.Cos(rad);

			Output.Good(( vec.Y < 0 || canMovePosZ ));
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
