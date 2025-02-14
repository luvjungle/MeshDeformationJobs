using Code.InputHandling.Infrastructure;
using UnityEngine;

namespace Code.Brush
{
	public class BrushMovement
	{
		private readonly IInputHandler _inputHandler;
		private readonly Transform _transform;

		public BrushMovement(IInputHandler inputHandler, Transform transform)
		{
			_inputHandler = inputHandler;
			_transform = transform;
		}

		public void OnUpdate()
		{
			if (_inputHandler.TryGetPosition(out Vector3 position))
				_transform.position = position;
		}
	}
}