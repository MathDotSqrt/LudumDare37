using MathDotSqrt.Sqrt3D.Util;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {
	public class Geometry3d : Geometry{
		protected List<Vector3> vertexList = new List<Vector3>();
		protected List<Vector2> textureList = new List<Vector2>();
		protected List<Vector3> normalList = new List<Vector3>();
		protected List<int> indexList = new List<int>();

		public float[] Verticies {
			get;
			protected set;
		}
		public float[] TextureCoords {
			get;
			protected set;
		}
		public float[] Normals {
			get;
			protected set;
		}
		public int[] Indicies {
			get;
			protected set;
		}

		protected void GenTriangleFace(Face face) {
			for(int i = 0; i < 3; i++) {

				int mergeIndex = -1;
				foreach(int index in indexList) {
					if(face.Verticies[i] == vertexList[index]) { //TODO add some margin of error for floating point percision
						mergeIndex = index;
						break;
					}
				}

				if(mergeIndex != -1) {
					indexList.Add(mergeIndex);
				}
				else {
					vertexList.Add(face.Verticies[i]);
					textureList.Add(face.TextureCoords[i]);
					normalList.Add(face.Normals[i]);

					if(indexList.Count == 0)
						indexList.Add(0);
					else
						indexList.Add(indexList.Max() + 1);
				}
			}
		}
		protected void GenArrays() {
			Verticies = ArrayConverter.ConvertVectorListToFloatArray(vertexList);
			TextureCoords = ArrayConverter.ConvertVectorListToFloatArray(textureList);
			Normals = ArrayConverter.ConvertVectorListToFloatArray(normalList);
			Indicies = indexList.ToArray();
		}
		protected void MergeVerticies() {

		}
		protected void CalculateTrueVertexCount() {

		}
	}
}
