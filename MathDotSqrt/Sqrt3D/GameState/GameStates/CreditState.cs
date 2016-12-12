using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.AudioEngine;
using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI;
using MathDotSqrt.Sqrt3D.World.Objects.Cameras;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK.Input;

namespace MathDotSqrt.Sqrt3D.GameState.GameStates {
	public class CreditState : GameState {

		private Scene scene;
		private GuiField gui;

		public CreditState(GameStateManager gsm) : base(gsm) {
		}

		public override void Dispose() {
			
		}

		public override void Init() {
			scene = new Scene();
			Camera camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 1000);
			scene.Add(camera);

			gui = new GuiField();

			Font font = FontLoader.LoadFont("candara");
			Material fontMaterial = new GuiFontMaterial(font.Texture);

			Geometry2d titleGeometry = new FontGeometry(font, "Credits", 3, 1000, 10, 20);
			Geometry2d programTitleGeometry = new FontGeometry(font, "---Programmers---", 1, 1000, 10, 20);
			Geometry2d programNameGeometry = new FontGeometry(font, "Chris and Jake", 1, 500, 80, 20);
			Geometry2d musicTitleGeometry = new FontGeometry(font, "---Music & Textures---", 1, 1000, 80, 20);
			Geometry2d musicNameGeometry = new FontGeometry(font, "Corbin", 1, 1000, 80, 20);
			Geometry2d memersTitleGeometry = new FontGeometry(font, "---Memers---", 1, 1000, 80, 20);
			Geometry2d memersNameGeometry = new FontGeometry(font, "Nathan, Jamie, and Sean", 1, 1000, 80, 20);
			Geometry2d backGeometry = new FontGeometry(font, "Back to Menu", 3, 1000, 10, 20);

			Geometry2d quad = new QuadGeometry2d();

			Material backBoxMaterial = new GuiBasicMaterial() {
				Color = Color.Red
			};

			GuiElement backBox = new GuiElement(quad, backBoxMaterial) {
				
			};
			backBox.PixelWidth = 600;
			backBox.PixelHeight = 100;
			backBox.PixelMarginBottom = 100;
			gui.Add(backBox);

			GuiElement title = new GuiElement(titleGeometry, fontMaterial);
			title.PixelMarginTop = 50;
			gui.Add(title);

			GuiElement programTitle = new GuiElement(programTitleGeometry, fontMaterial);
			programTitle.PixelMarginTop = 200;
			programTitle.CenterX();
			gui.Add(programTitle);

			GuiElement programName = new GuiElement(programNameGeometry, fontMaterial);
			programName.PixelMarginTop = 250;
			gui.Add(programName);

			GuiElement otherNameTitle = new GuiElement(musicTitleGeometry, fontMaterial);
			otherNameTitle.PixelMarginTop = 300;
			otherNameTitle.CenterX();
			gui.Add(otherNameTitle);

			GuiElement otherName = new GuiElement(musicNameGeometry, fontMaterial);
			otherName.PixelMarginTop = 350;
			gui.Add(otherName);

			GuiElement memeTitleGeometry = new GuiElement(memersTitleGeometry, fontMaterial);
			memeTitleGeometry.PixelMarginTop = 400;
			memeTitleGeometry.CenterX();
			gui.Add(memeTitleGeometry);

			GuiElement memeNameGeometry = new GuiElement(memersNameGeometry, fontMaterial);
			memeNameGeometry.PixelMarginTop = 450;
			gui.Add(memeNameGeometry);

			GuiElement back = new GuiElement(backBox, backGeometry, fontMaterial);
			gui.Add(back);
		}

		public override void PlayAudio(OpenALPlayer player) {
			
		}

		public override void Render(OpenGLRenderer renderer) {
			renderer.RenderScene(scene);
			renderer.RenderGui(gui);
		}

		public override void Update(float delta) {
			if (Input.IsEnterTyped)
					gsm.EnterGameState(GameStateManager.MENU_STATE);
		}

		public override void UpdateResize() {
			gui.UpdateScreenSize();
		}
	}
}
