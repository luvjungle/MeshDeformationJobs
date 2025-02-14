using UnityEngine;

namespace Code.MeshDeformation.Infrastructure
{
	public interface IDeformableMeshProvider
	{
		public Mesh Mesh { get; }
		public Transform transform { get; }
	}
}