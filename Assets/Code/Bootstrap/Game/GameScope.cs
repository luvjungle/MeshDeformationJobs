using Code.Brush;
using Code.Brush.Infrastructure;
using Code.InputHandling.Infrastructure;
using Code.InputHandling.MouseInput;
using Code.MeshDeformation;
using Code.MeshDeformation.Infrastructure;
using Code.MeshDeformation.Strategies;
using Code.MeshDeformation.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Bootstrap.Game
{
	public class GameScope : LifetimeScope
	{
		[SerializeField] private MouseInputConfigSO _mouseInputConfigSO;
		[SerializeField] private DeformationConfigSO _deformationConfigSO;
		[SerializeField] private BrushController _brushController;
		[SerializeField] private MonoGameEntryPoint _monoGameEntryPoint;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterComponent(_brushController).As<IBrush>();
			builder.RegisterComponent(_monoGameEntryPoint);

			builder.Register<DeformStrategyFactory>(Lifetime.Singleton).As<IDeformStrategyFactory>();
			builder.Register<MeshDeformViewModel>(Lifetime.Singleton).As<IMeshDeformViewModel>();
			builder.Register<DeformationModel>(Lifetime.Singleton).WithParameter(_deformationConfigSO).As<IDeformationModel>();

			builder.UseEntryPoints(entryPoints =>
			{
				entryPoints.Add<GameEntryPoint>();
				entryPoints.Add<MeshDeformer>();
				entryPoints.Add<MouseInputHandler>().As<IInputHandler>().WithParameter(_mouseInputConfigSO)
					.WithParameter(Camera.main);
			});
		}
	}
}