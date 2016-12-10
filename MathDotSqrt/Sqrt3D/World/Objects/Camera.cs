using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World.Objects {
	public abstract class Camera : Object3D {

		public Matrix4 InverseTransformationMatrix;
		public Matrix4 ProjectionMatrix;

		public virtual void UpdateInverseTransformationMatrix() {
			this.InverseTransformationMatrix = Matrix4.Identity;

			Object3D p = Parent;
			while(p != null) {
				if(!OmitParentTranslation)
					this.InverseTransformationMatrix *= Matrix4.CreateTranslation(-p.Position);

				if(!OmitParentRotation) {
					this.InverseTransformationMatrix *= Matrix4.CreateRotationZ(SexyMathHelper.ToRad(-p.Rotation.Z));
					this.InverseTransformationMatrix *= Matrix4.CreateRotationY(SexyMathHelper.ToRad(-p.Rotation.Y));
					this.InverseTransformationMatrix *= Matrix4.CreateRotationX(SexyMathHelper.ToRad(-p.Rotation.X));
				}
				
				p = p.Parent;
			}

			
			this.InverseTransformationMatrix *= Matrix4.CreateTranslation(-Position);
			this.InverseTransformationMatrix *= Matrix4.CreateRotationZ(SexyMathHelper.ToRad(Rotation.Z));
			this.InverseTransformationMatrix *= Matrix4.CreateRotationY(SexyMathHelper.ToRad(Rotation.Y));
			this.InverseTransformationMatrix *= Matrix4.CreateRotationX(SexyMathHelper.ToRad(Rotation.X));
		}
		public abstract void UpdateProjectionMatrix();

		public void MoveForward(float vel) {
			float rad = SexyMathHelper.ToRad(this.Rotation.Y);
			this.Position.X += vel * (float)Math.Sin(rad);
			this.Position.Z += vel * -(float)Math.Cos(rad);
		}
		public void MoveLeft(float vel) {
			float rad = SexyMathHelper.ToRad(this.Rotation.Y) + (float)Math.PI / 2;
			this.Position.X += vel * (float)Math.Sin(rad);
			this.Position.Z += vel * -(float)Math.Cos(rad);
		}
		


	}
}
