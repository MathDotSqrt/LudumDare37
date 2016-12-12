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

		public bool solved1 = false;
		public bool solved2 = false;
		public bool solved3 = false;
		public bool solved4 = false;
		public bool canSolve = true;

		public int eyeRot = 2;

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
				Texture = TextureLoader.LoadModelTexture("invwall.png", true),
				NormalMap = TextureLoader.LoadModelTexture("invwall_normal.png", true)
			});
			materials.Add(new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("eye.png", true),
				NormalMap = TextureLoader.LoadModelTexture("wall2_normal.png", true)
			});
			materials.Add(new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("metal.png", true),
				NormalMap = TextureLoader.LoadModelTexture("metal_normal.png", true)
			});
			materials.Add(new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("dontlook.png", true),
				NormalMap = TextureLoader.LoadModelTexture("dontlook_normal.png", true)
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
				FileWriter.WriteFile("BuildXTunnle(" + x + ", " + y + ", " +  z + ", materials[" + materialNum + "]);");
				break;
				case Orientation.PosY:
				case Orientation.NegY:
				BuildYTunnle(x, y, z, m);
				FileWriter.WriteFile("BuildYTunnle(" + x + ", " + y + ", " + z + ", materials[" + materialNum + "]);");
				break;
				case Orientation.PosZ:
				case Orientation.NegZ:
				BuildZTunnle(x, y, z, m);
				FileWriter.WriteFile("BuildZTunnle(" + x + ", " + y + ", " + z + ", materials[" + materialNum + "]);");
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
						break;
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
				if(Keyboard.GetState().IsKeyDown(Key.Number5)) {
					materialNum = 4;
					Output.Good("Material: " + materialNum);
				}
				if(Keyboard.GetState().IsKeyDown(Key.Number6)) {
					materialNum = 5;
					Output.Good("Material: " + materialNum);
				}

			}

			if(player.Chunk == new Vector3(1, 0, 3)) {
				solved1 = true;
				eyeRot = 1;

				for(int i = 0; i < 8; i++) {
					nodes[i].levelTo = 1;
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
					BuildWallPosX(5, i, -j - 5, materials[0]);
					BuildWallNegX(-5, i, -j - 5, materials[0]);
					BuildWallPosZ(-i + 5, j, -5, materials[0]);
					BuildWallNegZ(-i + 5, j, -15, materials[0]);
				}
			}

			//black cock border
			DelChunk(-1, 0, -5);
			BuildWallNegY(-1, 0, -5, materials[1]);
			BuildWallPosZ(-1, 0, -5, materials[0]);
			DelChunk(-1, 0, -6);
			DelChunk(-1, 0, -7);
			DelChunk(-1, 0, -8);
			DelChunk(-1, 0, -9);
			DelChunk(-1, 0, -10);
			DelChunk(-1, 0, -11);
			DelChunk(-1, 0, -12);
			DelChunk(-1, 0, -13);
			DelChunk(-1, 0, -14);
			DelChunk(-1, 0, -15);
			DelChunk(1, 0, -15);
			DelChunk(1, 0, -14);
			DelChunk(1, 0, -13);
			DelChunk(1, 0, -12);
			DelChunk(1, 0, -11);
			DelChunk(1, 0, -10);
			DelChunk(1, 0, -9);
			DelChunk(1, 0, -8);
			DelChunk(1, 0, -7);
			DelChunk(1, 0, -6);
			DelChunk(1, 0, -5);
			BuildWallPosZ(1, 0, -5, materials[0]);
			BuildWallNegY(1, 0, -5, materials[1]);
			BuildWallNegY(1, 0, -6, materials[1]);
			BuildWallNegY(1, 0, -7, materials[1]);
			BuildWallNegY(1, 0, -8, materials[1]);
			BuildWallNegY(1, 0, -9, materials[1]);
			BuildWallNegY(1, 0, -10, materials[1]);
			BuildWallNegY(1, 0, -11, materials[1]);
			BuildWallNegY(1, 0, -12, materials[1]);
			BuildWallNegY(1, 0, -13, materials[1]);
			BuildWallNegY(1, 0, -14, materials[1]);
			BuildWallNegY(1, 0, -15, materials[1]);
			BuildWallNegZ(1, 0, -15, materials[1]);
			BuildWallNegZ(-1, 0, -15, materials[1]);
			BuildWallNegY(-1, 0, -15, materials[1]);
			BuildWallNegY(-1, 0, -14, materials[1]);
			BuildWallNegY(-1, 0, -13, materials[1]);
			BuildWallNegY(-1, 0, -12, materials[1]);
			BuildWallNegY(-1, 0, -11, materials[1]);
			BuildWallNegY(-1, 0, -10, materials[1]);
			BuildWallNegY(-1, 0, -9, materials[1]);
			BuildWallNegY(-1, 0, -8, materials[1]);
			BuildWallNegY(-1, 0, -7, materials[1]);
			BuildWallNegY(-1, 0, -6, materials[1]);
			DelChunk(1, 0, -5);
			DelChunk(1, 0, -5);
			BuildWallNegY(1, 0, -5, materials[1]);
			BuildWallPosZ(1, 0, -5, materials[1]);
			DelChunk(-1, 0, -5);
			DelChunk(-1, 1, -5);
			DelChunk(0, 1, -5);
			DelChunk(1, 1, -5);
			BuildWallPosZ(1, 1, -5, materials[1]);
			BuildWallPosZ(0, 1, -5, materials[1]);
			BuildWallPosZ(-1, 1, -5, materials[1]);
			BuildWallPosZ(-1, 0, -5, materials[1]);
			BuildWallNegY(-1, 0, -5, materials[1]);
			DelChunk(0, 1, -15);
			DelChunk(-1, 1, -15);
			DelChunk(0, 1, -15);
			DelChunk(1, 1, -15);
			BuildWallNegZ(-1, 1, -15, materials[1]);
			BuildWallNegZ(0, 1, -15, materials[1]);
			BuildWallNegZ(1, 1, -15, materials[1]);

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
			


			//wall left cubes
			BuildWallPosZ(-5, 10, -6, materials[3]);
			BuildWallNegX(-4, 10, -5, materials[3]);
			BuildWallPosY(-5, 9, -5, materials[3]);
			BuildWallNegZ(-5, 9, -6, materials[3]);
			BuildWallNegX(-4, 9, -7, materials[3]);
			BuildWallPosZ(-5, 9, -8, materials[3]);
			BuildWallNegY(-5, 10, -7, materials[3]);
			BuildWallPosY(-5, 8, -7, materials[3]);
			BuildWallNegZ(-5, 8, -8, materials[3]);
			BuildWallNegX(-4, 8, -9, materials[3]);
			BuildWallPosZ(-5, 8, -10, materials[3]);
			BuildWallNegY(-5, 9, -9, materials[3]);
			BuildWallPosY(-5, 7, -9, materials[3]);
			BuildWallNegZ(-5, 7, -10, materials[3]);
			BuildWallNegX(-4, 7, -11, materials[3]);
			BuildWallPosZ(-5, 7, -12, materials[3]);
			BuildWallPosY(-5, 6, -11, materials[3]);
			BuildWallNegY(-5, 8, -11, materials[3]);
			BuildWallNegY(-5, 8, -11, materials[3]);
			BuildWallNegZ(-5, 6, -12, materials[3]);
			BuildWallNegX(-4, 6, -13, materials[3]);
			BuildWallPosZ(-5, 6, -14, materials[3]);
			BuildWallNegY(-5, 7, -13, materials[3]);
			BuildWallPosY(-5, 5, -13, materials[3]);

			//first eye tunnle
			DelChunk(-5, 5, -13);
			BuildWallPosY(-5, 5, -13, materials[3]);
			BuildXTunnle(-7, 5, -13);
			BuildXTunnle(-6, 5, -13);
			BuildXTunnle(-8, 5, -13);
			BuildXTunnle(-9, 5, -13);
			BuildXTunnle(-10, 5, -13);

			BuildWallNegY(0, 0, 1, materials[0]);
			BuildWallNegY(0, 0, 2, materials[0]);
			BuildWallNegY(0, 0, 3, materials[0]);
			BuildWallNegY(1, 0, 3, materials[0]);
			BuildWallNegY(2, 0, 3, materials[0]);
			BuildWallNegY(-1, 0, 3, materials[0]);
			BuildWallNegY(-2, 0, 3, materials[0]);
			BuildWallNegY(-3, 0, 3, materials[0]);
			BuildWallNegY(3, 0, 3, materials[0]);
			BuildWallPosX(3, 0, 3, materials[0]);
			BuildWallNegZ(3, 0, 3, materials[0]);
			BuildWallPosY(3, 0, 3, materials[0]);
			BuildWallPosZ(3, 0, 3, materials[0]);
			BuildWallPosZ(2, 0, 3, materials[0]);
			BuildWallPosY(2, 0, 3, materials[0]);
			BuildWallNegZ(2, 0, 3, materials[0]);
			BuildWallNegZ(1, 0, 3, materials[0]);
			BuildWallPosZ(1, 0, 3, materials[0]);
			BuildWallPosZ(0, 0, 3, materials[0]);
			BuildWallPosZ(-1, 0, 3, materials[0]);
			BuildWallPosZ(-2, 0, 3, materials[0]);
			BuildWallPosZ(-3, 0, 3, materials[0]);
			BuildWallNegX(-3, 0, 3, materials[0]);
			BuildWallPosY(-3, 0, 3, materials[0]);

			BuildWallPosY(-2, 0, 3, materials[0]);
			BuildWallPosY(-1, 0, 3, materials[0]);
			BuildWallPosY(0, 0, 3, materials[0]);
			BuildWallPosY(1, 0, 3, materials[0]);
			BuildWallPosY(2, 0, 3, materials[0]);
			BuildWallNegX(0, 0, 1, materials[0]);
			BuildWallNegX(0, 0, 2, materials[0]);
			BuildWallPosX(0, 0, 1, materials[0]);
			BuildWallPosX(0, 0, 2, materials[0]);
			BuildWallPosY(0, 0, 2, materials[0]);
			BuildWallPosY(0, 0, 1, materials[0]);
			BuildWallNegZ(-3, 0, 3, materials[0]);
			BuildWallNegZ(-2, 0, 3, materials[0]);
			BuildWallNegZ(-1, 0, 3, materials[0]);
			BuildWallNegX(0, 0, 3, materials[0]);
			BuildWallPosX(0, 0, 3, materials[0]);
			BuildWallNegY(-11, 5, -13, materials[0]);
			BuildWallNegZ(-11, 5, -13, materials[0]);
			BuildWallNegX(-11, 5, -13, materials[0]);
			BuildWallPosY(-11, 5, -13, materials[0]);
			BuildZTunnle(-11, 5, -12);
			BuildZTunnle(-11, 5, -11);

			DelChunk(-4, 7, -13);
			DelChunk(-4, 6, -13);
			DelChunk(-5, 5, -13);
			DelChunk(-5, 6, -12);
			DelChunk(-5, 7, -13);
			DelChunk(-5, 7, -14);
			DelChunk(-5, 7, -14);
			DelChunk(-5, 6, -14);
			BuildWallNegX(-4, 7, -12, materials[3]);
			BuildWallNegX(-4, 7, -13, materials[3]);
			BuildWallNegX(-4, 7, -14, materials[3]);
			BuildWallPosZ(-5, 7, -15, materials[3]);
			BuildWallNegY(-5, 8, -14, materials[3]);
			BuildWallNegY(-5, 8, -13, materials[3]);
			BuildWallNegY(-5, 8, -12, materials[3]);
			DelChunk(-5, 7, -15);
			BuildWallNegX(-4, 7, -15, materials[3]);
			DelChunk(-5, 9, -15);
			BuildWallNegX(-5, 9, -15, materials[0]);
			BuildWallNegZ(-5, 9, -15, materials[0]);
			BuildWallNegY(-5, 8, -15, materials[3]);
			BuildWallPosY(-5, 6, -15, materials[3]);
			BuildWallPosY(-5, 6, -14, materials[3]);
			BuildWallPosY(-5, 6, -13, materials[3]);
			BuildWallPosY(-5, 6, -12, materials[3]);
			BuildWallNegX(-5, 6, -14, materials[0]);
			BuildWallNegX(-5, 6, -12, materials[0]);

			DelChunk(-11, 5, -13);
			BuildWallNegY(-11, 5, -13, materials[0]);
			BuildWallNegX(-11, 5, -13, materials[0]);
			BuildWallPosY(-11, 5, -13, materials[0]);
			BuildZTunnle(-11, 5, -14);
			BuildZTunnle(-11, 5, -15);
			BuildZTunnle(-11, 5, -16);
			BuildZTunnle(-11, 5, -17);
			BuildZTunnle(-11, 5, -18);
			BuildZTunnle(-11, 5, -10);
			BuildZTunnle(-11, 5, -9);
			BuildZTunnle(-11, 5, -8);
			BuildWallNegY(-11, 5, -7, materials[0]);
			BuildWallNegX(-11, 5, -7, materials[0]);
			BuildWallPosY(-11, 5, -7, materials[0]);
			BuildWallPosZ(-11, 5, -7, materials[0]);
			BuildXTunnle(-10, 5, -7);
			BuildXTunnle(-9, 5, -7);
			BuildWallNegY(-11, 5, -19, materials[0]);
			BuildWallPosY(-11, 5, -19, materials[0]);
			BuildWallNegZ(-11, 5, -19, materials[0]);
			BuildXTunnle(-10, 5, -19);
			BuildXTunnle(-12, 5, -19);
			BuildXTunnle(-13, 5, -19);
			DelChunk(-5, 6, -13);
			DelChunk(-5, 6, -12);
			DelChunk(-5, 5, -12);
			DelChunk(-5, 4, -12);
			DelChunk(-5, 4, -13);
			DelChunk(-5, 4, -14);
			DelChunk(-5, 5, -14);
			DelChunk(-5, 6, -14);
			DelChunk(-6, 5, -13);
			BuildWallNegX(-6, 4, -13, materials[1]);
			BuildWallNegX(-6, 4, -12, materials[1]);
			BuildWallNegX(-6, 5, -12, materials[1]);
			BuildWallNegX(-6, 6, -12, materials[1]);
			BuildWallNegX(-6, 6, -13, materials[1]);
			BuildWallNegX(-6, 6, -14, materials[1]);
			BuildWallNegX(-6, 5, -14, materials[1]);
			BuildWallNegX(-6, 4, -14, materials[1]);
			BuildWallNegY(-6, 4, -13, materials[1]);
			BuildWallNegY(-6, 4, -14, materials[1]);
			BuildWallNegY(-6, 4, -12, materials[1]);
			BuildWallPosZ(-6, 4, -12, materials[1]);
			BuildWallPosZ(-6, 5, -12, materials[1]);
			BuildWallPosZ(-6, 6, -12, materials[1]);
			BuildWallPosY(-6, 6, -12, materials[1]);
			BuildWallPosY(-6, 6, -13, materials[1]);
			BuildWallPosY(-6, 6, -14, materials[1]);
			BuildWallNegZ(-6, 6, -14, materials[1]);
			BuildWallNegZ(-6, 5, -14, materials[1]);
			BuildWallNegZ(-6, 4, -14, materials[1]);
			BuildWallPosY(-5, 6, -12, materials[3]);
			BuildWallPosY(-5, 6, -13, materials[3]);
			BuildWallPosY(-5, 6, -14, materials[3]);

			DelChunk( 5,  5,  -10);

			BuildXTunnle(5, 5, -10, materials[1]);
			BuildWallPosX(4, 5, -9, materials[1]);
			BuildWallPosX(4, 5, -11, materials[1]);
			BuildWallPosX(4, 6, -10, materials[1]);
			BuildWallPosX(4, 4, -10, materials[1]);
			BuildWallPosX(4, 4, -9, materials[1]);
			BuildWallPosX(4, 4, -11, materials[1]);
			BuildWallPosX(4, 6, -11, materials[1]);
			BuildWallPosX(4, 6, -9, materials[1]);
			BuildWallNegZ(5, 6, -8, materials[1]);
			BuildWallNegZ(5, 5, -8, materials[1]);
			BuildWallPosY(5, 3, -9, materials[1]);
			BuildWallNegZ(5, 4, -8, materials[1]);
			BuildWallPosY(5, 3, -10, materials[1]);
			BuildWallPosY(5, 3, -11, materials[1]);
			BuildWallPosZ(5, 4, -12, materials[1]);
			BuildWallPosZ(5, 5, -12, materials[1]);
			BuildWallPosZ(5, 6, -12, materials[1]);
			BuildWallNegY(5, 7, -11, materials[1]);
			BuildWallNegY(5, 7, -10, materials[1]);
			BuildWallNegY(5, 7, -9, materials[1]);
			BuildXTunnle(6, 5, -10, materials[0]);
			BuildXTunnle(7, 5, -10, materials[0]);
			BuildXTunnle(8, 5, -10, materials[0]);
			BuildXTunnle(9, 5, -10, materials[0]);
			BuildXTunnle(10, 5, -10, materials[0]);
			BuildXTunnle(11, 5, -10, materials[0]);
			BuildXTunnle(12, 5, -10, materials[0]);
			BuildXTunnle(13, 5, -10, materials[0]);

			BuildWallNegZ(-5, -8, -13, materials[0]);
			BuildWallPosX(-5, -8, -13, materials[0]);
			BuildWallPosZ(-5, -8, -13, materials[0]);
			BuildWallNegX(-5, -8, -13, materials[0]);
			BuildWallNegZ(-5, -9, -13, materials[0]);
			BuildWallNegZ(-5, -10, -13, materials[0]);
			BuildWallPosX(-5, -9, -13, materials[0]);
			BuildWallPosX(-5, -10, -13, materials[0]);
			BuildWallPosZ(-5, -10, -13, materials[0]);
			BuildWallPosZ(-5, -9, -13, materials[0]);
			BuildWallNegX(-5, -9, -13, materials[0]);
			BuildWallNegX(-5, -10, -13, materials[0]);
			BuildWallNegX(-5, -11, -13, materials[0]);
			BuildWallNegX(-5, -11, -13, materials[0]);
			BuildWallNegY(-5, -11, -13, materials[0]);
			BuildWallPosX(-5, -11, -13, materials[0]);
			BuildZTunnle(-5, -11, -12, materials[0]);
			BuildZTunnle(-5, -11, -11, materials[0]);
			BuildZTunnle(-5, -11, -14, materials[0]);
			BuildZTunnle(-5, -11, -15, materials[0]);
			BuildZTunnle(-5, -11, -10, materials[0]);
			BuildZTunnle(-5, -11, -16, materials[0]);

			BuildWallNegX(-5, -7, -13, materials[0]);
			BuildWallPosZ(-5, -7, -13, materials[0]);
			BuildWallPosX(-5, -7, -13, materials[0]);
			BuildWallNegZ(-5, -7, -13, materials[0]);
			BuildWallPosY(-5, -7, -13, materials[0]);
			BuildWallPosY(-5, -11, -13, materials[0]);

			BuildWallNegY(14, 5, -10, materials[0]);
			BuildWallNegZ(14, 5, -10, materials[0]);
			BuildWallNegZ(14, 5, -10, materials[0]);
			BuildWallPosY(14, 5, -10, materials[0]);
			BuildWallPosX(14, 5, -10, materials[0]);
			BuildZTunnle(14, 5, -9, materials[0]);
			BuildZTunnle(14, 5, -8, materials[0]);
			BuildZTunnle(14, 5, -7, materials[0]);

			BuildWallPosY(14, 5, -6, materials[0]);
			BuildWallNegX(14, 5, -6, materials[0]);
			BuildWallPosZ(14, 5, -6, materials[0]);
			BuildWallPosX(14, 5, -6, materials[0]);
			BuildWallPosZ(14, 4, -6, materials[0]);
			BuildWallNegX(14, 4, -6, materials[0]);
			BuildWallNegZ(14, 4, -6, materials[0]);
			BuildWallPosX(14, 4, -6, materials[0]);
			BuildWallPosX(14, 3, -6, materials[0]);
			BuildWallNegZ(14, 3, -6, materials[0]);
			BuildWallNegX(14, 3, -6, materials[0]);
			BuildWallPosZ(14, 3, -6, materials[0]);
			BuildWallPosX(14, 2, -6, materials[0]);
			BuildWallNegZ(14, 2, -6, materials[0]);
			BuildWallPosZ(14, 2, -6, materials[0]);
			BuildWallNegX(14, 2, -6, materials[0]);
			BuildWallNegY(-5, -11, -9, materials[0]);
			BuildWallPosX(-5, -11, -9, materials[0]);
			BuildWallPosY(-5, -11, -9, materials[0]);
			BuildWallPosZ(-5, -11, -9, materials[0]);
			BuildXTunnle(-6, -11, -9, materials[0]);
			BuildXTunnle(-8, -11, -9, materials[0]);
			BuildXTunnle(-7, -11, -9, materials[0]);


			//black cock bemes
			DelChunk(0, 5, -15);
			BuildWallNegZ(0, 5, -15, materials[1]);
			DelChunk(0, 5, -5);
			DelChunk(-5, 5, -10);
			BuildWallNegX(-5, 5, -10, materials[1]);
			BuildWallPosZ(0, 5, -5, materials[1]);
			DelChunk(0, 10, -10);
			BuildWallPosY(0, 10, -10, materials[1]);

			BuildWallNegX(1, 5, -5, materials[1]);
			BuildWallNegX(1, 5, -6, materials[1]);
			BuildWallNegX(1, 5, -7, materials[1]);
			BuildWallNegX(1, 5, -8, materials[1]);
			BuildWallNegX(1, 5, -9, materials[1]);
			BuildWallNegX(1, 5, -10, materials[1]);
			BuildWallPosZ(-5, 5, -11, materials[1]);
			BuildWallPosZ(-4, 5, -11, materials[1]);
			BuildWallPosZ(-3, 5, -11, materials[1]);
			BuildWallPosZ(-2, 5, -11, materials[1]);
			BuildWallPosZ(-1, 5, -11, materials[1]);
			BuildWallPosZ(0, 5, -11, materials[1]);
			BuildWallNegY(0, 6, -10, materials[1]);
			BuildWallNegY(0, 6, -9, materials[1]);
			BuildWallNegY(0, 6, -8, materials[1]);
			BuildWallNegY(0, 6, -7, materials[1]);
			BuildWallNegY(0, 6, -6, materials[1]);
			BuildWallNegY(0, 6, -5, materials[1]);
			BuildWallNegY(-1, 6, -10, materials[1]);
			BuildWallNegY(-2, 6, -10, materials[1]);
			BuildWallNegY(-3, 6, -10, materials[1]);
			BuildWallNegY(-4, 6, -10, materials[1]);
			BuildWallNegY(-5, 6, -10, materials[1]);
			BuildWallPosX(-1, 5, -9, materials[1]);
			BuildWallNegZ(-1, 5, -9, materials[1]);
			BuildWallNegZ(-2, 5, -9, materials[1]);
			BuildWallNegZ(-3, 5, -9, materials[1]);
			BuildWallNegZ(-4, 5, -9, materials[1]);
			BuildWallNegZ(-5, 5, -9, materials[1]);
			BuildWallPosX(-1, 5, -8, materials[1]);
			BuildWallPosX(-1, 5, -7, materials[1]);
			BuildWallPosX(-1, 5, -6, materials[1]);
			BuildWallPosX(-1, 5, -5, materials[1]);
			BuildWallPosY(0, 4, -5, materials[1]);
			BuildWallPosY(0, 4, -6, materials[1]);
			BuildWallPosY(0, 4, -7, materials[1]);
			BuildWallPosY(0, 4, -8, materials[1]);
			BuildWallPosY(0, 4, -9, materials[1]);
			BuildWallPosY(0, 4, -10, materials[1]);
			BuildWallPosY(-1, 4, -10, materials[1]);
			BuildWallPosY(-2, 4, -10, materials[1]);
			BuildWallPosY(-3, 4, -10, materials[1]);
			BuildWallPosY(-4, 4, -10, materials[1]);
			BuildWallPosY(-5, 4, -10, materials[1]);
			BuildWallNegX(1, 10, -10, materials[1]);
			BuildWallNegX(1, 9, -10, materials[1]);
			BuildWallNegX(1, 8, -10, materials[1]);
			BuildWallNegX(1, 7, -10, materials[1]);
			BuildWallNegX(1, 6, -10, materials[1]);
			BuildWallPosZ(0, 6, -11, materials[1]);
			BuildWallPosZ(0, 7, -11, materials[1]);
			BuildWallPosZ(0, 8, -11, materials[1]);
			BuildWallPosZ(0, 9, -11, materials[1]);
			BuildWallPosZ(0, 10, -11, materials[1]);
			BuildWallPosX(-1, 10, -10, materials[1]);
			BuildWallPosX(-1, 9, -10, materials[1]);
			BuildWallPosX(-1, 8, -10, materials[1]);
			BuildWallPosX(-1, 7, -10, materials[1]);
			BuildWallPosX(-1, 6, -10, materials[1]);
			BuildWallNegZ(0, 6, -9, materials[1]);
			BuildWallNegZ(0, 7, -9, materials[1]);
			BuildWallNegZ(0, 8, -9, materials[1]);
			BuildWallNegZ(0, 9, -9, materials[1]);
			BuildWallNegZ(0, 10, -9, materials[1]);

			DelChunk(1, 10, -10);
			DelChunk(1, 10, -10);
			BuildWallNegX(1, 10, -10, materials[1]);
			BuildWallNegX(1, 11, -10, materials[1]);
			BuildWallPosZ(1, 11, -10, materials[1]);
			BuildWallPosX(1, 11, -10, materials[1]);
			BuildWallNegZ(1, 11, -10, materials[1]);
			BuildWallPosX(1, 12, -10, materials[0]);
			BuildWallNegZ(1, 12, -10, materials[0]);
			BuildWallNegX(1, 12, -10, materials[0]);
			BuildWallPosZ(1, 12, -10, materials[0]);
			DelChunk(1, 5, -5);
			BuildWallNegX(1, 5, -5, materials[1]);
			BuildWallNegY(1, 5, -4, materials[1]);
			BuildWallNegX(1, 5, -4, materials[1]);
			BuildWallPosY(1, 5, -4, materials[1]);
			BuildWallPosX(1, 5, -4, materials[1]);
			BuildWallNegY(1, 5, -3, materials[0]);
			BuildWallPosX(1, 5, -3, materials[0]);
			BuildWallPosY(1, 5, -3, materials[0]);
			BuildWallNegX(1, 5, -3, materials[0]);

			BuildWallNegZ(1, 13, -10, materials[0]);
			BuildWallPosX(1, 13, -10, materials[0]);
			BuildWallPosZ(1, 13, -10, materials[0]);
			BuildWallNegX(1, 13, -10, materials[0]);
			BuildWallNegZ(1, 14, -10, materials[0]);
			BuildWallPosX(1, 14, -10, materials[0]);
			BuildWallPosZ(1, 14, -10, materials[0]);
			BuildWallNegX(1, 14, -10, materials[0]);
			BuildWallNegZ(1, 15, -10, materials[0]);
			BuildWallNegZ(1, 15, -10, materials[0]);
			BuildWallNegX(1, 15, -10, materials[0]);
			BuildWallPosZ(1, 15, -10, materials[0]);
			BuildWallPosY(1, 15, -10, materials[0]);
			BuildWallNegZ(2, 15, -10, materials[0]);
			BuildWallPosY(2, 15, -10, materials[0]);
			BuildWallNegY(2, 15, -10, materials[0]);
			BuildWallPosZ(2, 15, -10, materials[0]);
			BuildXTunnle(3, 15, -10, materials[0]);
			BuildXTunnle(4, 15, -10, materials[0]);

			DelChunk(1, 15, -10);
			BuildWallPosY(1, 15, -10, materials[0]);
			BuildWallPosZ(1, 15, -10, materials[0]);
			BuildWallNegX(1, 15, -10, materials[0]);
			BuildZTunnle(1, 15, -11, materials[0]);
			BuildZTunnle(1, 15, -12, materials[0]);
			BuildWallNegX(1, 15, -13, materials[0]);
			BuildWallPosY(1, 15, -13, materials[0]);
			BuildWallNegZ(1, 15, -13, materials[0]);
			BuildWallPosX(1, 15, -13, materials[0]);
			BuildWallNegX(1, 14, -13, materials[0]);
			BuildWallNegZ(1, 14, -13, materials[0]);
			BuildWallPosX(1, 14, -13, materials[0]);
			BuildWallPosZ(1, 14, -13, materials[0]);
			BuildWallPosZ(1, 13, -13, materials[0]);
			BuildWallNegX(1, 13, -13, materials[0]);
			BuildWallNegZ(1, 13, -13, materials[0]);
			BuildWallPosX(1, 13, -13, materials[0]);

			BuildWallNegY(5, 15, -10, materials[0]);
			BuildWallPosZ(5, 15, -10, materials[0]);
			BuildWallPosX(5, 15, -10, materials[0]);
			BuildWallPosY(5, 15, -10, materials[0]);
			BuildZTunnle(5, 15, -11, materials[0]);
			BuildZTunnle(5, 15, -12, materials[0]);
			//
			BuildWallNegY(-12, 5, -13, materials[0]);
			BuildWallPosZ(-12, 5, -13, materials[0]);
			BuildWallPosY(-12, 5, -13, materials[0]);
			BuildWallNegZ(-12, 5, -13, materials[0]);
			BuildWallNegY(-13, 5, -13, materials[0]);
			BuildWallNegZ(-13, 5, -13, materials[0]);
			BuildWallPosZ(-13, 5, -13, materials[0]);
			BuildWallPosY(-13, 5, -13, materials[0]);
			BuildWallNegY(-14, 5, -13, materials[0]);
			BuildWallPosZ(-14, 5, -13, materials[0]);
			BuildWallNegX(-14, 5, -13, materials[0]);
			BuildWallPosY(-14, 5, -13, materials[0]);
			BuildZTunnle(-14, 5, -14, materials[0]);
			BuildZTunnle(-14, 5, -15, materials[0]);
			BuildZTunnle(-14, 5, -16, materials[0]);
			BuildZTunnle(-14, 5, -17, materials[0]);
			BuildWallNegZ(-14, 5, -17, materials[0]);
			BuildZTunnle(1, 5, -2, materials[0]);

			BuildZTunnle(1, 5, -1, materials[0]);
			BuildWallNegX(1, 5, 0, materials[0]);
			BuildWallPosY(1, 5, 0, materials[0]);
			BuildWallPosX(1, 5, 0, materials[0]);
			BuildWallPosZ(1, 5, 0, materials[0]);
			BuildWallPosZ(1, 4, 0, materials[0]);
			BuildWallNegX(1, 4, 0, materials[0]);
			BuildWallNegZ(1, 4, 0, materials[0]);
			BuildWallPosX(1, 4, 0, materials[0]);

			BuildXTunnle(-14, 1, -10, materials[0]);
			BuildWallNegY(-15, 1, -10, materials[0]);
			BuildWallPosZ(-15, 1, -10, materials[0]);
			BuildWallNegX(-15, 1, -10, materials[0]);
			BuildWallPosY(-15, 1, -10, materials[0]);
			BuildZTunnle(-15, 1, -11, materials[0]);
			BuildZTunnle(-15, 1, -12, materials[0]);
			BuildZTunnle(-15, 1, -13, materials[0]);
			BuildZTunnle(-15, 1, -14, materials[0]);
			BuildZTunnle(-15, 1, -15, materials[0]);
			BuildZTunnle(-15, 1, -16, materials[0]);
			BuildZTunnle(-15, 1, -17, materials[0]);
			BuildXTunnle(-13, 1, -10, materials[0]);
			BuildXTunnle(-12, 1, -10, materials[0]);
			BuildXTunnle(-11, 1, -10, materials[0]);
			BuildXTunnle(-10, 1, -10, materials[0]);
			BuildXTunnle(-9, 1, -10, materials[0]);
			BuildXTunnle(-8, 1, -10, materials[0]);
			BuildWallNegY(-15, 1, -18, materials[0]);
			BuildWallNegX(-15, 1, -18, materials[0]);
			BuildWallNegZ(-15, 1, -18, materials[0]);
			BuildWallPosY(-15, 1, -18, materials[0]);
			BuildWallNegY(-7, 1, -10, materials[0]);
			BuildWallPosZ(-7, 1, -10, materials[0]);
			BuildWallPosX(-7, 1, -10, materials[0]);
			BuildWallPosY(-7, 1, -10, materials[0]);
			BuildZTunnle(-7, 1, -11, materials[0]);
			BuildZTunnle(-7, 1, -12, materials[0]);
			BuildZTunnle(-7, 1, -13, materials[0]);
			BuildZTunnle(-7, 1, -14, materials[0]);
			BuildZTunnle(-7, 1, -15, materials[0]);
			BuildZTunnle(-7, 1, -16, materials[0]);
			BuildZTunnle(-7, 1, -17, materials[0]);
			BuildWallNegY(-7, 1, -18, materials[0]);
			BuildWallPosX(-7, 1, -18, materials[0]);
			BuildWallNegZ(-7, 1, -18, materials[0]);
			BuildWallPosY(-7, 1, -18, materials[0]);
			BuildXTunnle(-8, 1, -18, materials[0]);
			BuildXTunnle(-9, 1, -18, materials[0]);
			BuildXTunnle(-10, 1, -18, materials[0]);
			BuildXTunnle(-12, 1, -18, materials[0]);
			BuildXTunnle(-11, 1, -18, materials[0]);
			BuildXTunnle(-13, 1, -18, materials[0]);
			BuildXTunnle(-14, 1, -18, materials[0]);
			DelChunk(-15, 1, -10);
			BuildWallNegX(-15, 1, -10, materials[0]);
			BuildWallNegY(-15, 1, -10, materials[0]);
			BuildWallPosZ(-15, 1, -10, materials[0]);
			BuildWallPosX(-15, 2, -10, materials[0]);
			BuildWallPosX(-15, 3, -10, materials[0]);
			BuildWallPosX(-15, 4, -10, materials[0]);
			BuildWallPosX(-15, 5, -10, materials[0]);
			BuildWallPosY(-15, 5, -10, materials[0]);
			BuildWallPosZ(-15, 5, -10, materials[0]);
			BuildWallPosZ(-15, 4, -10, materials[0]);
			BuildWallPosZ(-15, 3, -10, materials[0]);
			BuildWallPosZ(-15, 2, -10, materials[0]);
			BuildWallNegX(-15, 2, -10, materials[0]);
			BuildWallNegX(-15, 3, -10, materials[0]);
			BuildWallNegX(-15, 4, -10, materials[0]);
			BuildWallNegX(-15, 5, -10, materials[0]);
			BuildWallNegZ(-15, 2, -10, materials[0]);
			BuildWallNegZ(-15, 3, -10, materials[0]);
			BuildWallNegZ(-15, 4, -10, materials[0]);
			BuildZTunnle(-15, 6, -11, materials[0]);
			BuildZTunnle(-15, 5, -11, materials[0]);
			BuildWallNegY(-12, 1, -13, materials[0]);
			BuildWallPosZ(-12, 1, -13, materials[0]);
			BuildWallPosZ(-12, 1, -13, materials[0]);
			BuildWallPosY(-12, 1, -13, materials[0]);
			BuildWallPosX(-12, 1, -13, materials[0]);
			BuildZTunnle(-12, 1, -14, materials[0]);
			BuildWallNegX(1, 12, -13, materials[0]);
			BuildWallPosZ(1, 12, -13, materials[0]);
			BuildWallPosX(1, 12, -13, materials[0]);
			BuildWallNegY(1, 12, -13, materials[0]);
			BuildZTunnle(1, 12, -14, materials[0]);
			DelChunk(1, 5, -10);
			BuildWallNegX(1, 5, -10, materials[4]);


			//rot180
			nodes.Add(new Node(0, 0, -1, -22, Orientation.NegZ, 2, 0, -1, -22, Orientation.NegZ));
			nodes.Add(new Node(0, 1, -1, -22, Orientation.NegZ, 2, 1, -1, -22, Orientation.NegZ));
			nodes.Add(new Node(0, -1, -1, -22, Orientation.NegZ, 2, -1, -1, -22, Orientation.NegZ));
			nodes.Add(new Node(0, -1, -1, -23, Orientation.NegZ, 2, -1, -1, -23, Orientation.NegZ));
			nodes.Add(new Node(0, 0, -1, -23, Orientation.NegZ, 2, 0, -1, -23, Orientation.NegZ));
			nodes.Add(new Node(0, 1, -1, -23, Orientation.NegZ, 2, 1, -1, -23, Orientation.NegZ));
			nodes.Add(new Node(0, 1, -1, -21, Orientation.NegZ, 2, 1, -1, -21, Orientation.NegZ));
			nodes.Add(new Node(0, 0, -1, -21, Orientation.PosZ, 2, 0, -1, -21, Orientation.PosZ));
			nodes.Add(new Node(0, 0, -1, -22, Orientation.PosZ, 2, 0, -1, -22, Orientation.PosZ));

			////eye1
			nodes.Add(new Node(2, 11, -5, -15, Orientation.NegZ, 2, 7, -5, -13, Orientation.PosX));
			nodes.Add(new Node(2, 11, -5, -8, Orientation.PosZ, 0, 1, 0, 3, Orientation.NegX));
			nodes.Add(new Node(2, 11, -5, -15, Orientation.PosX, 2, 8, -5, -13, Orientation.PosZ));
			nodes.Add(new Node(2, 11, -5, -15, Orientation.NegX, 2, 8, -5, -13, Orientation.NegZ));
			nodes.Add(new Node(2, 11, -5, -15, Orientation.PosZ, 2, 8, -5, -13, Orientation.PosX));

			////fall1
			nodes.Add(new Node(1, -5, -8, -13, Orientation.PosZ, 0, -5, -8, -13, Orientation.PosZ));
			nodes.Add(new Node(1, -5, -8, -13, Orientation.PosX, 0, -5, -8, -13, Orientation.PosX));
			nodes.Add(new Node(1, -5, -8, -13, Orientation.NegZ, 0, -5, -8, -13, Orientation.NegZ));
			nodes.Add(new Node(1, -5, -8, -13, Orientation.NegX, 0, -5, -8, -13, Orientation.NegX));

			//fall2
			nodes.Add(new Node(0, -5, -11, -10, Orientation.PosZ, 1, -4, 14, -6, Orientation.NegX));

			//early fall
			nodes.Add(new Node(2, -4, -15, -10, Orientation.NegX, 2, 14, -5, -14, Orientation.PosZ));

			//1
			nodes.Add(new Node(1, -14, 1, -10, Orientation.NegX, 0, -14, 1, -10, Orientation.NegX));
			//2
			nodes.Add(new Node(0, -15, 1, -17, Orientation.NegZ, 1, -4, 1, 0, Orientation.NegX));
			//3
			nodes.Add(new Node(1,-5,1,-1,Orientation.PosZ,0,-14,1,-18,Orientation.NegX));
			//4
			nodes.Add(new Node(0, -15, 1, -11, Orientation.PosZ, 1, -15, 1, -11, Orientation.PosZ));

			nodes.Add(new Node(1, -14, 1, -10, Orientation.NegZ, 0, -14, 1, -10, Orientation.NegZ));
			nodes.Add(new Node(1, -14, 1, -10, Orientation.PosZ, 0, -14, 1, -10, Orientation.PosZ));
			nodes.Add(new Node(1, -5, 1, -1, Orientation.NegX, 0, -14, 1, -18, Orientation.NegZ));
			nodes.Add(new Node(1, -5, 1, -1, Orientation.PosX, 0, -14, 1, -18, Orientation.PosZ));

			//solve2
			nodes.Add(new Node(1, -13, 1, -13, Orientation.PosX, 0, -1, 0, 3, Orientation.PosX));


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
