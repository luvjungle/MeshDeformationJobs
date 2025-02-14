using System.Collections.Generic;
using Code.Bootstrap.Loading.Infrastructure;
using Code.Bootstrap.Loading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Code.Bootstrap.Loading
{
	public class LoadingController
	{
		private readonly ILoadingView _loadingView;

		public LoadingController(ILoadingView loadingView)
		{
			_loadingView = loadingView;
		}

		public void FadeAndLoad(string sceneName)
		{
			_loadingView.FadeIn(() => LoadTasksAsync(new List<ILoadTask>() { new LevelLoadTask(sceneName) }).Forget());
		}

		public async UniTaskVoid LoadTasksAsync(List<ILoadTask> tasks)
		{
			int progressAmount = 0;
			int currentProgress = 0;
			foreach (var task in tasks)
			{
				if (task.CountProgress)
					progressAmount++;
			}

			progressAmount = Mathf.Max(1, progressAmount);

			for (int i = 0; i < tasks.Count; i++)
			{
				tasks[i].OnLoadStart();
				while (!tasks[i].IsDone())
				{
					tasks[i].OnLoading();

					_loadingView.SetProgress((currentProgress + tasks[i].GetProgress()) / progressAmount);

					await UniTask.NextFrame();
				}

				tasks[i].OnLoadFinish();

				if (tasks[i].CountProgress)
					currentProgress++;

				await UniTask.NextFrame();
			}

			_loadingView.FadeOut(null);
		}
	}
}