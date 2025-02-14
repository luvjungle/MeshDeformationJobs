using System;
using System.Collections.Generic;
using Code.MeshDeformation.Infrastructure;

namespace Code.MeshDeformation.Strategies
{
	public class DeformStrategyFactory : IDeformStrategyFactory
	{
		private readonly IDeformationModel _model;
		private readonly Dictionary<DeformType, IDeformStrategy> _pool = new();
		
		public DeformStrategyFactory(IDeformationModel model)
		{
			_model = model;
		}
		
		public IDeformStrategy GetStrategy(DeformType type)
		{
			if (_pool.ContainsKey(type))
				return _pool[type];

			IDeformStrategy strategy = type switch
			{
				DeformType.MoveVertexDown => new MoveVertexDownStrategy(_model),
				DeformType.PushVertexOutside => new PushVertexOutsideStrategy(_model),
				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
			
			_pool.Add(type, strategy);
			return strategy;
		}
	}
}