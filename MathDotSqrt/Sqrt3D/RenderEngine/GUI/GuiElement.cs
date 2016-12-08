using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.Util.Math;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.GUI {
	public class GuiElement {
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

		public GuiElement Parent {
			get;
			set;
		}
		public List<GuiElement> Children {
			get;
			set;
		}
		public Geometry2d Geometry {
			get;
			set;
		}
		public Material Material {
			get;
			set;
		}
		public Matrix4 TransformationMatrix {
			get;
			set;
		}

		public Vector2 Translation;
		public float Rotation;
		public Vector2 Scale;

		//OpenGL's coordinates are from X: -1 to 1 |  Y: -1 to 1 

		private float SCREEN_WIDTH = Window.WIDTH;
		private float SCREEN_HEIGHT = Window.HEIGHT;
		private float PIXEL_WIDTH = 1f / Window.WIDTH;
		private float PIXEL_HEIGHT = 1f / Window.HEIGHT;
		private float PARENT_WIDTH = Window.WIDTH;
		private float PARENT_HEIGHT = Window.HEIGHT;

		private float pixelX;
		private float pixelY;
		private float percentX;
		private float percentY;

		private float width;
		private float height;
		private float percentWidth;
		private float percentHeight;
		private float quadPercentWidth;
		private float quadPercentHeight;

		private float pixelMarginLeft;
		private float pixelMarginRight;
		private float pixelMarginTop;
		private float pixelMarginBottom;
		
		private bool boundMarginLeft;
		private bool boundMarginRight;
		private bool boundMarginTop;
		private bool boundMarginBottom;

		public float PixelX {
			get {
				return pixelX;
			}
			set {
				ScaledX = false;

				pixelX = value;
				percentX = pixelX / PARENT_WIDTH;

				Translation.X = 2 * pixelX * PIXEL_WIDTH - PARENT_WIDTH * PIXEL_WIDTH;
			}
		}
		public float PixelY {
			get {
				return pixelY;
			}
			set {
				ScaledY = false;

				pixelY = value;
				percentY = pixelY / PARENT_HEIGHT;

				Translation.Y = 2 * -pixelY * PIXEL_HEIGHT + PARENT_HEIGHT * PIXEL_HEIGHT;
			}
		}
		public float PercentX {
			get {
				return percentX;
			}
			set {
				if(value < 0)
					return;

				PixelX = value * PARENT_WIDTH;
				ScaledX = true;
			}
		}
		public float PercentY {
			get {
				return percentY;
			}
			set {
				if(value < 0)
					return;

				PixelY = value * PARENT_HEIGHT;
				ScaledY = true;
			}
		}
		public bool ScaledX {
			get;
			set;
		}
		public bool ScaledY {
			get;
			set;
		}

		public float PixelWidth {
			get {
				return width;
			}
			set {
				if(width < 0)
					return;

				ScaledWidth = false;

				width = value;
				percentWidth = width / PARENT_WIDTH;
				UpdateMargins();

				Scale.X = width / SCREEN_WIDTH / quadPercentWidth;
			}
		}
		public float PixelHeight {
			get {
				return height;
			}
			set {
				if(height < 0)
					return;
				ScaledHeight = false;

				height = value;
				percentHeight = height / PARENT_HEIGHT;
				UpdateMargins();

				Scale.Y = height / SCREEN_HEIGHT / quadPercentHeight;
			}
		}
		public float PercentWidth {
			get {
				return percentWidth;
			}
			set {
				if(value < 0)
					return;

				PixelWidth = value * PARENT_WIDTH;
				ScaledWidth = true;
			}
		}
		public float PercentHeight {
			get {
				return percentHeight;
			}
			set {
				if(value < 0)
					return;

				PixelHeight = value * PARENT_HEIGHT;
				ScaledHeight = true;
			}
		}
		public bool ScaledWidth {
			get;
			set;
		}
		public bool ScaledHeight {
			get;
			set;
		}

		public float PixelMarginLeft {
			get {
				return pixelMarginLeft;
			}
			set {
				pixelMarginLeft = value;
				BoundMarginLeft = true;

				PixelX = pixelMarginLeft + width / 2;
			}
		}
		public float PixelMarginRight {
			get {
				return pixelMarginRight;
			}
			set {
				pixelMarginRight = value;
				BoundMarginRight = true;

				PixelX = PARENT_WIDTH - pixelMarginRight - width / 2;
			}
		}
		public float PixelMarginTop {
			get {
				return pixelMarginTop;
			}
			set {
				pixelMarginTop = value;
				BoundMarginTop = true;

				PixelY = pixelMarginTop + height / 2;
			}
		}
		public float PixelMarginBottom {
			get {
				return pixelMarginBottom;
			}
			set {
				pixelMarginBottom = value;
				BoundMarginBottom = true;

				PixelY = PARENT_HEIGHT - pixelMarginBottom - height / 2;
			}
		}

		public bool BoundMarginLeft {
			get {
				return boundMarginLeft;
			}
			set {
				if(value) {
					boundMarginLeft = true;
					boundMarginRight = false;
				}
				else {
					BoundMarginLeft = false;
				}
			}
		}
		public bool BoundMarginRight {
			get {
				return boundMarginRight;
			}
			set {
				if(value) {
					boundMarginRight = true;
					boundMarginLeft = false;
				}
				else {
					boundMarginRight = false;
				}
			}
		}
		public bool BoundMarginTop {
			get {
				return boundMarginTop;
			}
			set {
				if(value) {
					boundMarginTop = true;
					boundMarginBottom = false;
				}
				else {
					boundMarginTop = false;
				}
			}
		}
		public bool BoundMarginBottom {
			get {
				return boundMarginBottom;
			}
			set {
				if(value) {
					boundMarginBottom = true;
					boundMarginTop = false;
				}
				else {
					boundMarginBottom = false;
				}
			}
		}

		public GuiElement(Geometry2d geometry, Material material) : this(null, geometry, material){
			
		}
		public GuiElement(GuiElement parent, Geometry2d geometry, Material material) {
			AutoUpdateTransformationMatrix = true;

			Translation = new Vector2(0, 0);
			Rotation = 0;
			Scale = new Vector2(1, 1);

			Geometry = geometry;
			Material = material;

			quadPercentWidth = geometry.BoundingWidth / 2;
			quadPercentHeight = geometry.BoundingHeight / 2;

			if(parent != null) {
				parent.Add(this);
			}

			PixelWidth = quadPercentWidth * SCREEN_WIDTH;
			PixelHeight = quadPercentHeight * SCREEN_HEIGHT;

			Center();
		}

		public void UpdateTransformationMatrix() {
			TransformationMatrix = Matrix4.Identity;
			TransformationMatrix *= Matrix4.CreateScale(Scale.X, Scale.Y, 0);
			TransformationMatrix *= Matrix4.CreateRotationZ(SexyMathHelper.ToRad(Rotation));
			TransformationMatrix *= Matrix4.CreateTranslation(Translation.X, Translation.Y, 0);

			GuiElement p = this.Parent;
			while(p != null) {
				TransformationMatrix *= Matrix4.CreateRotationZ(SexyMathHelper.ToRad(p.Rotation));
				TransformationMatrix *= Matrix4.CreateTranslation(new Vector3(p.Translation.X, p.Translation.Y, 0));

				p = p.Parent;
			}
		}

		public void Add(GuiElement element) {
			if(element == null) {
				Output.Error("GuiElement.Add could not add null element");
				return;
			}
			if(element == this) {
				Output.Error("GuiElement.Add could not add itself as child");
				return;
			}

			if(Children == null)
				Children = new List<GuiElement>();

			if(Children.Contains(element)) {
				Output.Error("GuiElement.Add could not add duplicate of child to parent");
				return;
			}

			this.Children.Add(element);

			element.Parent = this;
			element.PARENT_WIDTH = this.width;
			element.PARENT_HEIGHT = this.height;

			element.Update();

		}
		public bool Remove(GuiElement element, bool recursive = false) {
			if(element == null) {
				Output.Warning("GuiElement.Remove: GuiElement can not remove [null]");
				return false;
			}

			bool hasRemoved = false;

			for(int i = Children.Count - 1; i >= 0; i++) {
				if(recursive)
					Children[i].Remove(element, true);

				if(element == Children[i]) {
					Children[i].Parent = null;
					Children.RemoveAt(i);

					hasRemoved = true;
				}
			}

			element.Update();

			return hasRemoved;

		}
		public bool Contains(GuiElement element, bool recursive = false) {
			if(Children == null)
				return false;
			foreach(GuiElement child in Children) {
				if(element == child)
					return true;
				if(recursive)
					return child.Contains(element, true);
			}

			return false;
		}

		public void Update() {
			if(Parent != null) {
				PARENT_WIDTH = Parent.PixelWidth;
				PARENT_HEIGHT = Parent.PixelHeight;
			}
			if(ScaledX)
				PercentX = percentX;
			else
				PixelX = pixelX;
			if(ScaledY)
				PercentY = percentY;
			else
				PixelY = pixelY;

			if(ScaledWidth)
				PercentWidth = percentWidth;
			else
				PixelWidth = width;
			if(ScaledHeight)
				PercentHeight = percentHeight;
			else
				PixelHeight = height;
		}
		public void UpdateAllChildren() {
			if(Children == null || Children.Count == 0)
				return;

			foreach(GuiElement child in Children) {
				child.Update();
				child.UpdateAllChildren();
			}
		}

		public void Center(bool scaleToParent = true) {
			CenterX(scaleToParent);
			CenterY(scaleToParent);
		}
		public void CenterX(bool scaleToParent = true) {
			if(scaleToParent)
				PercentX = .5f;
			else
				PixelX = PARENT_WIDTH / 2;
		}
		public void CenterY(bool scaleToParent = true) {
			if(scaleToParent)
				PercentY = .5f;
			else
				PixelY = PARENT_HEIGHT / 2;
		}

		public void SetScale(float scale, bool scaleToParent = true) {
			SetScaleX(scale, scaleToParent);
			SetScaleY(scale, scaleToParent);
		}
		public void SetScaleX(float scaleX, bool scaleToParent = true) {
			if(scaleToParent)
				PercentWidth = .5f;
			else
				PixelWidth = PARENT_WIDTH / 2;
		}
		public void SetScaleY(float scaleY, bool scaleToParent = true) {
			if(scaleToParent)
				PercentHeight = .5f;
			else
				PixelHeight = PARENT_HEIGHT / 2;
		}

		public void UpdateScreenSize() {
			SCREEN_WIDTH = Window.WIDTH;
			SCREEN_HEIGHT = Window.HEIGHT;

			PIXEL_WIDTH = 1f / Window.WIDTH;
			PIXEL_HEIGHT = 1f / Window.HEIGHT;

			if(Parent == null) {
				PARENT_WIDTH = Window.WIDTH;
				PARENT_HEIGHT = Window.HEIGHT;
			}

			this.Update();
		}

		public void MatchParentDimensions(bool scaleToParent = true) {
			if(scaleToParent) {
				this.PercentWidth = Parent.PercentWidth;
				this.PercentHeight = Parent.PercentHeight;
			}
			else {
				this.PixelWidth = Parent.PixelWidth;
				this.PixelHeight = Parent.PixelHeight;
			}
		}


		private void UpdateMargins() {
			if(BoundMarginLeft)
				PixelMarginLeft = pixelMarginLeft;
			if(BoundMarginRight)
				PixelMarginRight = pixelMarginRight;
			if(BoundMarginTop)
				PixelMarginTop = pixelMarginTop;
			if(BoundMarginBottom)
				PixelMarginBottom = pixelMarginBottom;
		}
	}
}
