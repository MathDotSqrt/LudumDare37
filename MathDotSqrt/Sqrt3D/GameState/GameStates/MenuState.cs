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
			Geometry2d playGeometry = new FontGeometry(font, "Play", 2, 1000, 10, 20);
			Geometry2d exitGeometry = new FontGeometry(font, "Exit", 2, 1000, 10, 20);

			Geometry2d quad = new QuadGeometry2d();
			Material playBoxMaterial = new GuiBasicMaterial() {
				Color = Color.Grey
			};
			Material exitBoxMaterial = new GuiBasicMaterial() {
				Color = Color.Purple
			};

			Material fontMaterial = new GuiFontMaterial(font.Texture);

			

			GuiElement playBox = new GuiElement(quad, playBoxMaterial) {
				Name = "play"
			};
			playBox.PixelWidth = 350;
			playBox.PixelHeight = 100;
			playBox.PixelY = Window.HEIGHT / 2 + 50 + 20;
			gui.Add(playBox);
			GuiElement exitBox = new GuiElement(quad, exitBoxMaterial) {
				Name = "exit"
			};
			exitBox.PixelWidth = 350;
			exitBox.PixelHeight = 100;
			exitBox.PixelY = Window.HEIGHT / 2 - 50 - 20;

			gui.Add(exitBox);

			GuiElement title = new GuiElement(titleGeometry, fontMaterial);
			gui.Add(title);
			GuiElement play = new GuiElement(playBox, playGeometry, fontMaterial);
			gui.Add(play);
			GuiElement exit = new GuiElement(exitBox, exitGeometry, fontMaterial);
			gui.Add(exit);

			title.PixelMarginTop = 100;
			//play.PixelMarginTop = 300;
			//exit.PixelMarginTop = 500;
		}

		public override void Update(float delta) {
			if (Keyboard.GetState().IsKeyDown(Key.Escape))
				Environment.Exit(0);

			int state = 0;
			String[] stateElement = new String[2];

			if (Keyboard.GetState().IsKeyDown(Key.Down)) {
				if (stateElement[state] == "play") {
					GuiBasicMaterial material = (GuiBasicMaterial)gui.GetElement("play").Material;
					material.Color = Color.CornflowerBlue;
					state = 0;
				}
				else {
					GuiBasicMaterial material = (GuiBasicMaterial)gui.GetElement("exit").Material;
					material.Color = Color.BlackCock;
					state = 1;
				}
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
