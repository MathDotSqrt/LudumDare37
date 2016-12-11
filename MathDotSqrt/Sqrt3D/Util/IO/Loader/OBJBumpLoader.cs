using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.IO.Loader {
	public static class OBJBumpLoader {
		public static Geometry LoadOBJ(string fileName) {
			string filePath = FilePaths.GEOMETRY_RELATIVE + fileName;

			if(!System.IO.File.Exists(filePath)) {
				Output.Error("OBJBumpLoaderLoader.LoadOBJ: Could not load Geometry\r\n File Name: " + fileName + " does not exist");
				return null;
			}

			string[] OBJ = System.IO.File.ReadAllLines(filePath);
			List<Vector3> positionList = new List<Vector3>();
			List<Vector2> textureUVList = new List<Vector2>();
			List<Vector3> normalList = new List<Vector3>();
			List<TangentPoint> tangentPointList = new List<TangentPoint>();

			List<string> faceList = new List<string>();

			for(int i = 0; i < OBJ.Length; i++) {

				string[] line = OBJ[i].Replace("  ", " ").Split(' ');
				string tag = line[0];

				switch(tag) {
					case "v":
					//positionList.Add(new Vector3(Parse(line[1]), Parse(line[2]), Parse(line[3])));
					positionList.Add(new Vector3(Parse(line[1]), Parse(line[2]), Parse(line[3])));
					tangentPointList.Add(new TangentPoint());
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
			float[] tangentArray = new float[positionList.Count * 3];   //one tangent per face


			List<int> indicies = new List<int>();

			for(int i = 0; i < faceList.Count; i++) {
				string[] faceVertecies = faceList[i].Replace("  ", " ").Split(' ');

				List<Vector3> tempPos = new List<Vector3>();
				List<Vector2> tempUv = new List<Vector2>();
				List<int> tempTanIndex = new List<int>();

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

					tempPos.Add(positionList[vertexPointer]);
					tempUv.Add(textureUVList[textureUVIndex]);
					tempTanIndex.Add(vertexPointer);
				}

				
				CalculateTangents(tempPos[0], tempPos[1], tempPos[2], tempUv[0], tempUv[1], tempUv[2], 
					tangentPointList[tempTanIndex[0]], tangentPointList[tempTanIndex[1]], tangentPointList[tempTanIndex[2]]);
				//TODO test if this passes the reference
			}

			for(int i = 0; i < tangentPointList.Count; i++){
				tangentPointList[i].AverageTangents();
				tangentArray[i * 3] = tangentPointList[i].AveragedTangent.X;
				tangentArray[i * 3 + 1] = tangentPointList[i].AveragedTangent.Y;
				tangentArray[i * 3 + 2] = tangentPointList[i].AveragedTangent.Z;
			}

			float[] positionArray = ArrayConverter.ConvertVectorListToFloatArray(positionList);

			VAO vao = VAOLoader.LoadToVAO(positionArray, textureUVArray, normalArray, tangentArray, indicies.ToArray());
			VAOGeometry vaoGeometry = new VAOGeometry(vao);
			return vaoGeometry;
		}

		private static void ProcessVertex(Vector3 vertexm) {

		}

		private static void CalculateTangents(Vector3 position0, Vector3 position1, Vector3 postition2,
			Vector2 uv0, Vector2 uv1, Vector2 uv2, TangentPoint t0, TangentPoint t1, TangentPoint t2) {

			Vector3 deltaPos1 = Vector3.Subtract(position1, position0);
			Vector3 deltaPos2 = Vector3.Subtract(postition2, position0);
			Vector2 deltaUv1 = Vector2.Subtract(uv1, uv0);
			Vector2 deltaUv2 = Vector2.Subtract(uv2, uv0);

			float r = 1f / ( deltaUv1.X * deltaUv2.Y - deltaUv1.Y * deltaUv2.X );

			deltaPos1 = Vector3.Multiply(deltaPos1, deltaUv2.Y);
			deltaPos2 = Vector3.Multiply(deltaPos2, deltaUv1.Y);
			Vector3 tangent = Vector3.Subtract(deltaPos1, deltaPos2);
			tangent = Vector3.Multiply(tangent, r);

			t0.AddTangant(tangent);
			t1.AddTangant(tangent);
			t2.AddTangant(tangent);
		}
		private static float Parse(string parseString) {
			return float.Parse(parseString);
		}
	}
}
