using Code.MeshDeformation;
using Code.MeshDeformation.Infrastructure;
using Code.MeshDeformation.UI;
using UnityEngine;
using VContainer;

namespace Code.Bootstrap.Game
{
	public class MonoGameEntryPoint : MonoBehaviour
	{
		[SerializeField] private DeformableMesh _startMesh;
		[SerializeField] private MeshDeformView _meshDeformView;

		private IDeformationModel _model;

		[Inject]
		public void Construct(IDeformationModel model, IObjectResolver resolver)
		{
			_model = model;
			resolver.Inject(_meshDeformView);
		}
		
		public void OnStart()
		{
			_model.Mesh.Value = _startMesh;
		}
	}
}