using Code.Bootstrap.Loading.Infrastructure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Code.Bootstrap.Loading.Tasks
{
	public class LevelLoadTask : ILoadTask
	{
		private readonly string _sceneName;
		private readonly int _sceneIndex;
		
		private bool _loadImmediately = true;
		private AsyncOperation _op;

		public bool CountProgress => true;

		public LevelLoadTask(string sceneName)
		{
			_sceneIndex = -1;
			_sceneName = sceneName;
		}

		public LevelLoadTask(int sceneIndex)
		{
			_sceneIndex = sceneIndex;
		}

		public float GetProgress() => _op.progress;

		public bool IsDone()
		{
			if (_loadImmediately)
				return _op.isDone;
			
			return _op.progress >= 0.9f;
		}

		public void OnLoadStart()
		{
			if (_sceneIndex == -1)
				_op = SceneManager.LoadSceneAsync(_sceneName);
			else
				_op = SceneManager.LoadSceneAsync(_sceneIndex);

			if (!_loadImmediately)
				_op.allowSceneActivation = false;
		}
		
		public void SetLoadImmediately(bool immediately) => _loadImmediately = immediately;
		public void ActivateScene() => _op.allowSceneActivation = true;
		public void OnLoading() { }
		public void OnLoadFinish() { }
	}
}