using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK;
using OpenTK.Graphics.ES10;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World {

	public abstract class Object3D {

		public string Name {
			get;
			set;
		}
		public Guid GUID {
			get;
			private set;
		}

		public bool Visible {
			get;
			set;
		}
		public bool AutoUpdateTransformationMatrix {
			get;
			set;
		}
		public bool NeedUpdateTransformationMatrix {
			get;
			set;
		}

		public Object3D Parent {
			get;
			private set;
		}
		public List<Object3D> Children {
			get;
			private set;
		}
		public bool OmitParentTranslation {
			get;
			set;
		}
		public bool OmitParentRotation {
			get;
			set;
		}
		public bool OmitParentScale {
			get;
			set;
		}

		public Matrix4 TransformationMatrix;

		public Vector3 Position;
		public Vector3 Rotation;
		public Vector3 Scale;

		public Vector3 WorldSpacePostion {
			get { return TransformationMatrix.ExtractTranslation(); }
		}
		public Vector3 WorldSpaceRotation {
			get {
				Output.Error("Object3D.WorldSpaceRotation: Rotation is not world space");
				Quaternion q = TransformationMatrix.ExtractRotation();
				Vector3 rotation = new Vector3();
				float angle;
				q.ToAxisAngle(out rotation, out angle);

				Output.WriteLine(rotation.ToString());
				Output.WriteLine(angle.ToString());

				return Rotation;
			}
		}
		public Vector3 WorldSpaceScale {
			get { return TransformationMatrix.ExtractScale(); }
		}

		public Vector3 PositionVelocity;
		public Vector3 RotationVelocity;
		public Vector3 ScaleVelocity;

		public Object3D() {
			GUID = Guid.NewGuid();
			Visible = true;
			AutoUpdateTransformationMatrix = true;

			TransformationMatrix = new Matrix4();
			Position = new Vector3(0, 0, 0);
			Rotation = new Vector3(0, 0, 0);
			Scale = new Vector3(1, 1, 1);
		}
		
		public virtual void UpdateTransformationMatrix() {
			TransformationMatrix = Matrix4.Identity;
			TransformationMatrix *= Matrix4.CreateScale(Scale);

			TransformationMatrix *= Matrix4.CreateRotationX(SexyMathHelper.ToRad(Rotation.X));
			TransformationMatrix *= Matrix4.CreateRotationY(SexyMathHelper.ToRad(Rotation.Y));
			TransformationMatrix *= Matrix4.CreateRotationZ(SexyMathHelper.ToRad(Rotation.Z));

			TransformationMatrix *= Matrix4.CreateTranslation(Position);


			Object3D p = this.Parent;
			while(p != null) {

				if(!OmitParentScale)
					TransformationMatrix *= Matrix4.CreateScale(p.Scale);

				if(!OmitParentRotation){
					TransformationMatrix *= Matrix4.CreateRotationX(SexyMathHelper.ToRad(p.Rotation.X));
					TransformationMatrix *= Matrix4.CreateRotationY(SexyMathHelper.ToRad(p.Rotation.Y));
					TransformationMatrix *= Matrix4.CreateRotationZ(SexyMathHelper.ToRad(p.Rotation.Z));
				}
				if(!OmitParentTranslation)
					TransformationMatrix *= Matrix4.CreateTranslation(p.Position);

				

				p = p.Parent;
			}
		}

		public void Add(Object3D obj) {
			if(obj == null) {
				Output.Warning("Object3D.Add: Object3D can't add [null] as a child");
				return;
			}
			if(obj == this) {
				Output.Warning("Object3D.Add: Object3D can't be added as a child of itself");
				return;
			}

			if(Children == null)
				Children = new List<Object3D>();
			if(Children.Contains(obj))
				return;

			Children.Add(obj);

			if(obj.Parent != null)
				obj.Parent.Remove(obj);

			obj.Parent = this;
		}
		public bool Remove(Object3D obj, bool recursive = false) {
			if(obj == null) {
				Output.Warning("Object.Remove: Object3D can not remode [null]");
				return false;
			}

			bool hasRemoved = false;

			for(int i = Children.Count - 1; i >= 0; i++) {
				if(recursive)
					Children[i].Remove(obj, true);

				if(obj == Children[i]) {
					Children[i].Parent = null;
					Children.RemoveAt(i);

					hasRemoved = true;
				}
			}

			return hasRemoved;
		}
		public bool Contains(Object3D obj, bool recursive = false) {
			if(Children == null)
				return false;

			foreach(Object3D child in Children) {
				if(obj == child)
					return true;
				if(recursive)
					return child.Contains(obj, true);
			}

			return false;
		}

		public void SetPosition(float x, float y, float z) {
			this.Position = new Vector3(x, y, z);
		}
		public void SetRotation(float rx, float ry, float rz) {
			this.Rotation = new Vector3(rx, ry, rz);
		}
		public void SetScale(float scale) {
			this.Scale = new Vector3(scale, scale, scale);
		}
		public void SetScale(float x, float y, float z) {
			this.Scale = new Vector3(x, y, z);
		}

		public void Translate() {
			Position += PositionVelocity;
		}
		public void Translate(float x, float y, float z) {
			Position.X += x;
			Position.Y += y;
			Position.Z += z;
		}
		public void TranslateX(float x) {
			Position.X += x;
		}
		public void TranslateY(float y) {
			Position.Y += y;
		}
		public void TranslateZ(float z) {
			Position.Z += z;
		}

		public void Rotate() {
			Rotation += RotationVelocity;
		}
		public void Rotate(float rx, float ry, float rz) {
			Rotation.X += rx;
			Rotation.Y += ry;
			Rotation.Z += rz;
		}
		public void RotateX(float rx) {
			Rotation.X += rx;
		}
		public void RotateY(float ry) {
			Rotation.Y += ry;
		}
		public void RotateZ(float rz) {
			Rotation.Z += rz;
		}

		public void ScaleUp() {
			Scale += ScaleVelocity;
		}
		public void ScaleUp(float scale) {
			Scale.X += scale;
			Scale.Y += scale;
			Scale.Z += scale;
		}
		public void ScaleUp(float x, float y, float z) {
			Scale.X += x;
			Scale.Y += y;
			Scale.Z += z;
		}
		public void ScaleUpX(float x) {
			Scale.X += x;
		}
		public void ScaleUpY(float y) {
			Scale.Y += y;
		}
		public void ScaleUpZ(float z) {
			Scale.Z += z;
		}

		public void SetPositionVel(float x, float y, float z) {
			PositionVelocity = new Vector3(x, y, z);
		}
		public void SetRotationVel(float rx, float ry, float rz) {
			RotationVelocity = new Vector3(rx, ry, rz);
		}
		public void SetScaleVel(float x, float y, float z) {
			ScaleVelocity = new Vector3(x, y, z);
		}

		public override string ToString() {
			string output = "";
			output += "-----Object3D-----\r\n";
			output += "Name: " + this.Name + "\r\n";
			output += "Guid: " + this.GUID.ToString() + "\r\n";
			output += "Position: " + this.Position + "\r\n";
			output += "Rotation: " + this.Rotation + "\r\n";
			output += "Scale: " + this.Scale + "\r\n";
			return output;
		}

		public abstract void Dispose();
	}
}
