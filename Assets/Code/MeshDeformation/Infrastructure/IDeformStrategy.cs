using Unity.Collections;
using UnityEngine;

namespace Code.MeshDeformation.Infrastructure
{
	public interface IDeformStrategy
	{
		void RunJob(Transform brush, Transform mesh, NativeArray<Vector3> vertices);
		NativeArray<Vector3> GetNewVertices();
	}
}