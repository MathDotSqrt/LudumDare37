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
		private Node node;

		public List<Wall> walls;
		public List<Node> nodes;

		public Level(Scene scene, Player player) {
			this.scene = scene;
			this.player = player;
			walls = new List<Wall>();
			nodes = new List<Node>();

			planeGeometry = OBJBumpLoader.LoadOBJ("plane");

			nullPlaneMaterial = new MeshSpecularMaterial() {
				Texture = TextureLoader.LoadModelTexture("wall1.png", true),
				//NormalMap = TextureLoader.LoadModelTexture("wall1_normal.png", true),
			};
			floorMaterial = new MeshBumpMaterial() {
				Texture = TextureLoader.LoadModelTexture("floor1.png", true),
				NormalMap = TextureLoader.LoadModelTexture("floor_normal.png", true),
				Shininess = 10
			};
			eyeMaterial = new MeshSpecularMaterial() {
				Texture = TextureLoader.LoadModelTexture("eye.jpg")
			};

			BuildZTunnle(0,0,0);
			BuildWallPosZ(0,0,0, eyeMaterial);
			BuildWallNegZ(0,0,0);

			BuildZTunnle(0, 1, 0);
			BuildWallPosZ(0, 1, 0, eyeMaterial);
			for(int i = 0; i < 10; i++) {
				BuildZTunnle(0,1,-i);
			}
			//BuildWallNegZ(0, 0, 0);

			nodes.Add(new Node(0,0,0, 0, 1, 0, Orientation.PosZ));
			//stop typing
		} 

		public void Update() {


			foreach(Node node in nodes) {
				if(node.IsLooking(player)) {
					player.Teleport(node);
				}
			}
		}


		public void BuildWallPosX(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = -90;
			scene.Add(mesh);

			float centerX = mesh.Position.X;
			float centerY = mesh.Position.Y;
			float centerZ = mesh.Position.Z;
			walls.Add(new Wall(centerX, centerY - 5, centerZ - 5, centerX, centerY + 5, centerZ + 5, Orientation.NegX));

		}
		public void BuildWallNegX(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = 90;
			scene.Add(mesh);
			float centerX = mesh.Position.X;
			float centerY = mesh.Position.Y;
			float centerZ = mesh.Position.Z;
			walls.Add(new Wall(centerX, centerY - 5, centerZ - 5, centerX, centerY + 5, centerZ + 5, Orientation.PosX));

		}
		public void BuildWallNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10, z * 10 + 5);
			mesh.Rotation.X = -90;
			scene.Add(mesh);

			float centerX = mesh.Position.X;
			float centerY = mesh.Position.Y;
			float centerZ = mesh.Position.Z;
			walls.Add(new Wall(centerX - 5, centerY, centerZ - 5, centerX + 5, centerY, centerZ + 5, Orientation.PosY));

		}
		public void BuildWallPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 10, z * 10 + 5);
			mesh.Rotation.X = 90;
			scene.Add(mesh);

			float centerX = mesh.Position.X;
			float centerY = mesh.Position.Y;
			float centerZ = mesh.Position.Z;
			walls.Add(new Wall(centerX - 5, centerY, centerZ - 5, centerX + 5, centerY, centerZ + 5, Orientation.NegY));

		}
		public void BuildWallNegZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10);
			scene.Add(mesh);

			float centerX = mesh.Position.X;
			float centerY = mesh.Position.Y;
			float centerZ = mesh.Position.Z;
			walls.Add(new Wall(centerX - 5, centerY - 5, centerZ, centerX + 5, centerY + 5, centerZ, Orientation.PosZ));

		}
		public void BuildWallPosZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10 + 10);
			mesh.Rotation.Y = 180;
			scene.Add(mesh);

			float centerX = mesh.Position.X;
			float centerY = mesh.Position.Y;
			float centerZ = mesh.Position.Z;
			walls.Add(new Wall(centerX - 5, centerY - 5, centerZ, centerX + 5, centerY + 5, centerZ, Orientation.NegZ));

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
	}

	public class Wall{
		public float x1, y1, z1, x2, y2, z2;
		public Orientation O;

		public Wall(float x1, float y1, float z1, float x2, float y2, float z2, Orientation O) {
			this.x1 = x1;
			this.y1 = y1;
			this.z1 = z1;
			this.x2 = x2;
			this.y2 = y2;
			this.z2 = z2;

			this.O = O;
		}
	}
}
