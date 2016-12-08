using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Cameras;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts;
using OpenTK.Audio.OpenAL;
using MathDotSqrt.Sqrt3D.AudioEngine;

namespace MathDotSqrt.Sqrt3D.GameState.GameStates {
	public class PlayState : GameState {

		private Scene scene;
		private GuiField gui;

		public PlayState(GameStateManager gsm) : base(gsm) {

		}

		public override void Init() {
			scene = new Scene();
			PerspectiveCamera camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 10000);
			camera.Position.Z = 10;
			scene.Add(camera);

			Geometry cubeGeometry = OBJLoader.LoadOBJ("cube.obj");
			Material skyMaterial = new MeshSkyboxMaterial() {
				SkyboxCubeMap = TextureLoader.LoadCubeMap("lagoon", true)
			};
			Mesh skyBox = new Mesh(cubeGeometry, skyMaterial);
			skyBox.SetScale(9000);
			scene.Add(skyBox);

			Geometry levelGeometry = OBJLoader.LoadOBJ("level.obj");
			Material levelMaterial = new MeshSpecularMaterial() {
				CubeMapReflection = TextureLoader.LoadCubeMap("lagoon", true),
			};
			Mesh mesh = new Mesh(levelGeometry, levelMaterial);
			scene.Add(mesh);

			Light ambientLight = new AmbientLight(Color.Silver, 1f);
			scene.Add(ambientLight);
			Light pointLight = new PointLight(Color.Silver, 1f);
			camera.Add(pointLight);
			scene.Add(pointLight);


			//--GUI--//
			gui = new GuiField();
		}

		float count = 0;

		public override void Update(float delta) {
			//scene.Camera.Position.Z = ( (float)Math.Sin(count) * 1000 ) + 500;

			//count += .005f;

			

			if(Keyboard.GetState().IsKeyDown(Key.D))
				scene.Camera.RotateY(1.5f);
			if(Keyboard.GetState().IsKeyDown(Key.A))
				scene.Camera.RotateY(-1.5f);
			if(Keyboard.GetState().IsKeyDown(Key.Escape))
				Environment.Exit(0);

			if(Keyboard.GetState().IsKeyDown(Key.W)) {
				Output.Good(scene.Camera.Rotation.Y % 360);
				float rad = SexyMathHelper.ToRad(scene.Camera.Rotation.Y);
				scene.Camera.Position.X += (float)Math.Sin(rad);
				scene.Camera.Position.Z += (float)-Math.Cos(rad);
			}
		}
		public override void Render(OpenGLRenderer renderer) {
			renderer.RenderScene(scene);
			renderer.RenderGui(gui);
		}
		public override void PlayAudio(OpenALPlayer player) {

		}
		public override void UpdateResize() {
			PerspectiveCamera camera = (PerspectiveCamera)scene.GetCamera();
			camera.AspectRatio = Window.ASPECT_RATIO;
			camera.UpdateProjectionMatrix();

			gui.UpdateScreenSize();
		}

		public override void Dispose() {
			
		}
	}
}
