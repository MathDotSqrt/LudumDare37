using MathDotSqrt.Sqrt3D.RenderEngine;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.Util.IO.Loader {
	public static class VAOLoader {
		private static List<int> VAOs = new List<int>();
		private static List<int> VBOs = new List<int>();

		public static VAO LoadToVAO(float[] positions, int dimension) {
			int VAO_ID = CreateVAO();

			BindVAO(VAO_ID);
			BufferToAttrList(VAOAttribLocation.Position, positions, dimension);
			UnbindVAO();

			return new VAO(VAO_ID, (int)( positions.Length / dimension ));
		}
		public static VAO LoadToVAO(float[] positions, float[] textureUV, int dimension = 3) {
			int VAO_ID = CreateVAO();

			BindVAO(VAO_ID);
			BufferToAttrList(VAOAttribLocation.Position, positions, dimension);
			BufferToAttrList(VAOAttribLocation.Texture_UV, textureUV, 2);
			UnbindVAO();

			return new VAO(VAO_ID, (int)( positions.Length / dimension ));
		}
		public static VAO LoadToVAO(float[] positions, int[] indicies, int dimension = 3) {
			int VAO_ID = CreateVAO();
			BindVAO(VAO_ID);

			BindIndicies(indicies);
			BufferToAttrList(VAOAttribLocation.Position, positions, dimension);

			UnbindVAO();
			return new VAO(VAO_ID, indicies.Length);
		}
		public static VAO LoadToVAO(float[] positions, float[] textureUV, int[] indicies, int dimension = 3) {
			int VAO_ID = CreateVAO();
			BindVAO(VAO_ID);

			BindIndicies(indicies);
			BufferToAttrList(VAOAttribLocation.Position, positions, dimension);
			BufferToAttrList(VAOAttribLocation.Texture_UV, textureUV, 2);

			UnbindVAO();
			return new VAO(VAO_ID, indicies.Length);
		}
		public static VAO LoadToVAO(float[] positions, float[] textureUV, float[] normals, int[] indicies) {
			int VAO_ID = CreateVAO();
			BindVAO(VAO_ID);

			BindIndicies(indicies);
			BufferToAttrList(VAOAttribLocation.Position, positions, 3);
			BufferToAttrList(VAOAttribLocation.Texture_UV, textureUV, 2);
			BufferToAttrList(VAOAttribLocation.Normal, normals, 3);

			UnbindVAO();
			return new VAO(VAO_ID, indicies.Length);
		}

		private static void BindIndicies(int[] indicies) {
			int VBO = GL.GenBuffer();
			VBOs.Add(VBO);

			GL.BindBuffer(BufferTarget.ElementArrayBuffer, VBO);
			GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)( indicies.Length * sizeof(uint) ), indicies, BufferUsageHint.StaticDraw);
		}
		private static void BufferToAttrList(VAOAttribLocation attrNumber, float[] data, int size) {
			int VBO = GL.GenBuffer();
			VBOs.Add(VBO);

			GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
			GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)( data.Length * sizeof(float) ), data, BufferUsageHint.StaticDraw);
			GL.VertexAttribPointer((int)attrNumber, size, VertexAttribPointerType.Float, false, 0, 0);
			GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
		}

		private static int CreateVAO() {
			int VAO = GL.GenVertexArray();
			VAOs.Add(VAO);
			return VAO;
		}
		private static void BindVAO(int VAO) {
			GL.BindVertexArray(VAO);
		}
		private static void UnbindVAO() {
			GL.BindVertexArray(0);
		}

		public static void Dispose() {
			foreach(int VAO in VAOs) {
				GL.DeleteVertexArray(VAO);
			}

			foreach(int VBO in VBOs) {
				GL.DeleteBuffer(VBO);
			}
		}
	}
}
