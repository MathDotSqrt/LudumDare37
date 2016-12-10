using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.AudioEngine;
using MathDotSqrt.Sqrt3D.RenderEngine;
using OpenTK.Input;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.World.Objects.Cameras;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.Util.IO;

namespace MathDotSqrt.Sqrt3D.GameState.GameStates {
	public class TestState : GameState {
		private Scene scene;
		private GuiField gui;

		public TestState(GameStateManager gsm) : base(gsm) {

		}

		public override void Init() {
			scene = new Scene();

			Camera camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 10000);
			camera.Position.Z = 5;
			scene.Add(camera);

			Geometry cubeGeometry = OBJLoader.LoadOBJ("cube");
			Material skyMaterial = new MeshSkyboxMaterial() {
				SkyboxCubeMap = TextureLoader.LoadCubeMap("clouds")
			};
			Mesh skybox = new Mesh(cubeGeometry, skyMaterial);
			skybox.SetScale(9000);
			scene.Add(skybox);

			Geometry geometry = OBJLoader.LoadOBJ("level");
			Material material = new MeshLitMaterial() {
				CubeMapReflection = TextureLoader.LoadCubeMap("clouds"),
				RenderFace = RenderFace.FrontAndBack
			};
			Mesh mesh = new Mesh(geometry, material);
			//mesh.RotateY(45);
			scene.Add(mesh);

			Light point = new PointLight(Color.White, .5f);
			camera.Add(point);
			scene.Add(point);

			Light rad = new RadialLightPoint(Color.White, .5f);
			rad.SetPosition(-1377, 1410, 4468);
			scene.Add(rad);

			gui = new GuiField();
		}

		public override void Update(float delta) {
			KeyboardState k = Keyboard.GetState();
			if(k.IsKeyDown(Key.Up))
				scene.Camera.MoveForward(5f);
			if(k.IsKeyDown(Key.Down))
				scene.Camera.MoveForward(-5f);
			if(k.IsKeyDown(Key.Left))
				scene.Camera.Rotation.Y -= 4;
			if(k.IsKeyDown(Key.Right))
				scene.Camera.Rotation.Y += 4;
			if(k.IsKeyDown(Key.D))
				scene.Camera.MoveLeft(5);
			if(k.IsKeyDown(Key.A))
				scene.Camera.MoveLeft(-5);

			if(k.IsKeyDown(Key.Space))
				scene.Camera.Position.Y += 5;
			if(k.IsKeyDown(Key.ShiftLeft))
				scene.Camera.Position.Y -= 5;

			if(k.IsKeyDown(Key.P))
				Output.Good(scene.Camera.Position);
		}
		public override void Render(OpenGLRenderer renderer) {
			renderer.RenderScene(scene);
			renderer.RenderGui(gui);
		}
		public override void PlayAudio(OpenALPlayer player) {
			player.PlaySceneAudio(scene);
		}
		public override void UpdateResize() {
			gui.UpdateScreenSize();
		}

		public override void Dispose() {

		}
	}
}
