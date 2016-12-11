using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.AudioEngine;
using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects.Cameras;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI.Fonts;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using OpenTK.Input;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.Query;

namespace MathDotSqrt.Sqrt3D.GameState.GameStates{
	public class OptionState : GameState{
		private Scene scene;
		private GuiField gui;

		public OptionState(GameStateManager gsm) : base(gsm){

		}

		public override void Dispose(){
			
		}

		public override void Init(){
			scene = new Scene();
			Camera camera = new PerspectiveCamera(70, Window.ASPECT_RATIO, .01f, 1000);
			scene.Add(camera);

			gui = new GuiField();

			Font font = FontLoader.LoadFont("candara");
			Material fontMaterial = new GuiFontMaterial(font.Texture);

			Geometry2d titleGeometry = new FontGeometry(font, "Options", 3, 1000, 10, 20);
			Geometry2d backGeometry = new FontGeometry(font, "Back to Menu", 3, 1000, 10, 20);

			Geometry2d sensitivityGeometry = new FontGeometry(font, "Sensitivity: ", 2, 1000, 10, 20);
			Geometry2d sensitivityLowGeometry = new FontGeometry(font, "Low", 1, 1000, 10, 20);
			Geometry2d sensitivityMediumGeometry = new FontGeometry(font, "Medium", 1, 1000, 10, 20);
			Geometry2d sensitivityHighGeometry = new FontGeometry(font, "High", 1, 1000, 10, 20);

			Geometry2d quad = new QuadGeometry2d();

			Material sensitivityBoxMaterial = new GuiBasicMaterial() {
				Color = Color.Blue
			};

			Material sensitivityLowBoxMaterial = new GuiBasicMaterial(){
				Color = Color.Grey
			};

			Material sensitivityMediumBoxMaterial = new GuiBasicMaterial(){
				Color = Color.Red
			};

			Material sensitivityHighBoxMaterial = new GuiBasicMaterial(){
				Color = Color.Grey
			};

			Material backBoxMaterial = new GuiBasicMaterial()
			{
				Color = Color.Grey
			};

			GuiElement sensitivityBoxParent = new GuiElement(quad, sensitivityBoxMaterial) {
				Name = "sensitivity"
			};
			sensitivityBoxParent.PixelWidth = 1000;
			sensitivityBoxParent.PixelHeight = 100;
			sensitivityBoxParent.CenterX();
			sensitivityBoxParent.CenterY();
			gui.Add(sensitivityBoxParent);

			GuiElement sensitivityLowBox = new GuiElement(sensitivityBoxParent, quad, sensitivityLowBoxMaterial){
				Name = "low"
			};
			sensitivityLowBox.PixelWidth = 120;
			sensitivityLowBox.PixelHeight = 100;
			sensitivityLowBox.PixelMarginRight = sensitivityHighGeometry.BoundingWidth + 500;
			gui.Add(sensitivityLowBox);

			GuiElement sensitivityMediumBox = new GuiElement(sensitivityBoxParent, quad, sensitivityMediumBoxMaterial) {
				Name = "medium"
			};
			sensitivityMediumBox.PixelWidth = 120;
			sensitivityMediumBox.PixelHeight = 100;
			sensitivityMediumBox.PixelMarginRight = sensitivityHighGeometry.BoundingWidth + 300;
			gui.Add(sensitivityMediumBox);

			GuiElement sensitivityHighBox = new GuiElement(sensitivityBoxParent, quad, sensitivityHighBoxMaterial){
				Name = "high"
			};
			sensitivityHighBox.PixelWidth = 120;
			sensitivityHighBox.PixelHeight = 100;
			sensitivityHighBox.PixelMarginRight = 100;
			gui.Add(sensitivityHighBox);

			GuiElement backBox = new GuiElement(quad, backBoxMaterial)
			{
				Name = "back"
			};
			backBox.PixelWidth = 600;
			backBox.PixelHeight = 100;
			backBox.PixelMarginBottom = 100;
			gui.Add(backBox);

			GuiElement title = new GuiElement(titleGeometry, fontMaterial);
			title.PixelMarginTop = 100;
			gui.Add(title);

			GuiElement back = new GuiElement(backBox, backGeometry, fontMaterial);
			gui.Add(back);

			GuiElement sensitivityTitle = new GuiElement(sensitivityBoxParent, sensitivityGeometry, fontMaterial);
			sensitivityTitle.PixelMarginLeft = 10;
			gui.Add(sensitivityTitle);

			GuiElement sensitivityLowTitle = new GuiElement(sensitivityLowBox, sensitivityLowGeometry, fontMaterial);
			gui.Add(sensitivityLowTitle);

			GuiElement sensitivityMediumTitle = new GuiElement(sensitivityMediumBox, sensitivityMediumGeometry, fontMaterial);
			gui.Add(sensitivityMediumTitle);

			GuiElement sensitivityHighTitle = new GuiElement(sensitivityHighBox, sensitivityHighGeometry, fontMaterial);
			gui.Add(sensitivityHighTitle);
		}

		public override void PlayAudio(OpenALPlayer player){
			
		}

		public override void Render(OpenGLRenderer renderer){
			renderer.RenderScene(scene);
			renderer.RenderGui(gui);
		}

		private int horizontalState = 1;
		private int verticalState = 0;
		public override void Update(float delta){
			String[] stateElementVertical = { "sensitivity", "back" };
			String[] stateElementHorizontal = { "low", "medium", "high" };

			if (Keyboard.GetState().IsKeyDown(Key.Escape))
				Environment.Exit(0);

			////////////////////////////////////////////////////////////
			//Horzintal/////////////////////////////////////////////////
			////////////////////////////////////////////////////////////
			if (Input.IsLeftTyped || Input.IsRightTyped){
				GuiBasicMaterial button = (GuiBasicMaterial)gui.GetElement(stateElementHorizontal[horizontalState]).Material;
				button.Color = Color.Grey;
			}

			if (Input.IsLeftTyped){
				if (horizontalState != 0)
					horizontalState--;
			}
			if (Input.IsRightTyped){
				if (horizontalState != stateElementHorizontal.Length - 1)
					horizontalState++;
			}

			if (Input.IsLeftTyped || Input.IsRightTyped){
				GuiBasicMaterial button = (GuiBasicMaterial)gui.GetElement(stateElementHorizontal[horizontalState]).Material;
				button.Color = Color.Red;
			}

			////////////////////////////////////////////////////////////
			//Vertical//////////////////////////////////////////////////
			////////////////////////////////////////////////////////////

			if (Input.IsUpTyped || Input.IsDownTyped){
				switch (stateElementVertical[verticalState]){
					case "sensitivity":
						GuiBasicMaterial sensbutton = (GuiBasicMaterial)gui.GetElement(stateElementHorizontal[horizontalState]).Material;
						sensbutton.Color = Color.Red; break;
					default:
						GuiBasicMaterial button = (GuiBasicMaterial)gui.GetElement(stateElementVertical[verticalState]).Material;
						button.Color = Color.Grey; break;
				}
			}

			if (Input.IsUpTyped){
				if (verticalState != 0)
					verticalState--;
			}
			if (Input.IsDownTyped){
				if (verticalState != stateElementVertical.Length - 1)
					verticalState++;
			}
			 
			if (Input.IsUpTyped || Input.IsDownTyped){
				switch (stateElementVertical[verticalState]){
					case "sensitivity":
						GuiBasicMaterial sensbutton = (GuiBasicMaterial)gui.GetElement(stateElementHorizontal[horizontalState]).Material;
						sensbutton.Color = Color.Red; break;
					default:
						GuiBasicMaterial button = (GuiBasicMaterial)gui.GetElement(stateElementVertical[verticalState]).Material;
						button.Color = Color.Red; break;
				}
			}

			if (Keyboard.GetState().IsKeyDown(Key.Enter))
				switch (stateElementVertical[verticalState]){
					case "back": gsm.EnterGameState(GameStateManager.MENU_STATE); break; //call sesivitiy method and send stateElementHorizontal[horizontalState]
				}

		}

		public override void UpdateResize(){
			gui.UpdateScreenSize();
		}
	}
}
