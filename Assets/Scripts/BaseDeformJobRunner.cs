using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public abstract class BaseDeformJobRunner: MonoBehaviour
{
    protected JobHandle jobHandle;
    protected NativeArray<Vector3> _vertices;
    
    public void SetVertices(Vector3[] vertices)
    {
        _vertices = new NativeArray<Vector3>(vertices, Allocator.Persistent);
    }
    
    public abstract void RunJob(Transform ball, Transform mesh);
    public abstract NativeArray<Vector3> GetNewVertices();

    private void OnDestroy()
    {
        _vertices.Dispose();
    }
}