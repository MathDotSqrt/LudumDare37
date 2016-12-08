using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using OpenTK;

namespace MathDotSqrt.Sqrt3D.World.Objects {
	public class AudioSource : Object3D{

		public int BufferID {
			get;
			private set;
		}
		public int SourceID{
			get;
			private set;
		}

		public float Volume {
			get;
			set;
		}
		public float Pitch {
			get;
			set;
		}
		
		private bool looping = false;
		public bool Looping {
			get {
				return looping;
			}
			set {
				looping = value;
				AL.Source(SourceID, ALSourceb.Looping, value);
			}
		}
		public bool IsPlaying {
			get {
				int value;
				AL.GetSource(SourceID, ALGetSourcei.SourceState, out value);
				return value == (int)ALSourceState.Playing;
			}
		}

		public AudioSource(string fileName, bool start = false) {
			BufferID = AudioLoader.BufferWave(fileName);
			SourceID = AL.GenSource();
			AL.Source(SourceID, ALSourcei.Buffer, BufferID);

			Volume = 1;
			Pitch = 1;

			if(start)
				Start();
		}

		public void Start() {
			Stop();
			Continue();
		}
		public void Pause() {
			AL.SourcePause(SourceID);
		}
		public void Continue() {
			AL.SourcePlay(SourceID);
		}
		public void Stop() {
			AL.SourceStop(SourceID);
		}

		public override void Dispose() {

		}
	}
}
