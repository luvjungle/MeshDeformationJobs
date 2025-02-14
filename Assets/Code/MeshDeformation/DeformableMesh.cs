using Code.MeshDeformation.Infrastructure;
using UnityEngine;

namespace Code.MeshDeformation
{
	public class DeformableMesh : MonoBehaviour, IDeformableMeshProvider
	{
		[SerializeField] private MeshFilter _meshFilter;

		public Mesh Mesh => _meshFilter.mesh;
	}
}