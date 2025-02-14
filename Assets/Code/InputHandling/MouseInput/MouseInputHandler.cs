using Code.InputHandling.Infrastructure;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer.Unity;

namespace Code.InputHandling.MouseInput
{
	public class MouseInputHandler : IInputHandler, ITickable
	{
		private readonly MouseInputConfigSO _config;
		private readonly Camera _cam;

		private bool _isClickValid;

		public MouseInputHandler(MouseInputConfigSO config, Camera cam)
		{
			_config = config;
			_cam = cam;
		}

		public bool CanDeform => _isClickValid;

		public bool TryGetPosition(out Vector3 position)
		{
			position = default;
			
			if (!_isClickValid) 
				return false;
			
			var ray = _cam.ScreenPointToRay(Input.mousePosition);
			var isHit = Physics.Raycast(ray, out var hitInfo, _config.RaycastDistance, _config.GroundLayer.value);
			
			if (!isHit) 
				return false;

			position = hitInfo.point + Vector3.up * _config.GroundOffset;
			return true;
		}

		public void Tick()
		{
			if (!Input.GetMouseButton(0))
			{
				_isClickValid = false;
			}
			else if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
			{
				_isClickValid = true;
			}
		}
	}
}
