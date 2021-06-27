using System;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public struct MoveVertexDownSettings
{
    public float ballYOffset;
    public float ballRadius;
    public float edgeDistance;
}

public class MoveVertexDown : BaseDeformJobRunner
{
    [SerializeField] private MoveVertexDownSettings settings;
    
    private MoveVertexDownJob meshModJob;

    public override void RunJob(Transform ball, Transform mesh)
    {
        meshModJob = new MoveVertexDownJob()
        {
            settings = settings,
            ballPos = new float4(ball.position.x, ball.position.y, ball.position.z, 1),
            localToWorld = mesh.transform.localToWorldMatrix,
            worldToLocal = mesh.transform.worldToLocalMatrix,
            vertices = _vertices
        };

        jobHandle = meshModJob.Schedule(_vertices.Length, 64);
    }
    
    public override NativeArray<Vector3> GetNewVertices()
    {
        jobHandle.Complete();
        return meshModJob.vertices;
    }
}

[BurstCompile]
struct MoveVertexDownJob : IJobParallelFor
{
    public MoveVertexDownSettings settings;
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

        var targetY = ballPos.y + settings.ballYOffset - settings.ballRadius;

        // Check that vertex is not under desired coordinate already
        if (pos.y <= targetY) return;

        // Check that vertex is inside radius
        var distance = math.distance(ballPos, pos);
        if (distance > settings.ballRadius) return;

        // Edge smoothing
        if (distance > settings.ballRadius - settings.edgeDistance)
            targetY *= (settings.ballRadius - distance) / settings.edgeDistance;

        // Set vertex position
        pos.y = targetY;

        // Calculate local position of vertex
        var localPos = worldToLocal * pos;
        vertices[i] = new Vector3(localPos.x, localPos.y, localPos.z);
    }
}
