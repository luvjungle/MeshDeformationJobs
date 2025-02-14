using Code.Bootstrap.Loading.Infrastructure;

namespace Code.Bootstrap.Loading.Tasks
{
	public class ButtonPressTask : ILoadTask
	{
		private readonly LevelLoadTask _levelLoadTask;
		private readonly ILoadingView _loadingView;

		private bool _clicked;

		public bool CountProgress => false;
		
		public ButtonPressTask(LevelLoadTask levelLoadTask, ILoadingView view)
		{
			_levelLoadTask = levelLoadTask;
			_loadingView = view;
		}

		public float GetProgress()
		{
			return 1;
		}

		public bool IsDone()
		{
			return _clicked;
		}

		public void OnLoadStart()
		{
			_loadingView.ActivateButton(OnClick);
		}

		private void OnClick()
		{
			if (_clicked) return;
			
			_levelLoadTask.ActivateScene();
			_clicked = true;
		}
		
		public void OnLoading() { }
		public void OnLoadFinish() { }
	}
}