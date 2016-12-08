using MathDotSqrt.Sqrt3D.Util.IO;
using MathDotSqrt.Sqrt3D.World.Objects;
using MathDotSqrt.Sqrt3D.World.Objects.Lights;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathDotSqrt.Sqrt3D.World {
	/// <summary>
	/// Stores all the Object3Ds in 3D world space.
	/// </summary>
	public class Scene {

		private List<Object3D> objects;

		public Camera Camera {
			get;
			private set;
		}

		private List<Mesh> meshes;
		private List<AudioSource> sources;
		private List<Light> lights;
		private Dictionary<LightType, List<Light>> sortedLights;


		public Scene() {
			InitScene();
		}
		public Scene(Camera camera) {
			this.Camera = camera;

			InitScene();
		}
		private void InitScene() {
			objects = new List<Object3D>();
			meshes = new List<Mesh>();
			sources = new List<AudioSource>();
			lights = new List<Light>();
			sortedLights = new Dictionary<LightType, List<Light>>();

			foreach(LightType type in Enum.GetValues(typeof(LightType))) {
				sortedLights.Add(type, new List<Light>());
			}
		}

		public void Add(Object3D obj) {
			objects.Add(obj);

			if(obj is Camera)
				AddCamera((Camera)obj);
			else if(obj is Mesh)
				AddMesh((Mesh)obj);
			else if(obj is AudioSource)
				AddSource((AudioSource) obj);
			else if(obj is Light)
				AddLight((Light)obj);
		}

		public bool Remove(string name) {
			return Remove(GetObject(name));
		}
		public bool Remove(Guid guid) {
			return Remove(GetObject(guid));
		}
		public bool Remove(Object3D obj) {
			if(obj == null)
				return false;

			objects.Remove(obj);

			if(obj is Mesh)
				return RemoveMesh((Mesh)obj);
			else if(obj is AudioSource)
				return RemoveSource((AudioSource)obj);
			else if(obj is Light)
				return RemoveLight((Light)obj);

			Output.Warning("Object3D type not recognised");
			return false;
		}
		public bool RemoveAll(string name) {
			List<Object3D> objList = GetAllObjects(name);
			bool hasRemoved = false;

			foreach(Object3D obj in objList) {
				hasRemoved |= Remove(obj);
			}

			return hasRemoved;
		}

		public Object3D GetObject() {
			if(objects.Count <= 0)
				return null;

			return objects[0];
		}
		public Object3D GetObject(int index) {
			if(index < 0 || index >= objects.Count)
				return null;

			return objects[index];
		}
		public Object3D GetObject(string name) {
			foreach(Object3D obj in objects) {
				if(obj.Name == name)
					return obj;
			}

			return null;
		}
		public Object3D GetObject(Guid guid) {
			foreach(Object3D obj in objects) {
				if(obj.GUID == guid)
					return obj;
			}

			return null;
		}
		public List<Object3D> GetAllObjects() {
			return objects;
		}
		public List<Object3D> GetAllObjects(string name) {
			List<Object3D> objList = new List<Object3D>();

			foreach(Object3D obj in objects) {
				if(obj.Name == name)
					objList.Add(obj);
			}

			return objList;
		}

		public Mesh GetMesh() {
			return meshes[0];
		}
		public Mesh GetMesh(int index) {
			if(index < 0 || index >= meshes.Count)
				return null;

			return meshes[index];
		}
		public Mesh GetMesh(string name) {
			foreach(Mesh mesh in meshes) {
				if(mesh.Name == name)
					return mesh;
			}

			return null;
		}
		public Mesh GetMesh(Guid guid) {
			foreach(Mesh mesh in meshes) {
				if(mesh.GUID == guid)
					return mesh;
			}

			return null;
		}
		public List<Mesh> GetMeshes(string name) {
			List<Mesh> matchingMeshes = new List<Mesh>();

			foreach(Mesh mesh in meshes) {
				if(mesh.Name == name) {
					matchingMeshes.Add(mesh);
				}
			}

			return matchingMeshes;
		}
		public List<Mesh> GetAllMeshes() {
			return meshes;
		}

		public AudioSource GetSource() {
			return sources[0];
		}
		public AudioSource GetSource(int index) {
			if(index < 0 || index >= sources.Count)
				return null;

			return sources[index];
		}
		public AudioSource GetSource(string name) {
			foreach(AudioSource source in sources) {
				if(source.Name == name)
					return source;
			}
			return null;
		}
		public List<AudioSource> GetAllSources() {
			return sources;

		}

		public Light GetLight() {
			if(lights.Count <= 0)
				return null;

			return lights[0];
		}
		public Light GetLight(int index) {
			if(index < 0 || index >= lights.Count)
				return null;

			return lights[index];
		}
		public Light GetLight(string name) {
			foreach(Light light in lights) {
				if(light.Name == name)
					return light;
			}

			return null;
		}
		public Light GetLight(Guid guid) {
			foreach(Light light in lights) {
				if(light.GUID == guid)
					return light;
			}
			return null;
		}
		public List<Light> GetLights(string name) {
			List<Light> matchedLights = new List<Light>();

			foreach(Light light in lights) {
				if(light.Name == name)
					matchedLights.Add(light);
			}

			return matchedLights;
		}
		public List<Light> GetLights(LightType type) {
			return sortedLights[type];
		}
		public List<Light> GetAllLights() {
			return lights;
		}
		public Dictionary<LightType, List<Light>> GetSortedLights() {
			return sortedLights;
		}

		public Camera GetCamera() {
			return this.Camera;
		}

		public bool Exists(string name) {
			return Exists(GetObject(name));
		}
		public bool Exists(Guid guid) {
			return Exists(GetObject(guid));
		}
		public bool Exists(Object3D obj) {
			if(obj == null)
				return false;

			foreach(Object3D sampledObj in objects) {
				if(sampledObj.Equals(obj))
					return true;
			}

			return false;
		}

		private void AddCamera(Camera camera) {
			if(camera == null)
				return;

			this.Camera = camera;
		}
		private void AddMesh(Mesh mesh) {
			if(mesh == null)
				return;

			meshes.Add(mesh);
		}
		private void AddSource(AudioSource source) {
			if(source == null)
				return;

			sources.Add(source);
		}
		private void AddLight(Light light) {
			if(light == null)
				return;

			lights.Add(light);
			sortedLights[light.Type].Add(light);
		}
		private bool RemoveMesh(Mesh mesh) {
			if(mesh == null)
				return false;

			return meshes.Remove(mesh);
		}
		private bool RemoveSource(AudioSource source) {
			if(source == null)
				return false;

			return sources.Remove(source);
		}
		private bool RemoveLight(Light light) {
			if(light == null)
				return false;

			return lights.Remove(light) | sortedLights[light.Type].Remove(light);
		}
	}
}
