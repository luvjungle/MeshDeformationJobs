using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
    [SerializeField] private float ballYOffset;
    [SerializeField] private float ballRadius;
    [SerializeField] private float edgeDistance;
    [SerializeField] private Transform ball;
    [SerializeField] private MeshFilter meshFilter;

    NativeArray<Vector3> vertices;
    Vector3[] modifiedVertices;

    MeshModJob meshModJob;
    JobHandle jobHandle;

    private Mesh mesh;
    private bool jobFired;

    protected void Start()
    {
        mesh = meshFilter.mesh;
        mesh.MarkDynamic();

        vertices = new NativeArray<Vector3>(mesh.vertices, Allocator.Persistent);
        modifiedVertices = new Vector3[vertices.Length];
    }

    public void Update()
    {
        if (!Input.GetMouseButton(0)) return;

        meshModJob = new MeshModJob()
        {
            ballYOffset = ballYOffset,
            ballRadius = ballRadius,
            edgeDistance = edgeDistance,
            ballPos = new float4(ball.position.x, ball.position.y, ball.position.z, 1),
            localToWorld = transform.localToWorldMatrix,
            worldToLocal = transform.worldToLocalMatrix,
            vertices = vertices
        };

        jobHandle = meshModJob.Schedule(vertices.Length, 64);
        jobFired = true;
    }

    public void LateUpdate()
    {
        if (!jobFired) return;

        jobHandle.Complete();

        meshModJob.vertices.CopyTo(modifiedVertices);

        mesh.SetVertices(modifiedVertices);
        mesh.RecalculateNormals();
        jobFired = false;
    }

    private void OnDestroy()
    {
        vertices.Dispose();
    }

    [BurstCompile]
    struct MeshModJob : IJobParallelFor
    {
        public float ballYOffset;
        public float ballRadius;
        public float edgeDistance;
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

            var targetY = ballPos.y + ballYOffset - ballRadius;

            // Check that vertex is not under desired coordinate already
            if (pos.y <= targetY) return;

            // Check that vertex is inside radius
            var distance = math.distance(ballPos, pos);
            if (distance > ballRadius) return;

            // Edge smoothing
            if (distance > ballRadius - edgeDistance)
                targetY *= (ballRadius - distance) / edgeDistance;

            // Set vertex position
            pos.y = targetY;

            // Calculate local position of vertex
            var localPos = worldToLocal * pos;
            vertices[i] = new Vector3(localPos.x, localPos.y, localPos.z);
        }
    }
}