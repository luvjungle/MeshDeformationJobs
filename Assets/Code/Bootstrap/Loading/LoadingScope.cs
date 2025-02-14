using VContainer;
using VContainer.Unity;

namespace Code.Bootstrap.Loading
{
	public class LoadingScope : LifetimeScope
	{
		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterEntryPoint<LoadingEntryPoint>();
		}
	}
}