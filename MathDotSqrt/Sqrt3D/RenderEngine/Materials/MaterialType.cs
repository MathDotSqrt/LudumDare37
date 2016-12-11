using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.RenderEngine.Materials {
	[Flags]
	public enum MaterialType {
		NullMaterial = -1,
		MeshBasicMaterial = 0,
		MeshNormalMaterial = 1,
		MeshLitMaterial = 2,
		MeshSpecularMaterial = 3,
		MeshSkyboxMaterial = 4,
		MeshBumpMaterial = 5,

		GuiBasicMaterial = 10,
		GuiFontMaterial = 11
	}

	[Flags]
	public enum RenderMode {
		Fill = 0,
		Lines = 1,
		Points = 2
	}

	[Flags]
	public enum RenderFace {
		Front = 0,
		Back = 1,
		FrontAndBack = 2
	}
}
