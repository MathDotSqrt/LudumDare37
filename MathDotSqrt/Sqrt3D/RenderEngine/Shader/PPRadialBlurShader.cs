using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Shader {
	public class PPRadialBlurShader : ShaderProgram {
		private const string SHADER_PATH = @"radialBlurShader\";
		private const string VERTEX_SHADER = SHADER_PATH + @"radialBlurVertexShader.vs";
		private const string FRAGMENT_SHADER = SHADER_PATH + @"radialBlurFragmentShader.fs";

		public PPRadialBlurShader() : base(VERTEX_SHADER, FRAGMENT_SHADER) {

		}

		protected override void BindAllVertexArrayAttrs() {
			base.BindVertexArrayAttr(VAOAttribLocation.Position, "position");
		}

		protected override void BindAllUniformLocations() {
			base.AddUniform("lightPositionOnScreen");
		}

		public void BindColorTexture(int colorTexture) {
			GL.ActiveTexture(TextureUnit.Texture0);
			GL.BindTexture(TextureTarget.Texture2D, colorTexture);
		}

		public void BindLightPositionOnScreen(Object3D obj, Camera camera) {
			Vector3 position = obj.Position;

			position = Vector3.Transform(position, camera.InverseTransformationMatrix.ClearTranslation());
			position = Vector3.Transform(position, camera.ProjectionMatrix);

			position.X /= position.Z;
			position.Y /= position.Z;
			position.X = (( position.X + 1 ) * Window.WIDTH / 2) / Window.WIDTH;
			position.Y = (( position.Y + 1 ) * Window.HEIGHT / 2) / Window.HEIGHT;

			base.LoadVector2(uniforms["lightPositionOnScreen"], position.Xy);
		}
	}
}
