using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World.Objects.Cameras {
	public class PerspectiveCamera : Camera {

		private float fov;
		private float aspectRatio;
		private float nearPlane;
		private float farPlane;

		public float FOV {
			get { return fov; }
			set {
				if(value > 0 && value < 180) {
					fov = value;
					UpdateProjectionMatrix();
				}
				else {
					Output.Warning("[Camera] could not set fov: " + value);
				}
			}
		}
		public float AspectRatio {
			get { return aspectRatio; }
			set {
				if(value > 0) {
					aspectRatio = value;
					UpdateProjectionMatrix();
				}
				else {
					Output.Warning("[Camera] could not set aspect ratio: " + value);
				}
			}
		}
		public float NearPlane {
			get { return nearPlane; }
			set {
				if(value > 0 && value < farPlane) {
					nearPlane = value;
					UpdateProjectionMatrix();
				}
				else {
					Output.Warning("[Camera] could not set near: " + value);
				}
			}
		}
		public float FarPlane {
			get { return farPlane; }
			set {
				if(farPlane > nearPlane) {
					farPlane = value;
					UpdateProjectionMatrix();
				}
				else {
					Output.Warning("[Camera] could not set far: " + value);
				}
			}
		}

		public PerspectiveCamera(float fov, float aspectRatio, float nearPlane, float farPlane) {
			this.fov = fov;
			this.aspectRatio = aspectRatio;
			this.nearPlane = nearPlane;
			this.farPlane = farPlane;

			this.OmitParentScale = true;
			this.UpdateProjectionMatrix();
		}

		public override void UpdateProjectionMatrix() {
			float yScale = (float)( ( 1f / Math.Tan(SexyMathHelper.ToRad(FOV / 2f)) ));
			float xScale = yScale / aspectRatio;
			float frustumLength = FarPlane - NearPlane;

			this.ProjectionMatrix = new Matrix4();
			ProjectionMatrix.M11 = xScale;
			ProjectionMatrix.M22 = yScale;
			ProjectionMatrix.M33 = -( ( FarPlane + NearPlane ) / frustumLength );
			ProjectionMatrix.M34 = -1;
			ProjectionMatrix.M43 = -( ( 2 * NearPlane * FarPlane ) / frustumLength );
			ProjectionMatrix.M44 = 0;
		}

		public override void Dispose() {

		}
	}
}
