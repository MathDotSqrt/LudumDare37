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

		Scene scene;
		GuiField gui;
		Level level;

		public PlayState(GameStateManager gsm) : base(gsm) {
			
		}

		public override void Init() {
			scene = new Scene();

			level = new Level(scene);

			Geometry cubeGeometry = OBJLoader.LoadOBJ("cube");
			Material material = new MeshBasicMaterial() {
				Color = Color.Yellow
			};
			Mesh mesh = new Mesh(cubeGeometry, material);
			scene.Add(mesh);

			gui = new GuiField();
		}

		public override void Update(float delta) {
			Input.UpdateCamera(scene.Camera);

			level.Update();

			if (Keyboard.GetState().IsKeyDown(Key.M)) {
				gsm.EnterGameState(GameStateManager.MENU_STATE);
			}
		}
		public override void Render(OpenGLRenderer renderer) {
			renderer.RenderScene(scene);
		}
		public override void PlayAudio(OpenALPlayer player) {

		}
		public override void UpdateResize() {
			gui.UpdateScreenSize();
			PerspectiveCamera camera = (PerspectiveCamera)scene.Camera;
			camera.AspectRatio = Window.ASPECT_RATIO;
			camera.UpdateProjectionMatrix();
		}

		public override void Dispose() {
			
		}
	}
}
