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
		public static bool IsEnterTyped { get; private set; }
		public static bool IsDelTyped { get; private set; }
		public static bool IsGTyped { get; private set; }
		public static bool IsCTyped { get; private set; }
		public static bool IsRShift { get; private set; }

		public static bool FullScreenToggle = false;

		public static Vector2 MouseVel;
		public static float SensitivityX = 80;
		public static float SensitivityY = 70;

		public static bool IsUpTyped = false;
		public static bool IsDownTyped = false;
		public static bool IsLeftTyped = false;
		public static bool IsRightTyped = false;
		public static bool isAltTyped = false;
		public static bool isRControlTyped = false;

		public static bool isTTyped = false;

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
			isAltTyped = false;
			isRControlTyped = false;
			IsEnterTyped = false;
			IsDelTyped = false;
			isTTyped = false;
			IsGTyped = false;
			IsCTyped = false;
			IsRShift = false;

			if( TestType(Key.Down) )
				IsUpTyped = true;
			if( TestType(Key.Down) )
				IsDownTyped = true;
			if(TestType(Key.Left))
				IsLeftTyped = true;
			if(TestType(Key.Right))
				IsRightTyped = true;
			if(TestType(Key.M))
				IsMTyped = true;
			if(TestType(Key.AltRight))
				isAltTyped = true;
			if(TestType(Key.RControl))
				isRControlTyped = true;
			if(TestType(Key.T))
				isTTyped = true;
			if(TestType(Key.Enter))
				IsEnterTyped = true;
			if(TestType(Key.Delete))
				IsDelTyped = true;
			if(TestType(Key.G))
				IsGTyped = true;
			if(TestType(Key.C))
				IsCTyped = true;
			if(TestType(Key.ShiftRight))
				IsRShift = true;

		}

		private static bool TestType(Key k) {
			return currentState.IsKeyDown(k) && lastFrameState.IsKeyUp(k);
		}

		public static void UpdatePlayer(Player player) {
			KeyboardState k = Keyboard.GetState();

			if(currentState.IsKeyDown(Key.W)) {
				player.MoveForward(.6f);
			}
			if(currentState.IsKeyDown(Key.S)) {
				player.MoveForward(-.6f);
			}
			if(currentState.IsKeyDown(Key.D)) {
				player.MoveLeft(.6f);
			}
			if(currentState.IsKeyDown(Key.A)) {
				player.MoveLeft(-.6f);
			}
			if(currentState.IsKeyDown(Key.Space) && lastFrameState.IsKeyUp(Key.Space))
				player.Jump();
			if(currentState.IsKeyDown(Key.ShiftLeft))
				player.MoveUp(-.6f);
			if(currentState.IsKeyDown(Key.Escape))
				Environment.Exit(0);

			player.camera.RotateY(MouseVel.X * SensitivityX);
			player.camera.RotateX(MouseVel.Y * SensitivityY);


		}


	}

}
