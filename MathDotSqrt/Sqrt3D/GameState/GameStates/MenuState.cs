using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.AudioEngine;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Cameras;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.Math;

namespace MathDotSqrt.Sqrt3D.GameState.GameStates {
	public class MenuState : GameState {
		private Scene scene;
		private GuiField gui;

		public MenuState(GameStateManager gsm) : base(gsm) {

		}

		public override void Init() {
			scene = new Scene();
			Camera camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 1000);
			scene.Add(camera);


			gui = new GuiField();

			Font font = FontLoader.LoadFont("candara");

			Geometry2d titleGeometry = new FontGeometry(font, "Title", 3, 1000, 10, 20);
			Geometry2d playGeometry = new FontGeometry(font, "Play", 1, 1000, 10, 20);
			Geometry2d optionsGeometry = new FontGeometry(font, "Options", 1, 1000, 10, 20);
			Geometry2d exitGeometry = new FontGeometry(font, "Exit", 1, 1000, 10, 20);

			Geometry2d quad = new QuadGeometry2d();
			Material playBoxMaterial = new GuiBasicMaterial() {
				Color = Color.Grey
			};
			Material optionsBoxMaterial = new GuiBasicMaterial() {
				Color = Color.Grey
			};
			Material exitBoxMaterial = new GuiBasicMaterial() {
				Color = Color.Grey
			};

			Material fontMaterial = new GuiFontMaterial(font.Texture);

			GuiElement playBox = new GuiElement(quad, playBoxMaterial) {
				Name = "play"
			};
			playBox.PixelWidth = 350;
			playBox.PixelHeight = 100;
			playBox.PixelY = Window.HEIGHT / 2 - 50 - 80;
			gui.Add(playBox);

			GuiElement optionsBox = new GuiElement(quad, optionsBoxMaterial) {
				Name = "options"
			};
			optionsBox.PixelWidth = 350;
			optionsBox.PixelHeight = 100;
			optionsBox.PixelY = Window.HEIGHT / 2;
			gui.Add(playBox);

			GuiElement exitBox = new GuiElement(quad, exitBoxMaterial) {
				Name = "exit"
			};
			exitBox.PixelWidth = 350;
			exitBox.PixelHeight = 100;
			exitBox.PixelY = Window.HEIGHT / 2 + 50 + 80;
			gui.Add(exitBox);

			GuiElement title = new GuiElement(titleGeometry, fontMaterial);
			gui.Add(title);

			GuiElement play = new GuiElement(playBox, playGeometry, fontMaterial);
			gui.Add(play);

			GuiElement options = new GuiElement(optionsBox, optionsGeometry, fontMaterial);
			gui.Add(optionsBox);

			GuiElement exit = new GuiElement(exitBox, exitGeometry, fontMaterial);
			gui.Add(exit);

			title.PixelMarginTop = 100;
		}
		private int state = 0;

		public override void Update(float delta) {
			string[] stateElement = { "play", "options", "exit" };
			if (Input.IsDownTyped || Input.IsUpTyped) {
				GuiBasicMaterial button = (GuiBasicMaterial)gui.GetElement(stateElement[state]).Material;
				button.Color = Color.Grey;
			}

			if (Input.IsUpTyped) {
				if (state != 0)
					state--;
			}

			if (Input.IsDownTyped) {
				if (state != stateElement.Length - 1)
					state++;
			}

			if (Input.IsDownTyped || Input.IsUpTyped) {
				GuiBasicMaterial button = (GuiBasicMaterial)gui.GetElement(stateElement[state]).Material;
				button.Color = Color.Red;
			}

			if (Keyboard.GetState().IsKeyDown(Key.Enter))
				switch (stateElement[state]) {
					case "play":
					gsm.EnterGameState(GameStateManager.PLAY_STATE);
					break;
					case "options":
					gsm.EnterGameState(GameStateManager.OPTION_STATE);
					break;
					default:
					Environment.Exit(0);
					break;
				}
		}

		public override void Render(OpenGLRenderer renderer) {
			renderer.RenderScene(scene);
			renderer.RenderGui(gui);
		}

		public override void PlayAudio(OpenALPlayer player) {

		}

		public override void UpdateResize() {
			gui.UpdateScreenSize();
		}

		public override void Dispose() {

		}
	}
}