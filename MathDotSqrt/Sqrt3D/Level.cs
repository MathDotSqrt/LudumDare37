using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MathDotSqrt.Sqrt3D {
	public class Level {
		private Scene scene;
		private Geometry planeGeometry;

		public int materialNum = 0;
		private List<Material> materials;

		private Player player;

		public int currentLevel = 0;
		public List<List<Wall>> walls;
		public List<List<Mesh>> meshes;
		public List<Node> nodes;

		public int tempLevel = -1;
		public Vector3 tempNodeChunk;
		public Orientation tempLookAt;



		public Level(Scene scene, Player player) {
			this.scene = scene;
			this.player = player;
			materials = new List<Material>();
			walls = new List<List<Wall>>();
			meshes = new List<List<Mesh>>();
			nodes = new List<Node>();
			FileWriter.Clear();
			materials.Add(new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("wall2.png", true),
				NormalMap = TextureLoader.LoadModelTexture("wall2_normal.png", true)
			});
			materials.Add(new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("wall2.png", true),
				NormalMap = TextureLoader.LoadModelTexture("wall2_normal.png", true)
			});
			materials.Add(new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("tim.jpg", true),
				NormalMap = TextureLoader.LoadModelTexture("wall2_normal.png", true)
			});

			for(int i = 0; i < 6; i++) {
				walls.Add(new List<Wall>());
				meshes.Add(new List<Mesh>());
			}
			planeGeometry = OBJBumpLoader.LoadOBJ("plane");

			ChangeLevel(0);

			//////////////////////////////////////
			BuildLevel();
			//////////////////////////////////////
			RotZ90(0, 1);
			RotZ90(1, 2);
			RotZ90(2, 3);
			ChangeLevel(0);
		}

		public void ChangeLevel(int level) {
			level %= 4;
			currentLevel = level;
			scene.meshes = meshes[currentLevel];

		}

		public void Center(int level, float xOffset, float yOffset, float zOffset) {
			List<Wall> wallList = walls[level];
			List<Mesh> meshList = meshes[level];

			walls[level] = new List<Wall>();
			meshes[level] = new List<Mesh>();

			ChangeLevel(level);

			for(int i = 0; i < wallList.Count; i++) {
				Wall wall = wallList[i];
				Mesh mesh = meshList[i];
				Material m = mesh.Material;

				float x = wall.chunk.X;
				float y = wall.chunk.Y;
				float z = wall.chunk.Z;
				switch(wallList[i].normalOrientation) {
					case Orientation.NegX:
					BuildWallPosX( x + xOffset, y + yOffset, z + zOffset, m);
					break;
					case Orientation.PosX:
					BuildWallNegX(x + xOffset, y + yOffset, z + zOffset, m);
					break;
					case Orientation.PosY:
					BuildWallNegY(x + xOffset, y + yOffset, z + zOffset, m);
					break;
					case Orientation.NegY:
					BuildWallPosY(x + xOffset, y + yOffset, z + zOffset, m);
					break;
					case Orientation.PosZ:
					BuildWallNegZ( x + xOffset, y + yOffset, z + zOffset, m);
					break;
					case Orientation.NegZ:
					BuildWallPosZ(x + xOffset, y + yOffset, z + zOffset, m);
					break;
				}
			}
		}

		public void BuildWallPosX(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = -90;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(-1, 0, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.X));
		}
		public void BuildWallNegX(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = 90;
			scene.Add(mesh);
			
			walls[currentLevel].Add(new Wall(new Vector3(1, 0, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.X));

		}
		public void BuildWallNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10, z * 10 + 5);
			mesh.Rotation.X = -90;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0, 1, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.Y));
		}
		public void BuildWallPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 10, z * 10 + 5);
			mesh.Rotation.X = 90;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0, -1, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.Y));

		}
		public void BuildWallNegZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10);
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0,0,1), mesh.Position, new Vector3(x, y, z), mesh.Position.Z));
		}
		public void BuildWallPosZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10 + 10);
			mesh.Rotation.Y = 180;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0, 0, -1), mesh.Position, new Vector3(x, y, z), mesh.Position.Z));
		}
		public void BuildWallPlayer(Player p, Material m = null) {
			float x = p.Chunk.X;
			float y = p.Chunk.Y;
			float z = p.Chunk.Z;

			if(p.camera.Rotation.X > 90 - 35) {
				BuildWallNegY(x, y, z, m);
				FileWriter.WriteFile("BuildWallNegY(" + x + ", " + y + ", " + z + ", materials[" + materialNum+ "]);");
			}
			else if(p.camera.Rotation.X < -90 + 35) {
				BuildWallPosY(x, y, z, m);
				FileWriter.WriteFile("BuildWallPosY(" + x + ", " + y + ", " + z + ", materials[" + materialNum + "]);");
			}
			else {
				switch(p.o) {
					case Orientation.PosX:
					BuildWallPosX(x, y, z, m);
					FileWriter.WriteFile("BuildWallPosX(" + x + ", " + y + ", " + z + ", materials[" + materialNum + "]);");
					break;
					case Orientation.NegX:
					BuildWallNegX(x, y, z, m);
					FileWriter.WriteFile("BuildWallNegX(" + x + ", " + y + ", " + z + ", materials[" + materialNum + "]);");
					break;
					case Orientation.PosZ:
					BuildWallPosZ(x, y, z, m);
					FileWriter.WriteFile("BuildWallPosZ(" + x + ", " + y + ", " + z + ", materials[" + materialNum + "]);");
					break;
					case Orientation.NegZ:
					BuildWallNegZ(x, y, z, m);
					FileWriter.WriteFile("BuildWallNegZ(" + x + ", " + y + ", " + z + ", materials[" + materialNum + "]);");
					break;
				}
			}
			
		}


		public void BuildXWalls(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildWallPosX(x, y, z, m);
			BuildWallNegX(x, y, z, m);
		}
		public void BuildYWalls(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildWallPosY(x, y, z, m);
			BuildWallNegY(x, y, z, m);
		}
		public void BuildZWalls(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildWallPosZ(x, y, z, m);
			BuildWallNegZ(x, y, z, m);
		}

		public void BuildXTunnle(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildYWalls(x, y, z, m);
			BuildZWalls(x, y, z, m);
		}
		public void BuildYTunnle(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildXWalls(x, y, z, m);
			BuildZWalls(x, y, z, m);
		}
		public void BuildZTunnle(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildYWalls(x, y, z, m);
			BuildXWalls(x, y, z, m);
		}
		public void BuildPlayerTunnle(Player p, Material m = null) {
			float x = p.Chunk.X;
			float y = p.Chunk.Y;
			float z = p.Chunk.Z;
			switch(p.o) {
				case Orientation.PosX:
				case Orientation.NegX:
				BuildXTunnle(x, y, z, m);
				FileWriter.WriteFile("BuildXTunnle(" + x + ", " + y + ", " +  z + ");");
				break;
				case Orientation.PosY:
				case Orientation.NegY:
				BuildYTunnle(x, y, z, m);
				FileWriter.WriteFile("BuildYTunnle(" + x + ", " + y + ", " + z + ");");
				break;
				case Orientation.PosZ:
				case Orientation.NegZ:
				BuildZTunnle(x, y, z, m);
				FileWriter.WriteFile("BuildZTunnle(" + x + ", " + y + ", " + z + ");");
				break;
			}
		}

		public void BuildCornerPosXPosZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildYWalls(x, y, z, m);
			BuildWallNegX(x, y, z, m);
			BuildWallNegZ(x, y, z, m);
		}
		public void BuildCornerNegXPosZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildYWalls(x, y, z, m);
			BuildWallPosX(x, y, z, m);
			BuildWallNegZ(x, y, z, m);
		}
		public void BuildCornerPosXNegZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildYWalls(x, y, z, m);
			BuildWallNegX(x, y, z, m);
			BuildWallPosZ(x, y, z, m);
		}
		public void BuildCornerNegXNegZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildYWalls(x, y, z, m);
			BuildWallPosX(x, y, z, m);
			BuildWallPosZ(x, y, z, m);
		}
		public void BuildCornerPosXPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildZWalls(x, y, z, m);
			BuildWallNegX(x, y, z, m);
			BuildWallNegY(x, y, z, m);
		}
		public void BuildCornerNegXPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildZWalls(x, y, z, m);
			BuildWallPosX(x, y, z, m);
			BuildWallNegY(x, y, z, m);
		}
		public void BuildCornerPosXNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildZWalls(x, y, z, m);
			BuildWallNegX(x, y, z, m);
			BuildWallPosY(x, y, z, m);
		}
		public void BuildCornerNegXNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildZWalls(x, y, z, m);
			BuildWallPosX(x, y, z, m);
			BuildWallPosY(x, y, z, m);
		}
		public void BuildCornerPosZPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildXWalls(x, y, z, m);
			BuildWallNegZ(x, y, z, m);
			BuildWallNegY(x, y, z, m);
		}
		public void BuildCornerNegZPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildXWalls(x, y, z, m);
			BuildWallPosZ(x, y, z, m);
			BuildWallNegY(x, y, z, m);
		}
		public void BuildCornerPosZNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildXWalls(x, y, z, m);
			BuildWallNegZ(x, y, z, m);
			BuildWallPosY(x, y, z, m);
		}
		public void BuildCornerNegZNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;

			BuildXWalls(x, y, z, m);
			BuildWallPosZ(x, y, z, m);
			BuildWallPosY(x, y, z, m);
		}


		public void BuildCube(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? materials[0] : m;
			BuildWallPosX(x - 1, y, z);
			BuildWallNegX(x + 1, y, z);
			BuildWallPosZ(x, y, z - 1);
			BuildWallNegZ(x, y, z + 1);
			BuildWallPosY(x, y - 1, z);
			BuildWallNegY(x, y + 1, z);
		}
		public void DelChunk(float x, float y, float z, bool write = false) {
			for(int i = walls[currentLevel].Count - 1; i >= 0; i--) {
				if(walls[currentLevel][i].chunk == new Vector3(x, y, z)) {
					walls[currentLevel].RemoveAt(i);
					meshes[currentLevel].RemoveAt(i);
					scene.meshes = meshes[currentLevel];
				}
			}
			if(write)
				FileWriter.WriteFile("DelChunk( " + x + ",  " + y +",  " + z + ");");
		}

		public void RotZ180() {
			
		}
		public void RotZ90(int pullFrom, int levelTo) {
			ChangeLevel(levelTo);

			for(int i = 0; i < walls[pullFrom].Count; i++) {
				Wall wall = walls[pullFrom][i];
				Mesh mesh = meshes[pullFrom][i];
				Material m = mesh.Material;

				float x = wall.chunk.X;
				float y = wall.chunk.Y;
				float z = wall.chunk.Z;
				switch(walls[pullFrom][i].normalOrientation) {
					case Orientation.NegX:
					BuildWallPosY(-y, x, z, m);
					break;
					case Orientation.PosX:
					BuildWallNegY(-y, x, z, m);
					break;
					case Orientation.PosY:
					BuildWallPosX(-y, x, z, m);
					break;
					case Orientation.NegY:
					BuildWallNegX(-y, x, z, m);
					break;
					case Orientation.PosZ:
					BuildWallNegZ(-y, x, z, m);
					break;
					case Orientation.NegZ:
					BuildWallPosZ(-y, x, z, m);
					break;
				}
			}
		}
		public void Update() {
			if(!player.godMode) {
				foreach(Node node in nodes) {
					if(node.levelAt != currentLevel)
						continue;
					if(node.IsLooking(player)) {
						player.Teleport(node);
						ChangeLevel(node.levelTo);
					}
				}
			}

			if(player.godMode) {
				if(Keyboard.GetState().IsKeyDown(Key.LControl))
					Output.Good(player.Chunk + " | " + player.o);
				if(Input.isAltTyped) {
					this.tempLevel = currentLevel;
					this.tempLookAt = player.o;
					this.tempNodeChunk = player.Chunk;
					Output.Good("Set");
				}
				if(Input.isRControlTyped) {

					string output = "nodes.Add(new Node(" + this.tempLevel + "," +
						 this.tempNodeChunk.X + "," + this.tempNodeChunk.Y + "," + this.tempNodeChunk.Z + ",Orientation." + this.tempLookAt
						 + "," + currentLevel + "," + player.Chunk.X + "," + player.Chunk.Y + "," + player.Chunk.Z + ",Orientation." + player.o+"));";
					Output.Good("Node set\n" + output);
					FileWriter.WriteFile(output);
					nodes.Add(new Node(this.tempLevel, this.tempNodeChunk.X, this.tempNodeChunk.Y, this.tempNodeChunk.Z, this.tempLookAt, currentLevel, player.Chunk.X, player.Chunk.Y, player.Chunk.Z, player.o));
				}

				if(Input.isTTyped)
					BuildPlayerTunnle(player, materials[materialNum]);
				if(Input.IsEnterTyped)
					BuildWallPlayer(player, materials[materialNum]);
				if(Input.IsDelTyped) {
					meshes[currentLevel].RemoveAt(meshes[currentLevel].Count - 1);
					scene.meshes = meshes[currentLevel];
					walls[currentLevel].RemoveAt(walls[currentLevel].Count - 1);
					FileWriter.DeleteLastLine();
				}
				if(Input.IsRShift)
					DelChunk(player.Chunk.X, player.Chunk.Y, player.Chunk.Z, true);

				if(Keyboard.GetState().IsKeyDown(Key.Number0)) {
					materialNum = 9;
					Output.Good("Material: " + materialNum);
				}
				if(Keyboard.GetState().IsKeyDown(Key.Number1)) {
					materialNum = 0;
					Output.Good("Material: " + materialNum);
				}
				if(Keyboard.GetState().IsKeyDown(Key.Number2)) {
					materialNum = 1;
					Output.Good("Material: " + materialNum);
				}
				if(Keyboard.GetState().IsKeyDown(Key.Number3)) {
					materialNum = 2;
					Output.Good("Material: " + materialNum);
				}
				if(Keyboard.GetState().IsKeyDown(Key.Number4)) {
					materialNum = 3;
					Output.Good("Material: " + materialNum);
				}

			}

			//Output.Good(player.camera.Rotation.Y);
		}

		public void BuildLevel() {
			BuildWallNegY(0,0,0);
			BuildWallPosZ(0, 0, 0);

			BuildWallNegY(0, 0, -1);
			BuildWallNegX(0, 0, 0);
			BuildWallPosY(0, 0, 0);
			BuildWallPosX(0, 0, 0);
			BuildWallNegX(0, 0, -1);
			BuildWallPosY(0, 0, -1);
			BuildWallPosX(0, 0, -1);
			BuildZTunnle(0, 0, -2);
			BuildZTunnle(0, 0, -3);
			BuildZTunnle(0, 0, -4);

			for(int i = 0; i < 11; i++) {
				for(int j = 0; j < 11; j++) {
					BuildWallNegY(-i + 5, 0, -j - 5, materials[0]);
					BuildWallPosY(-i + 5, 10, -j - 5, materials[0]);
					BuildWallPosX(5, i, -j - 5, materials[1]);
					BuildWallNegX(-5, i, -j - 5, materials[1]);
					BuildWallPosZ(-i + 5, j, -5, materials[1]);
					BuildWallNegZ(-i + 5, j, -15, materials[1]);
				}
			}

			//eye hall
			DelChunk(0, 0, - 15);
			BuildWallNegY(0, 0, -15, materials[0]);
			BuildZTunnle(0, 0, -16);
			BuildZTunnle(0, 0, -17);
			BuildZTunnle(0, 0, -18);
			BuildZTunnle(0, 0, -19);
			BuildZTunnle(0, 0, -20);

			//eye room
			BuildWallPosZ(0, -1, -21, materials[0]);
			BuildWallPosZ(1, -1, -21, materials[0]);
			BuildWallPosZ(-1, -1, -21, materials[0]);
			BuildWallPosZ(-1, 0, -21, materials[0]);
			BuildWallPosZ(-1, 1, -21, materials[0]);
			BuildWallPosZ(0, 1, -21, materials[0]);
			BuildWallPosZ(1, 1, -21, materials[0]);
			BuildWallPosZ(1, 0, -21, materials[0]);
			BuildWallPosY(-1, 1, -21, materials[0]);
			BuildWallPosY(0, 1, -21, materials[0]);
			BuildWallPosY(1, 1, -21, materials[0]);
			BuildWallNegY(-1, -1, -21, materials[0]);
			BuildWallNegY(0, -1, -21, materials[0]);
			BuildWallNegY(1, -1, -21, materials[0]);
			BuildWallNegY(1, -1, -22, materials[0]);
			BuildWallNegY(0, -1, -22, materials[0]);
			BuildWallNegY(-1, -1, -22, materials[0]);
			BuildWallNegY(-1, -1, -23, materials[0]);
			BuildWallNegY(0, -1, -23, materials[0]);
			BuildWallNegY(1, -1, -23, materials[0]);
			BuildWallPosY(1, 1, -22, materials[0]);
			BuildWallPosY(0, 1, -22, materials[0]);
			BuildWallPosY(-1, 1, -22, materials[0]);
			BuildWallPosY(-1, 1, -23, materials[0]);
			BuildWallPosY(0, 1, -23, materials[0]);
			BuildWallPosY(1, 1, -23, materials[0]);
			BuildWallNegX(-1, -1, -21, materials[0]);
			BuildWallNegX(-1, -1, -22, materials[0]);
			BuildWallNegX(-1, -1, -23, materials[0]);
			BuildWallNegX(-1, 0, -23, materials[0]);
			BuildWallNegX(-1, 0, -22, materials[0]);
			BuildWallNegX(-1, 0, -21, materials[0]);
			BuildWallNegX(-1, 1, -21, materials[0]);
			BuildWallNegX(-1, 1, -22, materials[0]);
			BuildWallNegX(-1, 1, -23, materials[0]);
			BuildWallPosX(1, -1, -21, materials[0]);
			BuildWallPosX(1, -1, -22, materials[0]);
			BuildWallPosX(1, -1, -23, materials[0]);
			BuildWallPosX(1, 0, -23, materials[0]);
			BuildWallPosX(1, 0, -22, materials[0]);
			BuildWallPosX(1, 0, -21, materials[0]);
			BuildWallPosX(1, 1, -21, materials[0]);
			BuildWallPosX(1, 1, -22, materials[0]);
			BuildWallPosX(1, 1, -23, materials[0]);
			BuildWallNegZ(1, -1, -23, materials[0]);
			BuildWallNegZ(1, 0, -23, materials[0]);
			BuildWallNegZ(1, 1, -23, materials[0]);
			BuildWallNegZ(0, 1, -23, materials[0]);
			BuildWallNegZ(-1, 1, -23, materials[0]);
			BuildWallNegZ(-1, 0, -23, materials[0]);
			BuildWallNegZ(-1, -1, -23, materials[0]);
			BuildWallNegZ(0, -1, -23, materials[0]);
			BuildZTunnle(0, 0, -24);
			BuildWallNegZ(0, 0, -24, materials[2]);
			nodes.Add(new Node(0, 0, -1, -22, Orientation.NegZ, 2, 0, -1, -22, Orientation.NegZ));
			nodes.Add(new Node(0, 1, -1, -22, Orientation.NegZ, 2, 1, -1, -22, Orientation.NegZ));
			nodes.Add(new Node(0, -1, -1, -22, Orientation.NegZ, 2, -1, -1, -22, Orientation.NegZ));
			nodes.Add(new Node(0, -1, -1, -23, Orientation.NegZ, 2, -1, -1, -23, Orientation.NegZ));
			nodes.Add(new Node(0, 0, -1, -23, Orientation.NegZ, 2, 0, -1, -23, Orientation.NegZ));
			nodes.Add(new Node(0, 1, -1, -23, Orientation.NegZ, 2, 1, -1, -23, Orientation.NegZ));
			nodes.Add(new Node(0, 1, -1, -21, Orientation.NegZ, 2, 1, -1, -21, Orientation.NegZ));
			nodes.Add(new Node(0, 0, -1, -21, Orientation.PosZ, 2, 0, -1, -21, Orientation.PosZ));
			nodes.Add(new Node(0, 0, -1, -22, Orientation.PosZ, 2, 0, -1, -22, Orientation.PosZ));


		}
	}

	public class Wall{
		public float x1, y1, z1, x2, y2, z2;
		public Orientation normalOrientation;
		public Vector3 normal;
		public Vector3 position;
		public float offset;
		public Vector3 chunk;

		public Wall(float x1, float y1, float z1, float x2, float y2, float z2, Orientation O) {
			this.x1 = x1;
			this.y1 = y1;
			this.z1 = z1;
			this.x2 = x2;
			this.y2 = y2;
			this.z2 = z2;

			this.normalOrientation = O;
		}

		public Wall(Vector3 normal, Vector3 position, Vector3 chunk, float offset) {
			this.normal = normal;
			this.offset = offset;
			this.position = position;
			this.chunk = chunk;

			if(normal == new Vector3(0,0,1)) {
				normalOrientation = Orientation.PosZ;
			}
			if(normal == new Vector3(0, 0, -1)) {
				normalOrientation = Orientation.NegZ;

			}
			if(normal == new Vector3(0, 1, 0)) {
				normalOrientation = Orientation.PosY;

			}
			if(normal == new Vector3(0, -1, 0)) {
				normalOrientation = Orientation.NegY;

			}
			if(normal == new Vector3(1, 0, 0)) {
				normalOrientation = Orientation.PosX;

			}
			if(normal == new Vector3(-1, 0, 0)) {
				normalOrientation = Orientation.NegX;

			}
		}
	}
}
