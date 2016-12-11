using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D {
	public class Level {
		private Scene scene;
		private Geometry planeGeometry;
		private Material nullPlaneMaterial;
		private Material eyeMaterial;
		private Material floorMaterial;

		private Player player;

		public int currentLevel = 0;
		public List<List<Wall>> walls;
		public List<List<Node>> nodes;
		public List<List<Mesh>> meshes;

		public Level(Scene scene, Player player) {
			this.scene = scene;
			this.player = player;
			walls = new List<List<Wall>>();
			nodes = new List<List<Node>>();
			meshes = new List<List<Mesh>>();

			for(int i = 0; i < 6; i++) {
				walls.Add(new List<Wall>());
				nodes.Add(new List<Node>());
				meshes.Add(new List<Mesh>());
			}
			planeGeometry = OBJBumpLoader.LoadOBJ("plane");

			nullPlaneMaterial = new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("wall1.png", true),
				NormalMap = TextureLoader.LoadModelTexture("wall1_normal.png", true),
			};
			floorMaterial = new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("floor1.png", true),
				NormalMap = TextureLoader.LoadModelTexture("floor_normal.png", true),
				Shininess = 10
			};
			eyeMaterial = new MeshSpecularMaterial() {
				Texture = TextureLoader.LoadModelTexture("eye.jpg")
			};

			ChangeLevel(0);

			for(int i = 0; i < 11; i++) {
				for(int j = 0; j < 11; j++) {
					BuildWallNegY(i, 0, j);
					BuildWallPosY(i, 10, j);
					BuildWallPosX(10, i, j);
					BuildWallNegX(0, i, j );
					BuildWallPosZ(i, j, 10);
					BuildWallNegZ(i, j, 0);
				}
			}

			BuildCube(2, 0, 2);
			BuildCube(3, 1, 2);
			BuildCube(4, 2, 2);
			Center(0, -5,-5,-5);
			RotZ90(0, 1);
			RotZ90(1, 2);
			RotZ90(2, 3);
			ChangeLevel(0);
			//BuildWallNegY(0,0,0);
			//stop typing
		}

		

		public void ChangeLevel(int level) {
			level %= 6;
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
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = -90;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(-1, 0, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.X));
		}
		public void BuildWallNegX(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = 90;
			scene.Add(mesh);
			
			walls[currentLevel].Add(new Wall(new Vector3(1, 0, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.X));

		}
		public void BuildWallNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10, z * 10 + 5);
			mesh.Rotation.X = -90;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0, 1, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.Y));
		}
		public void BuildWallPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 10, z * 10 + 5);
			mesh.Rotation.X = 90;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0, -1, 0), mesh.Position, new Vector3(x, y, z), mesh.Position.Y));

		}
		public void BuildWallNegZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10);
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0,0,1), mesh.Position, new Vector3(x, y, z), mesh.Position.Z));
		}
		public void BuildWallPosZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10 + 10);
			mesh.Rotation.Y = 180;
			scene.Add(mesh);

			walls[currentLevel].Add(new Wall(new Vector3(0, 0, -1), mesh.Position, new Vector3(x, y, z), mesh.Position.Z));
		}

		public void BuildXWalls(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			BuildWallPosX(x, y, z, m);
			BuildWallNegX(x, y, z, m);
		}
		public void BuildYWalls(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			BuildWallPosY(x, y, z, m);
			BuildWallNegY(x, y, z, m);
		}
		public void BuildZWalls(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			BuildWallPosZ(x, y, z, m);
			BuildWallNegZ(x, y, z, m);
		}
		public void BuildXTunnle(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			BuildYWalls(x, y, z, m);
			BuildZWalls(x, y, z, m);
		}
		public void BuildYTunnle(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			BuildXWalls(x, y, z, m);
			BuildZWalls(x, y, z, m);
		}
		public void BuildZTunnle(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			BuildYWalls(x, y, z, m);
			BuildXWalls(x, y, z, m);
		}

		public void BuildCube(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;
			BuildWallPosX(x - 1, y, z);
			BuildWallNegX(x + 1, y, z);
			BuildWallPosZ(x, y, z - 1);
			BuildWallNegZ(x, y, z + 1);
			BuildWallPosY(x, y - 1, z);
			BuildWallNegY(x, y + 1, z);
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
			foreach(Node node in nodes[currentLevel]) {
				if(node.IsLooking(player)) {
					player.Teleport(node);
				}
			}
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
