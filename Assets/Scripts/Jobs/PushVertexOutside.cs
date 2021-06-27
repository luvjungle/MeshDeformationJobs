using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct PushVertexOutsideSettings
{
    public float ballRadius;
    public float force;
    [HideInInspector] public float deltaTime;
}

public class PushVertexOutside : BaseDeformJobRunner
{
    [SerializeField] private PushVertexOutsideSettings _settings;
    
    private MoveVertexDownJob job;
    
    public override void RunJob(Transform ball, Transform mesh)
    {
        var settings = _settings;
        settings.deltaTime = Time.deltaTime;
        
        job = new MoveVertexDownJob
        {
            settings = settings,
            ballPos = new float4(ball.position.x, ball.position.y, ball.position.z, 1),
            localToWorld = mesh.transform.localToWorldMatrix,
            worldToLocal = mesh.transform.worldToLocalMatrix,
            vertices = _vertices
        };

        jobHandle = job.Schedule(_vertices.Length, 64);
    }

    public override NativeArray<Vector3> GetNewVertices()
    {
        jobHandle.Complete();
        return job.vertices;
    }

    [BurstCompile]
    struct MoveVertexDownJob : IJobParallelFor
    {
        public PushVertexOutsideSettings settings;
        public float4 ballPos;
        public Matrix4x4 localToWorld;
        public Matrix4x4 worldToLocal;
        public NativeArray<Vector3> vertices;

        public void Execute(int i)
        {
            // Convert vertex position to world space
            var pos = float4.zero;
            pos.xyz = vertices[i];
            pos.w = 1f;
            pos = localToWorld * pos;

            // Check that vertex is inside radius
            var distance = math.distance(ballPos, pos);
            if (distance > settings.ballRadius) return;

            var direction = pos.xyz - ballPos.xyz;
            pos.xyz += math.normalize(direction) * settings.force * settings.deltaTime;

            // Calculate local position of vertex
            var localPos = worldToLocal * pos;
            vertices[i] = new Vector3(localPos.x, localPos.y, localPos.z);
        }
    }
}
