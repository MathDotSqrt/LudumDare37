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
		public static KeyboardState currentState {
			get;
			private set;
		}
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

		public static bool IsMTyped = false;

		public static void AppendCurrentState() {
			currentState = Keyboard.GetState();
		}
		public static void AppendLastState() {
			lastFrameState = currentState;
		}
		public static void UpdateInput() {
			FullScreenToggle = false;
			if(currentState.IsKeyDown(Key.F11) && lastFrameState.IsKeyUp(Key.F11))
				FullScreenToggle = true;

			IsUpTyped = false;
			IsDownTyped = false;
			IsLeftTyped = false;
			IsRightTyped = false;
			IsMTyped = false;
			if(currentState.IsKeyDown(Key.Up) && lastFrameState.IsKeyDown(Key.Up))
				IsUpTyped = true;
			if(currentState.IsKeyDown(Key.Down) && lastFrameState.IsKeyDown(Key.Down))
				IsDownTyped = true;
			if(currentState.IsKeyDown(Key.Left) && lastFrameState.IsKeyDown(Key.Left))
				IsLeftTyped = true;
			if(currentState.IsKeyDown(Key.Right) && lastFrameState.IsKeyDown(Key.Right))
				IsRightTyped = true;
			if(currentState.IsKeyDown(Key.M) && lastFrameState.IsKeyUp(Key.M))
				IsMTyped = true;
		}
		public static void UpdatePlayer(Player player) {
			KeyboardState k = Keyboard.GetState();

			if(currentState.IsKeyDown(Key.W) | currentState.IsKeyDown(Key.Up))
				player.MoveForward(.4f);
			if(currentState.IsKeyDown(Key.S) | currentState.IsKeyDown(Key.Down))
				player.MoveForward(-.4f);
			if(currentState.IsKeyDown(Key.D) | currentState.IsKeyDown(Key.Right))
				player.MoveLeft(.4f);
			if(currentState.IsKeyDown(Key.A) | currentState.IsKeyDown(Key.Left))
				player.MoveLeft(-.4f);
			if(currentState.IsKeyDown(Key.Space) && lastFrameState.IsKeyUp(Key.Space))
				player.Jump();
			if(currentState.IsKeyDown(Key.ShiftLeft))
				player.MoveUp(-.4f);
			if(currentState.IsKeyDown(Key.Escape))
				Environment.Exit(0);

			player.camera.RotateY(MouseVel.X * SensitivityX);
			player.camera.RotateX(MouseVel.Y * SensitivityY);


		}


	}

}
