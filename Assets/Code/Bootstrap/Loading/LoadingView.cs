using System;
using Code.Bootstrap.Loading.Infrastructure;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Bootstrap.Loading
{
	public class LoadingView : MonoBehaviour, ILoadingView
	{
		[SerializeField] private Image _fill;
		[SerializeField] private CanvasGroup _group;
		[SerializeField] private Button _button;
		[SerializeField] private GameObject _progress;

		private Tween _buttonScaleTween;

		public void ShowDefault()
		{
			_button?.gameObject.SetActive(false);
			_buttonScaleTween?.Kill();
			_progress.SetActive(true);
			gameObject.SetActive(true);
			_fill.fillAmount = 0;
			_group.alpha = 1;
		}

		public void FadeIn(Action onComplete = null)
		{
			ShowDefault();
			_group.alpha = 0;
			_group.DOFade(1, 0.3f).onComplete += () => onComplete?.Invoke();
		}

		public void FadeOut(Action onComplete = null)
		{
			gameObject.SetActive(true);
			_fill.fillAmount = 1;
			_group.alpha = 1;
			_group.DOFade(0, 0.3f).onComplete += () =>
			{
				gameObject.SetActive(false);
				_buttonScaleTween?.Kill();
				onComplete?.Invoke();
			};
		}

		public void SetProgress(float progress) => _fill.fillAmount = progress;

		public void ActivateButton(Action callback)
		{
			if (!_button)
			{
				callback?.Invoke();
				return;
			}

			_progress.SetActive(false);
			_button.gameObject.SetActive(true);
			_button.onClick.RemoveAllListeners();
			_button.onClick.AddListener(() => callback?.Invoke());

			_buttonScaleTween?.Kill();
			_buttonScaleTween = _button.transform.DOScale(Vector3.one * 1.2f, 0.3f).SetLoops(-1, LoopType.Yoyo);
		}
	}
}