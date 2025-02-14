namespace Code.Bootstrap.Loading.Infrastructure
{
	public interface ILoadTask
	{
		public bool CountProgress { get; }
		public float GetProgress();
		public bool IsDone();
		public void OnLoadStart();
		public void OnLoading();
		public void OnLoadFinish();
	}
}