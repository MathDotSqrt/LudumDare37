using MathDotSqrt.Sqrt3D.AudioEngine;
using MathDotSqrt.Sqrt3D.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.GameState {
	/// <summary>
	/// GameState is an abstract class that other sub GameStates inherit
	/// It provides a specific format that all GameStates need to follow
	/// </summary>
	public abstract class GameState {

		protected GameStateManager gsm;

		public GameState(GameStateManager gsm) {
			this.gsm = gsm; //GSM is passed to allow GameState switching to happen within the gameStates them selfs
		}

		//All your favorite functions as seen in the GSM
		public abstract void Init();
		public abstract void Update(float delta);
		public abstract void Render(OpenGLRenderer renderer);
		public abstract void PlayAudio(OpenALPlayer player);
		public abstract void UpdateResize();
		public abstract void Dispose();
	}
}
