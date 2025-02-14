using System;

namespace Code.Utils
{
	public class Observable<T>
	{
		public T Value
		{
			get => _value;
			set
			{
				_value = value;
				OnChange?.Invoke(_value);
			}
		}

		private event Action<T> OnChange;

		private T _value;

		public Observable(T defaultValue)
		{
			_value = defaultValue;
		}

		public void Subscribe(Action<T> action, bool immediate = false)
		{
			OnChange += action;

			if (immediate)
				action?.Invoke(_value);
		}

		public void Unsubscribe(Action<T> action)
		{
			OnChange -= action;
		}
	}
}