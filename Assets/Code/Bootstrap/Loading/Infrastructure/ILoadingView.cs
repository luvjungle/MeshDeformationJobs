using System;

namespace Code.Bootstrap.Loading.Infrastructure
{
	public interface ILoadingView
	{
		public void ShowDefault();
		public void FadeIn(Action onComplete);
		public void FadeOut(Action onComplete);
		public void SetProgress(float progress);
		public void ActivateButton(Action onClick);
	}
}