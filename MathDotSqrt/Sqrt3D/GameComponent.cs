using MathDotSqrt.Sqrt3D.GameState;
using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.AudioEngine;
using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using OpenTK.Input;

namespace MathDotSqrt.Sqrt3D {
	/// <summary>
	/// GameWindow is an OpenTK class that creates a window with a renderable screen.
	///
	/// GameComponent inherites GameWindow which allows GameComponent to have all the sexy
	/// methods from GameWindow.
	///
	/// All the methods in GameWindow are event based and are only called when something happens to the window
	/// i.e. (When a nigga tries to resize the window the OnResize will be called)
	/// </summary>
	public class GameComponent : GameWindow {
		private const float FPS = 1 / 60f;

		private Window window;
		private GameStateManager gsm;
		private OpenGLRenderer renderer;
		private OpenALPlayer player;

		private float totalDelta = 0;
		private bool hasFocus = false;

		public GameComponent(int width, int height) : base(width, height) {
			//The only use for this constructor is to pass the width and heihgt values to the GameWindow to create the window
		}

		
		protected override void OnLoad(EventArgs e) {
			base.OnLoad(e);

			window = new Window(this.Width, this.Height);               //Window stores the width and heihgt, and calculates the aspect ratiop from it
			renderer = new OpenGLRenderer() {                           //This is where we instantiate and configure the renderer
				ClearColor = Util.Math.Color.BlackCock							
			};
			//player = new OpenALPlayer();
			gsm = new GameStateManager(GameStateManager.PLAY_STATE);    //Launches the game in the [PLAY_STATE]
			OnFocusedChanged(e);
		}
		
		protected override void OnUpdateFrame(FrameEventArgs e) {
			base.OnUpdateFrame(e);

			//Update time is the ammount of time passed from the previous frame (called delta normally)
			//this is variable so it is used for physics calculation with enitties

			Input.AppendCurrentState();
			Input.UpdateInput();
			if(Input.FullScreenToggle) {
				if(WindowState == WindowState.Normal)
					WindowState = WindowState.Fullscreen;
				else
					WindowState = WindowState.Normal;
			}


			gsm.UpdateGameState((float)FPS);
			if(Focused) {
				Input.MouseVel.X = Mouse.X / (float)Window.WIDTH - .5f;
				Input.MouseVel.Y = Mouse.Y / (float)Window.HEIGHT - .5f;

				//Output.Good(Input.MouseVel);

				Point center = new Point(Width / 2, Height / 2);
				Point mousePos = PointToScreen(center);
				OpenTK.Input.Mouse.SetPosition(mousePos.X, mousePos.Y);
			}

			Input.AppendLastState();
		}

		
		protected override void OnRenderFrame(FrameEventArgs e) {
			base.OnRenderFrame(e);
			gsm.PlayAudioGameState(player);


			renderer.Prepare();             //configuration code that has to run before every render frame
			gsm.RenderGameState(renderer);  //Tells the current gamestate to render with the OpenGLRenderer
			this.SwapBuffers();             //Swap the pixel buffers from the default windows one every frame

		}

		
		protected override void OnResize(EventArgs e) {
			base.OnResize(e);

			window.OnWindowResize(Width, Height);   //passes the GameWindow's new heights and widths
			gsm.UpdateResizeGameState();
		}

		protected override void OnFocusedChanged(EventArgs e) {
			base.OnFocusedChanged(e);
			if(Focused) {
				CursorVisible = false;
			}
			else {
				CursorVisible = true;
			}
		}

		protected override void OnClosing(CancelEventArgs e) {
			base.OnClosing(e);
			//TODO actually make this do more shit
			VAOLoader.Dispose();
		}
		
	}
}
