using UnityEngine;

namespace Code.InputHandling.Infrastructure
{
	public interface IInputHandler
	{
		bool CanDeform { get; }
		bool TryGetPosition(out Vector3 position);
	}
}