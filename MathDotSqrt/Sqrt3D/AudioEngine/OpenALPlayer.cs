using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK.Audio.OpenAL;
using OpenTK.Audio;
using OpenTK.Graphics.OpenGL;
using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects;
using OpenTK;

namespace MathDotSqrt.Sqrt3D.AudioEngine {
	public class OpenALPlayer {

		private AudioContext context;

		public OpenALPlayer(float speedOfSound = 343.3f) {
			context = new AudioContext();
			AL.SpeedOfSound(speedOfSound);
			AL.DistanceModel(ALDistanceModel.ExponentDistanceClamped);
		}
		public void PlaySceneAudio(Scene scene) {
			Camera camera = scene.Camera;

			List<AudioSource> sources = scene.GetAllSources();

			Vector3 cameraWSP = camera.WorldSpacePostion;
			AL.Listener(ALListener3f.Position, ref cameraWSP);
			AL.Listener(ALListener3f.Velocity, ref camera.PositionVelocity);

			foreach(AudioSource source in sources) {
				source.UpdateTransformationMatrix();
				Vector3 sourceWSP = source.WorldSpacePostion;
				AL.Source(source.SourceID, ALSource3f.Position, ref sourceWSP);
				AL.Source(source.SourceID, ALSource3f.Velocity, ref source.PositionVelocity);
				AL.Source(source.SourceID, ALSourcef.Gain, source.Volume);
				AL.Source(source.SourceID, ALSourcef.Pitch, source.Pitch);
			}
		}
	}
}
