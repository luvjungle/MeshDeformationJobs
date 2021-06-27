using System.Collections.Generic;
using UnityEngine;

// Logic of Move Vertex Down Job running on Main Thread
// For benchmarking purposes
public class MeshDeformerMainThread : MonoBehaviour
{
    [SerializeField] private float ballYOffset;
    [SerializeField] private float ballRadius;
    [SerializeField] private float edgeDistance;
    [SerializeField] private Transform ball;
    [SerializeField] private MeshFilter meshFilter;

    private List<Vector3> vertices;
    private Mesh mesh;

    protected void Start()
    {
        mesh = meshFilter.mesh;
        mesh.MarkDynamic();

        vertices = new List<Vector3>(mesh.vertices);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        
        mesh.GetVertices(vertices);

        for (int i = 0; i < vertices.Count; i++)
        {
            var pos = transform.TransformPoint(vertices[i]);

            var targetY = ball.position.y + ballYOffset - ballRadius;

            // Check that vertex is not under desired coordinate already
            if (pos.y <= targetY) continue;

            // Check that vertex is inside radius
            var distance = Vector3.Distance(ball.position, pos);
            if (distance > ballRadius) continue;

            // Edge smoothing
            if (distance > ballRadius - edgeDistance)
                targetY *= (ballRadius - distance) / edgeDistance;

            // Set vertex position
            pos.y = targetY;

            // Calculate local position of vertex
            var localPos = transform.InverseTransformPoint(pos);
            vertices[i] = new Vector3(localPos.x, localPos.y, localPos.z);
        }
        
        mesh.SetVertices(vertices);
        mesh.RecalculateNormals();
    }
}