using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using MathDotSqrt.Sqrt3D.Util.IO;
using OpenTK;
using MathDotSqrt.Sqrt3D.Util;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	/// <summary>
	/// Handels binding vertex and fragment shaders to the GPU and loading uniforms to the shader program
	/// </summary>
	public abstract class ShaderProgram {
		private int programID;
		private int vertexShaderID;
		private int fragmentShaderID;

		protected Dictionary<string, int> uniforms = new Dictionary<string, int>();

		public ShaderProgram(string vertexShader, string fragmentShader) {
			vertexShaderID = LoadShader(vertexShader, ShaderType.VertexShader);
			fragmentShaderID = LoadShader(fragmentShader, ShaderType.FragmentShader);

			programID = GL.CreateProgram();
			GL.AttachShader(programID, vertexShaderID);
			GL.AttachShader(programID, fragmentShaderID);

			GL.LinkProgram(programID);
			GL.ValidateProgram(programID);

			BindAllVertexArrayAttrs();
			BindAllUniformLocations();
		}

		protected abstract void BindAllUniformLocations();
		protected abstract void BindAllVertexArrayAttrs();

		public void Start() {
			GL.UseProgram(programID);
		}
		public void Stop() {
			GL.UseProgram(0);
		}
		public void Dispose() {
			this.Stop();
			GL.DetachShader(programID, vertexShaderID);
			GL.DetachShader(programID, fragmentShaderID);

			GL.DeleteShader(vertexShaderID);
			GL.DeleteShader(fragmentShaderID);
			GL.DeleteProgram(programID);
		}

		protected int LoadShader(string file, ShaderType type) {
			string shaderSource = "";

			try {
				string path = FilePaths.SHADER_RELATIVE + file;
				shaderSource = System.IO.File.ReadAllText(FilePaths.SHADER_RELATIVE + file);
			}
			catch(Exception e) {
				Output.Warning("[ERROR] Could not read shader from source: " + file);
				Output.Error(e.Message);
			}

			int shaderID = GL.CreateShader(type);
			GL.ShaderSource(shaderID, shaderSource);
			GL.CompileShader(shaderID);

			int param;

			GL.GetShader(shaderID, ShaderParameter.CompileStatus, out param);

			if(param == 0) {
				string output;
				GL.GetShaderInfoLog(shaderID, out output);
				Output.Warning("Could not compile " + type.ToString() + " shader from " + file);
				Output.Error(output);
			}

			return shaderID;
		}

		protected void BindVertexArrayAttr(VAOAttribLocation attr, string name) {
			GL.BindAttribLocation(programID, (int)attr, name);
		}

		protected void LoadBool(int location, bool value) {
			if(value) {
				LoadInt(location, 1);
			}
			else {
				LoadInt(location, 0);
			}
		}
		protected void LoadBoolArray(int location, bool[] values) {
			int[] intValue = new int[values.Length];

			for(int i = 0; i < values.Length; i++) {
				if(values[i]) {
					intValue[i] = 1;
				}
				else {
					intValue[i] = 0;
				}
			}

			LoadIntArray(location, intValue);
		}
		protected void LoadInt(int location, int value) {
			GL.Uniform1(location, value);
		}
		protected void LoadIntArray(int location, int[] values) {
			GL.Uniform1(location, values.Length, values);
		}
		protected void LoadFloat(int location, float value) {
			GL.Uniform1(location, value);
		}
		protected void LoadFloatArray(int location, float[] values) {
			GL.Uniform1(location, values.Length, values);
		}
		protected void LoadVector3(int location, Vector3 value) {
			GL.Uniform3(location, value);
		}
		protected void LoadVector3Array(int location, Vector3[] values) {
			float[] vectorFloatArray = ArrayConverter.ConvertVectorArrayToFloatArray(values);
			GL.Uniform3(location, vectorFloatArray.Length, vectorFloatArray);
		}
		protected void LoadVector3Array(int location, float[] values) {
			GL.Uniform3(location, values.Length, values);
		}
		protected void LoadVector4(int location, Vector4 value) {
			GL.Uniform4(location, value);
		}
		protected void LoadMatrix3(int location, Matrix3 value) {
			GL.UniformMatrix3(location, false, ref value);
		}
		protected void LoadMatrix4(int location, Matrix4 value) {
			GL.UniformMatrix4(location, false, ref value);
		}

		protected void AddUniform(string uniformName) {
			uniforms.Add(uniformName, GetUniformLocation(uniformName));
		}
		protected int GetUniformLocation(string uniformName) {
			int uniformID = GL.GetUniformLocation(programID, uniformName);
			if(uniformID < 0) {
				Output.Error("[GetUniformLocation] Error could not find uniform named: " + uniformName);
				return -1;
			}
			return uniformID;
		}
	}
}
