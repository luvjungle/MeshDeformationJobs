using System.Collections.Generic;
using Code.Bootstrap.Loading.Infrastructure;
using Code.Bootstrap.Loading.Tasks;
using VContainer.Unity;

namespace Code.Bootstrap.Loading
{
	public class LoadingEntryPoint : IStartable
	{
		private readonly LoadingController _loadingController;
		private readonly ILoadingView _loadingView;

		public LoadingEntryPoint(LoadingController loadingController, ILoadingView loadingView)
		{
			_loadingController = loadingController;
			_loadingView = loadingView;
		}

		public void Start()
		{
			_loadingView.ShowDefault();
			
			List<ILoadTask> taskList = new List<ILoadTask>();
			var levelLoadTask = new LevelLoadTask("Game");
			taskList.Add(levelLoadTask);
			_loadingController.LoadTasksAsync(taskList).Forget();
		}
	}
}