using MathDotSqrt.Sqrt3D.Util.IO.Loader;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Geometries {
	public class PlaneGeometry : Geometry3d {

		public PlaneGeometry(float width, float height, int widthDivisions = 1, int heightDivisions = 1){

			Face face1 = new Face();
			face1.Verticies[0] = new Vector3(width / 2, height / 2, 0);
			face1.Verticies[1] = new Vector3(-width / 2, height / 2, 0);
			face1.Verticies[2] = new Vector3(-width / 2, -height / 2, 0);

			face1.TextureCoords[0] = new Vector2(1, 0);
			face1.TextureCoords[1] = new Vector2(0, 0);
			face1.TextureCoords[2] = new Vector2(0, 1);

			face1.Normals[0] = new Vector3(0, 0, 1);
			face1.Normals[1] = new Vector3(0, 0, 1);
			face1.Normals[2] = new Vector3(0, 0, 1);

			Face face2 = new Face();
			face2.Verticies[0] = new Vector3(width / 2, height / 2, 0);
			face2.Verticies[1] = new Vector3(-width / 2, -height / 2, 0);
			face2.Verticies[2] = new Vector3(width / 2, -height / 2, 0);

			face2.TextureCoords[0] = new Vector2(1, 0);
			face2.TextureCoords[1] = new Vector2(0, 1);
			face2.TextureCoords[2] = new Vector2(1, 1);

			face1.Normals[0] = new Vector3(0, 0, 1);
			face1.Normals[1] = new Vector3(0, 0, 1);
			face1.Normals[2] = new Vector3(0, 0, 1);

			GenTriangleFace(face1);
			GenTriangleFace(face2);

			GenArrays();

			VAO = VAOLoader.LoadToVAO(Verticies, TextureCoords, Normals, Indicies);
		}

		

	}
}
