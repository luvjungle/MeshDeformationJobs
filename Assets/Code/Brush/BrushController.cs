using Code.Brush.Infrastructure;
using Code.InputHandling.Infrastructure;
using UnityEngine;
using VContainer;

namespace Code.Brush
{
	// This class can be made with vContainer's loop (like MeshDeformer). I just wanted to show different approach.
	public class BrushController : MonoBehaviour, IBrush
	{
		private BrushMovement _movement;

		[Inject]
		public void Construct(IInputHandler inputHandler)
		{
			_movement = new BrushMovement(inputHandler, transform);
		}

		private void Update()
		{
			_movement.OnUpdate();
		}
	}
}