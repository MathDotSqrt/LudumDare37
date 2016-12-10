using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using MathDotSqrt.Sqrt3D.Util.Math;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.RenderEngine.Materials;
using MathDotSqrt.Sqrt3D.RenderEngine.Geometries;
using MathDotSqrt.Sqrt3D.RenderEngine.Shader;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using MathDotSqrt.Sqrt3D.World;
using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.RenderEngine.GUI;
using MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer;
using MathDotSqrt.Sqrt3D.Util;
using MathDotSqrt.Sqrt3D.RenderEngine.Framebuffer;

namespace MathDotSqrt.Sqrt3D.RenderEngine {
	
	public class OpenGLRenderer {
		
		public Color ClearColor;
		public bool DoPostProcessing = true;

		private FBO multiSampledFBO;
		private FBO resolvedFBO;

		public OpenGLRenderer() {
			GL.Enable(EnableCap.Multisample);

			ClearColor = Color.BlackCock;

			multiSampledFBO = new FBO(Window.WIDTH, Window.HEIGHT, true);
			resolvedFBO = new FBO(Window.WIDTH, Window.HEIGHT, false);
			PostProcessing.InitPostProcessing();
		}
		public void Prepare() {
			GL.Enable(EnableCap.DepthTest); 
											
			GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

			GL.Enable(EnableCap.CullFace);  
			GL.CullFace(CullFaceMode.Back); 

			GL.ClearColor(ClearColor.R, ClearColor.G, ClearColor.B, ClearColor.A);  
		}

		public void RenderScene(Scene scene) {
			if(scene == null) {
				Output.Error("Scene is null");
				return;
			}
			if(scene.GetAllObjects() == null || scene.GetAllObjects().Count == 0)
				return;
			if(scene.Camera == null)
				return;


			Camera camera = scene.Camera;

			if(camera.AutoUpdateTransformationMatrix) {
				camera.UpdateTransformationMatrix();
				camera.UpdateInverseTransformationMatrix();
			}

			foreach(Light light in scene.GetAllLights()) {
				if(light.AutoUpdateTransformationMatrix)
					light.UpdateTransformationMatrix();
			}

			if(DoPostProcessing) 
				multiSampledFBO.BindFrameBuffer();

			foreach(Mesh mesh in scene.GetAllMeshes()) {   
				if(!mesh.Visible)
					continue;

				GL.BindVertexArray(mesh.Geometry.VAO.Id);      

				if(mesh.AutoUpdateTransformationMatrix)
					mesh.UpdateTransformationMatrix();

				MaterialFace face = MaterialFace.Front;

				switch(mesh.Material.RenderFace) {
					case RenderFace.Front:
					GL.FrontFace(FrontFaceDirection.Ccw);
					face = MaterialFace.Front;
					break;
					case RenderFace.Back:
					GL.FrontFace(FrontFaceDirection.Cw);
					face = MaterialFace.Back;
					break;
					case RenderFace.FrontAndBack:
					GL.Disable(EnableCap.CullFace);
					face = MaterialFace.FrontAndBack;
					break;
				}

				switch(mesh.Material.RenderMode) {
					case RenderMode.Lines:
					GL.PolygonMode(face, PolygonMode.Line);
					break;
					case RenderMode.Points:
					GL.PolygonMode(face, PolygonMode.Point);
					break;
					case RenderMode.Fill:
					default:
					GL.PolygonMode(face, PolygonMode.Fill);
					break;
				}

				switch(mesh.Material.Type) {
					case MaterialType.MeshBasicMaterial:
					RenderBasic(mesh, camera);
					break;
					case MaterialType.MeshNormalMaterial:
					RenderNormal(mesh, camera);
					break;
					case MaterialType.MeshLitMaterial:
					RenderLit(mesh, camera, scene.GetSortedLights());
					break;
					case MaterialType.MeshSpecularMaterial:
					RenderSpecular(mesh, camera, scene.GetSortedLights());
					break;
					case MaterialType.MeshSkyboxMaterial:
					RenderSkybox(mesh, camera);
					break;
					default:
					Output.Warning("OpenGLRenderer.RenderScene unsuported material type " + mesh.Material.Type);
					break;
				}
				GL.BindVertexArray(0);
			}

			if(DoPostProcessing) {
				multiSampledFBO.UnbindCurrentFramebuffer();
				multiSampledFBO.ResolveToFBO(resolvedFBO);
				PostProcessing.RenderPostProcessingPipeLine(resolvedFBO, scene);
			}
		}

		public void RenderGui(GuiField gui) {
			if(gui == null)
				return;
			GL.Enable(EnableCap.Blend);
			GL.Disable(EnableCap.DepthTest);
			GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

			RenderGui(gui.GetAllParentElements());

			GL.Disable(EnableCap.Blend);
			GL.Enable(EnableCap.DepthTest);

		}
		private void RenderGui(List<GuiElement> parentElements) {
			foreach(GuiElement element in parentElements) {
				switch(element.Material.Type) {
					case MaterialType.GuiBasicMaterial:
					RenderBasicGui(element);
					break;
					case MaterialType.GuiFontMaterial:
					RenderFontGui(element);
					break;
					default:
					Output.Warning("OpenGLRenderer.RenderGui unsuported material type " + element.Material.Type);
					break;
				}

				if(element.Children != null && element.Children.Count > 0)
					RenderGui(element.Children);

				if(element.AutoUpdateTransformationMatrix)
					element.UpdateTransformationMatrix();
			}
		}


		//Everything below here gets (sexually) intimate with the GPU and becomes really difficult to understand.
		
			//MESH RENDERING
		private void RenderBasic(Mesh mesh, Camera camera) {
			Geometry geometry = mesh.Geometry;
			MeshBasicMaterial material = (MeshBasicMaterial)mesh.Material;
			BasicShader shader = (BasicShader)material.Shader;

			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Texture_UV);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Normal);

			shader.Start();

			shader.LoadTransformationMatrix(mesh.TransformationMatrix);
			shader.LoadCamera(camera);
			shader.LoadColor(material.Color);

			shader.ConnectTextures();
			shader.LoadTexture(material.Texture);
			shader.LoadReflectiveCubeMap(material.CubeMapReflection);
			shader.LoadRefractiveCubeMap(material.CubeMapRefraction, material.RefractionIndex);

			GL.DrawElements(BeginMode.Triangles, geometry.VAO.IndexCount, DrawElementsType.UnsignedInt, 0);

			shader.Stop();

			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Texture_UV);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Normal);
		}
		private void RenderNormal(Mesh mesh, Camera camera) {
			Geometry geometry = mesh.Geometry;
			MeshNormalMaterial material = (MeshNormalMaterial)mesh.Material;
			NormalShader shader = (NormalShader)material.Shader;

			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Normal);
			shader.Start();

			shader.LoadTransformationMatrix(mesh.TransformationMatrix);
			shader.LoadCamera(camera);

			GL.DrawElements(BeginMode.Triangles, geometry.VAO.IndexCount, DrawElementsType.UnsignedInt, 0);

			shader.Stop();
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Normal);
		}
		private void RenderLit(Mesh mesh, Camera camera, Dictionary<LightType, List<Light>> lights) {  //Its Lit
			Geometry geometry = mesh.Geometry;
			MeshLitMaterial material = (MeshLitMaterial)mesh.Material;
			LitShader shader = (LitShader)material.Shader;

			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Texture_UV);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Normal);
			shader.Start();

			shader.LoadTransformationMatrix(mesh.TransformationMatrix);
			shader.LoadCamera(camera);

			shader.LoadColor(material.Color);
			shader.ConnectTextures();
			shader.LoadTexture(material.Texture);
			shader.LoadReflectiveCubeMap(material.CubeMapReflection);
			shader.LoadRefractiveCubeMap(material.CubeMapRefraction, material.RefractionIndex);

			shader.LoadPointLights(lights[LightType.PointLight]);
			shader.LoadAmbientLights(lights[LightType.AmbientLight]);

			GL.DrawElements(BeginMode.Triangles, geometry.VAO.IndexCount, DrawElementsType.UnsignedInt, 0);

			shader.Stop();
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Texture_UV);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Normal);
		}
		private void RenderSpecular(Mesh mesh, Camera camera, Dictionary<LightType, List<Light>> lights) {  //Its Lit
			Geometry geometry = mesh.Geometry;
			MeshSpecularMaterial material = (MeshSpecularMaterial)mesh.Material;
			SpecularShader shader = (SpecularShader)material.Shader;

			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Texture_UV);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Normal);
			shader.Start();

			shader.LoadTransformationMatrix(mesh.TransformationMatrix);
			shader.LoadCamera(camera);
			
			shader.LoadColor(material.Color);
			shader.LoadSpecularColor(material.SpecularColor);
			shader.LoadSpecularShininess(material.Shininess);

			shader.ConnectTextures();
			shader.LoadTexture(material.Texture);
			shader.LoadReflectiveCubeMap(material.CubeMapReflection);
			shader.LoadRefractiveCubeMap(material.CubeMapRefraction, material.RefractionIndex);

			shader.LoadPointLights(lights[LightType.PointLight]);
			shader.LoadAmbientLights(lights[LightType.AmbientLight]);

			GL.DrawElements(BeginMode.Triangles, geometry.VAO.IndexCount, DrawElementsType.UnsignedInt, 0);

			shader.Stop();
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Texture_UV);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Normal);
		}
		private void RenderSkybox(Mesh mesh, Camera camera) {
			Geometry geometry = mesh.Geometry;
			MeshSkyboxMaterial material = (MeshSkyboxMaterial)mesh.Material;
			SkyboxShader shader = (SkyboxShader)material.Shader;

			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);
			shader.Start();

			shader.LoadTransformationMatrix(mesh.TransformationMatrix);
			shader.LoadCamera(camera);
			
			//shader.LoadColor(material.Color);
			shader.LoadSkyboxCubeMap(material.SkyboxCubeMap);

			GL.DrawElements(BeginMode.Triangles, geometry.VAO.IndexCount, DrawElementsType.UnsignedInt, 0);

			shader.Stop();
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
		}

		//GUI RENDERING STUFF
		private void RenderBasicGui(GuiElement element) {
			Geometry2d geometry = element.Geometry;
			GuiBasicMaterial material = (GuiBasicMaterial)element.Material;
			GuiBasicShader shader = (GuiBasicShader)element.Material.Shader;
			GL.BindVertexArray(geometry.VAO.Id);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);

			shader.Start();


			shader.LoadTransformation(element.TransformationMatrix);
			shader.LoadTexture(material.Texture);
			shader.LoadColor(material.Color);

			GL.DrawArrays(PrimitiveType.TriangleStrip, 0, geometry.VAO.IndexCount);
			//GL.DrawElements(BeginMode.Triangles, geometry.VAO.IndexCount, DrawElementsType.UnsignedInt, 0);


			shader.Stop();

			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.BindVertexArray(0);
		}
		private void RenderFontGui(GuiElement element) {
			Geometry2d geometry = element.Geometry;
			GuiFontMaterial material = (GuiFontMaterial)element.Material;
			GuiFontShader shader = (GuiFontShader)element.Material.Shader;
			GL.BindVertexArray(geometry.VAO.Id);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.EnableVertexAttribArray((int)VAOAttribLocation.Texture_UV);

			shader.Start();
			
			shader.LoadTransformation(element.TransformationMatrix);
			shader.ConnectTextures();
			shader.LoadTexture(material.Texture);
			shader.LoadColor(material.Color);

			GL.DrawElements(BeginMode.Triangles, geometry.VAO.IndexCount, DrawElementsType.UnsignedInt, 0);
			//GL.DrawArrays(PrimitiveType.TriangleStrip, 0, geometry.VAO.IndexCount);

			shader.Stop();

			GL.DisableVertexAttribArray((int)VAOAttribLocation.Position);
			GL.DisableVertexAttribArray((int)VAOAttribLocation.Texture_UV);

			GL.BindVertexArray(0);
		}
	}

}
