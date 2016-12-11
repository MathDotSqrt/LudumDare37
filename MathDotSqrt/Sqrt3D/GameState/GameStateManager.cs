using MathDotSqrt.Sqrt3D.AudioEngine;
using MathDotSqrt.Sqrt3D.GameState.GameStates;
using MathDotSqrt.Sqrt3D.RenderEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.GameState {
	
	public class GameStateManager {

		private int currentState = -1;      //The currently selected GameState to be Updated/Render

		public const int MENU_STATE = 0;    //Index value of MenuState (sub class of GameState)
		public const int OPTION_STATE = 1; //Index value of OptionState (sub class of GameState)
		public const int PLAY_STATE = 2;    //Index value of PlayState (sub class of GameState)
		public const int TEST_STATE = 3;

		private List<GameState> gameStates; //All GameStates are stored in a List<GameState>


		public GameStateManager(int state = 0) {
			gameStates = new List<GameState>();     //initalizes the GameState List
			gameStates.Add(new MenuState(this));    //Adds the MenuState to the GameState List
			gameStates.Add(new OptionState(this));    //Adds the OptionState to the GameState List
			gameStates.Add(new PlayState(this));    //Adds the MenuState to the GameState List
			gameStates.Add(new TestState(this));    //Adds the TestState to the GameState List

			EnterGameState(state);                  //Enters the GameState specified from the constructor
		}

		
		public void EnterGameState(int state) {
			// if state is a valid state number then go to that gameState
			if(state < 0 || state >= gameStates.Count)
				return;

			this.currentState = state;  //sets the current gameState index 
			this.InitGameState();       //Initalizes said current GameState
		}

		
		public void InitGameState() {
			gameStates[currentState].Init();
		}
		public void UpdateGameState(float delta) {
			gameStates[currentState].Update(delta);
		}
		public void RenderGameState(OpenGLRenderer renderer) {
			gameStates[currentState].Render(renderer);
		}
		public void PlayAudioGameState(OpenALPlayer player) {
			gameStates[currentState].PlayAudio(player);
		}
		public void UpdateResizeGameState() {
			gameStates[currentState].UpdateResize();
		}

		public void DisposeGameState() {
			gameStates[currentState].Dispose();
		}
		public void DisposeAllGameStates() {
			for(int i = 0; i < gameStates.Count; i++) {
				gameStates[i].Dispose();
			}
		}

	}
}
