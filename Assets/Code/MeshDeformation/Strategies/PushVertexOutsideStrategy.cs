using Code.MeshDeformation.Infrastructure;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Code.MeshDeformation.Strategies
{
	public class PushVertexOutsideStrategy : IDeformStrategy
	{
		private readonly IDeformationModel _model;

		private JobHandle _jobHandle;
		private MoveVertexDownJob _job;

		public PushVertexOutsideStrategy(IDeformationModel model)
		{
			_model = model;
		}

		public void RunJob(Transform brush, Transform mesh, NativeArray<Vector3> vertices)
		{
			_job = new MoveVertexDownJob
			{
				Settings = new PushVertexOutsideSettings()
				{
					BallRadius = _model.BrushRadius.Value,
					Force = _model.Force.Value,
					DeltaTime = Time.deltaTime
				},
				BallPos = new float4(brush.position.x, brush.position.y, brush.position.z, 1),
				LocalToWorld = mesh.transform.localToWorldMatrix,
				WorldToLocal = mesh.transform.worldToLocalMatrix,
				Vertices = vertices
			};

			_jobHandle = _job.Schedule(vertices.Length, 64);
		}

		public NativeArray<Vector3> GetNewVertices()
		{
			_jobHandle.Complete();
			return _job.Vertices;
		}

		private struct PushVertexOutsideSettings
		{
			public float BallRadius;
			public float Force;
			public float DeltaTime;
		}

		[BurstCompile]
		struct MoveVertexDownJob : IJobParallelFor
		{
			public PushVertexOutsideSettings Settings;
			public float4 BallPos;
			public Matrix4x4 LocalToWorld;
			public Matrix4x4 WorldToLocal;
			public NativeArray<Vector3> Vertices;

			public void Execute(int i)
			{
				// Convert vertex position to world space
				var pos = float4.zero;
				pos.xyz = Vertices[i];
				pos.w = 1f;
				pos = LocalToWorld * pos;

				// Check that vertex is inside radius
				var distance = math.distance(BallPos, pos);
				if (distance > Settings.BallRadius) return;

				var direction = pos.xyz - BallPos.xyz;
				pos.xyz += math.normalize(direction) * Settings.Force * Settings.DeltaTime;

				// Calculate local position of vertex
				var localPos = WorldToLocal * pos;
				Vertices[i] = new Vector3(localPos.x, localPos.y, localPos.z);
			}
		}
	}
}