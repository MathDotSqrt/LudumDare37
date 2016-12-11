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
using OpenTK.Input;
using MathDotSqrt.Sqrt3D.World.Objects;

namespace MathDotSqrt.Sqrt3D.Util.IO{
	public static class Input {

		public static KeyboardState lastFrameState {
			get;
			private set;
		}

		public static bool FullScreenToggle = false;

		public static Vector2 MouseVel;
		public static float SensitivityX = 80;
		public static float SensitivityY = 70;

		public static bool IsUpTyped = false;
		public static bool IsDownTyped = false;
		public static bool IsLeftTyped = false;
		public static bool IsRightTyped = false;

		public static void UpdateInput() {
			KeyboardState k = Keyboard.GetState();
			FullScreenToggle = false;
			if(k.IsKeyDown(Key.F11) && lastFrameState.IsKeyUp(Key.F11))
				FullScreenToggle = true;

			IsUpTyped = false;
			IsDownTyped = false;
			IsLeftTyped = false;
			IsRightTyped = false;
			if(k.IsKeyDown(Key.Up) && lastFrameState.IsKeyDown(Key.Up))
				IsUpTyped = true;
			if(k.IsKeyDown(Key.Down) && lastFrameState.IsKeyDown(Key.Down))
				IsDownTyped = true;
			if(k.IsKeyDown(Key.Left) && lastFrameState.IsKeyDown(Key.Left))
				IsLeftTyped = true;
			if(k.IsKeyDown(Key.Right) && lastFrameState.IsKeyDown(Key.Right))
				IsRightTyped = true;

			//Output.Good(FullScreenToggle);

			lastFrameState = k;
		}
		public static void UpdatePlayer(Player player) {
			KeyboardState k = Keyboard.GetState();
			if(k.IsKeyDown(Key.W) | k.IsKeyDown(Key.Up))
				player.MoveForward(.1f);
			if(k.IsKeyDown(Key.S) | k.IsKeyDown(Key.Down))
				player.MoveForward(-.1f);
			if(k.IsKeyDown(Key.D) | k.IsKeyDown(Key.Right))
				player.MoveLeft(.1f);
			if(k.IsKeyDown(Key.A) | k.IsKeyDown(Key.Left))
				player.MoveLeft(-.1f);
			if(k.IsKeyDown(Key.Space))
				player.MoveUp(.1f);
			if(k.IsKeyDown(Key.ShiftLeft))
				player.MoveUp(-.1f);
			if(k.IsKeyDown(Key.Escape))
				Environment.Exit(0);

			player.camera.RotateY(MouseVel.X * SensitivityX);
			player.camera.RotateX(MouseVel.Y * SensitivityY);
		}


	}

}
