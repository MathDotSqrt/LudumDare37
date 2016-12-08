using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.IO.Loader {

	public static class OBJLoader {

		public static Geometry LoadOBJ(string fileName) {
			string filePath = FilePaths.GEOMETRY_RELATIVE + fileName;

			if(!System.IO.File.Exists(filePath)) {
				Output.Error("OBJLoader.LoadOBJ: Could not load Geometry\r\n File Name: " + fileName + " does not exist");
				return null;
			}

			string[] OBJ = System.IO.File.ReadAllLines(filePath);
			List<Vector3> positionList = new List<Vector3>();
			List<Vector2> textureUVList = new List<Vector2>();
			List<Vector3> normalList = new List<Vector3>();

			List<string> faceList = new List<string>();

			for(int i = 0; i < OBJ.Length; i++) {

				string[] line = OBJ[i].Replace("  ", " ").Split(' ');
				string tag = line[0];

				switch(tag) {
					case "v":
					positionList.Add(new Vector3(Parse(line[1]), Parse(line[2]), Parse(line[3])));
					break;
					case "vt":
					textureUVList.Add(new Vector2(Parse(line[1]), Parse(line[2])));
					break;
					case "vn":
					normalList.Add(new Vector3(Parse(line[1]), Parse(line[2]), Parse(line[3])));
					break;
					case "f":
					faceList.Add(OBJ[i].Substring(2));
					break;

				}
			}
			float[] textureUVArray = new float[positionList.Count * 2];
			float[] normalArray = new float[positionList.Count * 3];

			List<int> indicies = new List<int>();

			for(int i = 0; i < faceList.Count; i++) {
				string[] faceVertecies = faceList[i].Replace("  ", " ").Split(' ');
				for(int j = 0; j < faceVertecies.Length; j++) {
					if(!faceVertecies[j].Contains('/'))
						continue;

					string[] faceVertex = faceVertecies[j].Split('/');

					//OBJ starts counting from 1 for what ever reason :/
					//-69 means there is no data in that face value
					int vertexPointer = ( faceVertex[0] != "" ? int.Parse(faceVertex[0]) - 1 : -69 );
					int textureUVIndex = ( faceVertex[1] != "" ? int.Parse(faceVertex[1]) - 1 : -69 );
					int normalIndex = ( faceVertex[2] != "" ? int.Parse(faceVertex[2]) - 1 : -69 );

					indicies.Add(vertexPointer);

					if(textureUVIndex != -69) {
						textureUVArray[vertexPointer * 2 + 0] = ( textureUVList[textureUVIndex].X );
						textureUVArray[vertexPointer * 2 + 1] = 1 - ( textureUVList[textureUVIndex].Y );    //textures are fliped for some reason
					}

					normalArray[vertexPointer * 3 + 0] = ( normalList[normalIndex].X );
					normalArray[vertexPointer * 3 + 1] = ( normalList[normalIndex].Y );
					normalArray[vertexPointer * 3 + 2] = ( normalList[normalIndex].Z );
				}
			}

			float[] positionArray = ArrayConverter.ConvertVectorListToFloatArray(positionList);

			VAO vao = VAOLoader.LoadToVAO(positionArray, textureUVArray, normalArray, indicies.ToArray());
			VAOGeometry vaoGeometry = new VAOGeometry(vao);
			return vaoGeometry;
		}
		private static float Parse(string parseString) {
			return float.Parse(parseString);
		}
	}
}
