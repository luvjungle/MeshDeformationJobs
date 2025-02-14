using UnityEngine;

namespace Code.InputHandling.MouseInput
{
	[CreateAssetMenu(fileName = "Mouse Input Config", menuName = "Configs/Mouse Input Config")]
	public class MouseInputConfigSO: ScriptableObject
	{
		[SerializeField] private float _raycastDistance;
		[SerializeField] private float _groundOffset;
		[SerializeField] private LayerMask _groundLayer;
		
		public float RaycastDistance => _raycastDistance;
		public float GroundOffset => _groundOffset;
		public LayerMask GroundLayer => _groundLayer;
	}
}