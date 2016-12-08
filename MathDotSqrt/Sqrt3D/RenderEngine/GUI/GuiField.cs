using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.GUI {
	public class GuiField {
		private List<GuiElement> elements;

		public GuiField() {
			elements = new List<GuiElement>();
		}

		public void Add(GuiElement element) {
			elements.Add(element);
		}

		public GuiElement GetElement(string name) {
			foreach(GuiElement element in elements) {
				if(element.Name == name)
					return element;
			}

			return null;
		}
		public List<GuiElement> GetAllElements() {
			return elements;
		}
		public List<GuiElement> GetAllParentElements() {
			List<GuiElement> parentElements = new List<GuiElement>();

			foreach(GuiElement element in elements) {
				if(element.Parent == null)
					parentElements.Add(element);
			}

			return parentElements;
		}

		public void UpdateScreenSize() {
			foreach(GuiElement element in elements) {
				element.UpdateScreenSize();
			}
		}
	}
}
