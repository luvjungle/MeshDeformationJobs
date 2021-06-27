using UnityEngine;

public class MeshDeformer : MonoBehaviour
{
    [SerializeField] private BaseDeformJobRunner jobRunner;
    [SerializeField] private Transform ball;
    [SerializeField] private MeshFilter meshFilter;

    private Mesh mesh;
    private bool jobFired;

    protected void Start()
    {
        mesh = meshFilter.mesh;
        mesh.MarkDynamic();

        jobRunner.SetVertices(mesh.vertices);
    }

    public void Update()
    {
        if (!Input.GetMouseButton(0)) return;
        
        jobRunner.RunJob(ball, meshFilter.transform);
        jobFired = true;
    }

    public void LateUpdate()
    {
        if (!jobFired) return;
        
        mesh.SetVertices(jobRunner.GetNewVertices());
        mesh.RecalculateNormals();
        jobFired = false;
    }
}