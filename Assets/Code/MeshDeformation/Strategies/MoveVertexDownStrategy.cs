using Code.MeshDeformation.Infrastructure;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Code.MeshDeformation.Strategies
{
	public class MoveVertexDownStrategy : IDeformStrategy
	{
		private readonly IDeformationModel _model;

		private JobHandle _jobHandle;
		private MoveVertexDownJob _meshModJob;

		public MoveVertexDownStrategy(IDeformationModel model)
		{
			_model = model;
		}

		public void RunJob(Transform brush, Transform mesh, NativeArray<Vector3> vertices)
		{
			_meshModJob = new MoveVertexDownJob()
			{
				Settings = new MoveVertexDownSettings()
				{
					BallRadius = _model.BrushRadius.Value,
					EdgeDistance = _model.EdgeDistance.Value
				},
				BallPos = new float4(brush.position.x, brush.position.y, brush.position.z, 1),
				LocalToWorld = mesh.transform.localToWorldMatrix,
				WorldToLocal = mesh.transform.worldToLocalMatrix,
				Vertices = vertices
			};

			_jobHandle = _meshModJob.Schedule(vertices.Length, 64);
		}

		public NativeArray<Vector3> GetNewVertices()
		{
			_jobHandle.Complete();
			return _meshModJob.Vertices;
		}

		private struct MoveVertexDownSettings
		{
			public float BallRadius;
			public float EdgeDistance;
		}

		[BurstCompile]
		private struct MoveVertexDownJob : IJobParallelFor
		{
			public MoveVertexDownSettings Settings;
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

				var targetY = BallPos.y - Settings.BallRadius;

				// Check that vertex is not under desired coordinate already
				if (pos.y <= targetY) return;

				// Check that vertex is inside radius
				var distance = math.distance(BallPos, pos);
				if (distance > Settings.BallRadius) return;

				// Edge smoothing
				if (distance > Settings.BallRadius - Settings.EdgeDistance)
					targetY *= (Settings.BallRadius - distance) / Settings.EdgeDistance;

				// Set vertex position
				pos.y = targetY;

				// Calculate local position of vertex
				var localPos = WorldToLocal * pos;
				Vertices[i] = new Vector3(localPos.x, localPos.y, localPos.z);
			}
		}
	}
}