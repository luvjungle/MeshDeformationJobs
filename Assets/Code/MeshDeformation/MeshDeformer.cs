using System;
using Code.Brush.Infrastructure;
using Code.InputHandling.Infrastructure;
using Code.MeshDeformation.Infrastructure;
using Unity.Collections;
using UnityEngine;
using VContainer.Unity;

namespace Code.MeshDeformation
{
	public class MeshDeformer : IPostTickable, IPostLateTickable, IDisposable
	{
		private readonly IInputHandler _inputHandler;
		private readonly IBrush _brush;
		private readonly IDeformationModel _model;
		private readonly IDeformStrategyFactory _strategyFactory;
		
		private IDeformStrategy _deformStrategy;
		private IDeformableMeshProvider _meshProvider;
		private NativeArray<Vector3> _vertices;
		private bool _jobFired;

		public MeshDeformer(IInputHandler inputHandler, IBrush brush, IDeformationModel model, IDeformStrategyFactory strategyFactory)
		{
			_inputHandler = inputHandler;
			_brush = brush;
			_model = model;
			_strategyFactory = strategyFactory;
			
			_model.DeformType.Subscribe(ChangeStrategy, true);
			_model.Mesh.Subscribe(SetMesh);
		}

		private void SetMesh(IDeformableMeshProvider meshProvider)
		{
			if (meshProvider == null)
				return;
			
			if (_jobFired)
				CompleteJob();

			if (_vertices.IsCreated)
				_vertices.Dispose();
			
			_meshProvider = meshProvider;
			_meshProvider.Mesh.MarkDynamic();
			_vertices = new NativeArray<Vector3>(_meshProvider.Mesh.vertices, Allocator.Persistent);
		}

		private void ChangeStrategy(DeformType type)
		{
			if (_jobFired)
				CompleteJob();

			_deformStrategy = _strategyFactory.GetStrategy(type);
		}

		public void PostTick()
		{
			if (!_inputHandler.CanDeform) return;
			if (_meshProvider == null) return;

			_deformStrategy.RunJob(_brush.transform, _meshProvider.transform, _vertices);
			_jobFired = true;
		}

		public void PostLateTick()
		{
			CompleteJob();
		}
		
		private void CompleteJob()
		{
			if (!_jobFired) return;

			_meshProvider.Mesh.SetVertices(_deformStrategy.GetNewVertices());
			_meshProvider.Mesh.RecalculateNormals();
			_jobFired = false;
		}
		
		public void Dispose()
		{
			_model.DeformType.Unsubscribe(ChangeStrategy);
			_model.Mesh.Unsubscribe(SetMesh);
			
			if (_vertices.IsCreated)
				_vertices.Dispose();
		}
	}
}