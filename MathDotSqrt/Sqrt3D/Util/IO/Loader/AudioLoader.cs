using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Audio.OpenAL;
using System.IO;
using NAudio.Wave;
namespace MathDotSqrt.Sqrt3D.Util.IO.Loader {
	public class AudioLoader {

		
		public static int BufferWave(string path) {
			int buffer = AL.GenBuffer();
			BufferWave(path, buffer);
			return buffer;
		}
		public static void BufferWave(string path, int buffer) {

			int channels;
			int bits; 
			int rate;
			byte[] soundData = LoadWaveFile(path, out channels, out bits, out rate);
			AL.BufferData(buffer, GetSoundFormat(channels, bits), soundData, soundData.Length, rate);
		}
		public static byte[] LoadWaveFile(string file, out int channels, out int bits, out int rate) {
			string path = FilePaths.AUDIO_RELATIVE + file;
			channels = -1;
			bits = -1;
			rate = -1;

			if(!System.IO.File.Exists(path)) {
				Output.Error("AudioLoader.LoadWaveFile: " + file + " does not exist");
				return new byte[0];
			}

			Stream stream = File.Open(path, FileMode.Open);
			using(BinaryReader reader = new BinaryReader(stream)) {


				//RIFF HEADER
				string riff_header = new string(reader.ReadChars(4));
				long riff_chunck_size = reader.ReadInt32();
				string riff_format = new string(reader.ReadChars(4));

				if(riff_header != "RIFF" || riff_format != "WAVE") {
					Output.Error("AudioLoader.LoadWaveFile: Audio format not supported");
					return new byte[0];
				}

				//WAVE FORMAT
				string sub_chunck_id = new string(reader.ReadChars(4));
				long sub_chunck_size = reader.ReadInt32();
				short audio_format = reader.ReadInt16();
				short num_channels = reader.ReadInt16();
				int sample_rate = reader.ReadInt32();
				long byte_rate = reader.ReadInt32();
				short block_align = reader.ReadInt16();
				short bits_per_sample = reader.ReadInt16();

				if(sub_chunck_id != "fmt ") {
					Output.Error("AudioLoader.LoadWaveFile: Invalid WAVE format");
					return new byte[0];
				}

				if(sub_chunck_size > 16) {
					Output.Error("AudioLoader.LoadWaveFile: Ummm, please check this, sub chunck size is > 16 extra paramaters?");
					return new byte[0];
				}

				//WAVE DATA
				string sub_chunck2_id = new string(reader.ReadChars(4));
				long sub_chunck2_size = reader.ReadInt32();
				long length = (long)reader.BaseStream.Length;



				channels = num_channels;
				bits = bits_per_sample;
				rate = sample_rate;

				return reader.ReadBytes((int)sub_chunck2_size);
			}
		}

		private static ALFormat GetSoundFormat(int channels, int bits) {
			switch(channels) {
				case 1:
				return bits == 8 ? ALFormat.Mono8 : ALFormat.Mono16;
				case 2:
				return bits == 8 ? ALFormat.Stereo8 : ALFormat.Stereo16;
				default:
				Output.Error("AudioLoader.GetSoundFormat: The specified sound format is not supported.");
				throw new NotSupportedException("The specified sound format is not supported.");
			}
		}
	}
}
