using Code.Bootstrap.Loading.Infrastructure;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Bootstrap.Loading
{
	public class RootScope : LifetimeScope
	{
		[SerializeField] private LoadingView _loadingView;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterComponentInNewPrefab(_loadingView, Lifetime.Singleton).DontDestroyOnLoad().As<ILoadingView>();
			builder.Register<LoadingController>(Lifetime.Singleton);
		}
	}
}