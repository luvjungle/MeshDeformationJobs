using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Utils.UI
{
	public class SliderValue : MonoBehaviour
	{
		[SerializeField] private TMP_Text _name;
		[SerializeField] private TMP_Text _value;
		[SerializeField] private Slider _slider;
		
		public Action<float> OnValueChangedAction;

		public void LoadSliderValue(float value) => _slider.SetValueWithoutNotify(value);
		
		public void SetText(float value) => _value.SetText(value.ToString("0.##"));
		
		public void SetName(string text) => _name.SetText(text);

		public void Activate(bool active) => gameObject.SetActive(active);
		
		public void OnValueChanged(float value) => OnValueChangedAction?.Invoke(value);
	}
}