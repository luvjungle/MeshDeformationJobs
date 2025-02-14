using Code.MeshDeformation.Infrastructure;
using UnityEngine;

namespace Code.MeshDeformation
{
	[CreateAssetMenu(fileName = "Deformation Config", menuName = "Configs/Deformation Config")]
	public class DeformationConfigSO : ScriptableObject
	{
		[SerializeField] private DeformType _startType;
		[SerializeField] private float _brushRadius;

		[Header("Move Down Settings")] 
		
		[SerializeField] private float _edgeDistance;

		[Header("Push Outside Settings")]
		
		[SerializeField] private float _force;
		
		public DeformType StartType => _startType;
		public float BrushRadius => _brushRadius;
		public float EdgeDistance => _edgeDistance;
		public float Force => _force;
	}
}