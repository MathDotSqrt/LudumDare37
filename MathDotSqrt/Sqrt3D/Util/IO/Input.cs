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
			if(k.IsKeyDown(Key.Up) && lastFrameState.IsKeyUp(Key.Up))
				IsUpTyped = true;
			if(k.IsKeyDown(Key.Down) && lastFrameState.IsKeyUp(Key.Down))
				IsDownTyped = true;
			if(k.IsKeyDown(Key.Left) && lastFrameState.IsKeyUp(Key.Left))
				IsLeftTyped = true;
			if(k.IsKeyDown(Key.Right) && lastFrameState.IsKeyUp(Key.Right))
				IsRightTyped = true;

			//Output.Good(FullScreenToggle);

			lastFrameState = k;
		}
		public static void UpdateCamera(Camera camera) {
			KeyboardState k = Keyboard.GetState();
			if(k.IsKeyDown(Key.W) | k.IsKeyDown(Key.Up))
				camera.MoveForward(.1f);
			if(k.IsKeyDown(Key.S) | k.IsKeyDown(Key.Down))
				camera.MoveForward(-.1f);
			if(k.IsKeyDown(Key.D) | k.IsKeyDown(Key.Right))
				camera.MoveLeft(.1f);
			if(k.IsKeyDown(Key.A) | k.IsKeyDown(Key.Left))
				camera.MoveLeft(-.1f);
			if(k.IsKeyDown(Key.Space))
				camera.Position.Y += .1f;
			if(k.IsKeyDown(Key.ShiftLeft))
				camera.Position.Y -= .1f;
			if(k.IsKeyDown(Key.Escape))
				Environment.Exit(0);

			camera.RotateY(MouseVel.X * SensitivityX);
			camera.RotateX(MouseVel.Y * SensitivityY);
		}


	}

}
