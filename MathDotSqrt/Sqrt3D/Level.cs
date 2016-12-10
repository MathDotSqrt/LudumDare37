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
		private Player player;

		private Node node;

		public Level(Scene scene) {
			this.scene = scene;
			planeGeometry = OBJLoader.LoadOBJ("plane");
			nullPlaneMaterial = new MeshSpecularMaterial();
			eyeMaterial = new MeshSpecularMaterial() {
				Texture = TextureLoader.LoadModelTexture("tim.jpg")
			};

			BuildBox(0, 0, 0, null, Orientation.NegZ);
			BuildWallNegZ(0, 0, 0, eyeMaterial);

			node = new Node(0, 0, 0, 0, 0, Orientation.NegZ);



			/////////////////////////////////////////////////////////////

			player = new Player(5, 5, 5);
			Light point = new PointLight(Color.White, 1);
			player.camera.Add(point);
			scene.Add(player.camera);
			scene.Add(point);
		}

		public void Update() {
			player.Update();
			Output.Good(node.IsLooking(player));
		}


		public void BuildWallNegX(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = -90;
			scene.Add(mesh);
		}
		public void BuildWallPosX(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10, y * 10 + 5, z * 10 + 5);
			mesh.Rotation.Y = 90;
			scene.Add(mesh);
		}
		public void BuildWallPosY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10, z * 10 + 5);
			mesh.Rotation.X = -90;
			scene.Add(mesh);
		}
		public void BuildWallNegY(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 10, z * 10 + 5);
			mesh.Rotation.X = 90;
			scene.Add(mesh);
		}
		public void BuildWallPosZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10);
			scene.Add(mesh);
		}
		public void BuildWallNegZ(float x, float y, float z, Material m = null) {
			Material material = ( m == null ) ? nullPlaneMaterial : m;

			Mesh mesh = new Mesh(planeGeometry, material);
			mesh.SetPosition(x * 10 + 5, y * 10 + 5, z * 10 + 10);
			mesh.Rotation.Y = 180;
			scene.Add(mesh);
		}

		public void BuildBox(float x, float y, float z, Material m = null, Orientation omit = Orientation.None) {
			if(omit != Orientation.NegX)
				BuildWallNegX(x, y, z, m);
			if(omit != Orientation.NegY)
				BuildWallNegY(x, y, z, m);
			if(omit != Orientation.NegZ)
				BuildWallNegZ(x, y, z, m);
			if(omit != Orientation.PosX)
				BuildWallPosX(x, y, z, m);
			if(omit != Orientation.PosY)
				BuildWallPosY(x, y, z, m);
			if(omit != Orientation.PosZ)
				BuildWallPosZ(x, y, z, m);
		}
	
	}
}
