using System.Collections.Generic;
using Code.MeshDeformation.Infrastructure;
using Code.Utils.UI;
using TMPro;
using UnityEngine;
using VContainer;

namespace Code.MeshDeformation.UI
{
	public class MeshDeformView : MonoBehaviour
	{
		[SerializeField] private TMP_Text _dropDownName;
		[SerializeField] private TMP_Dropdown _dropdown;
		[SerializeField] private SliderValue _radius;
		[SerializeField] private SliderValue _edgeDistance;
		[SerializeField] private SliderValue _force;

		private IMeshDeformViewModel _viewModel;

		[Inject]
		public void Construct(IMeshDeformViewModel viewModel)
		{
			_viewModel = viewModel;

			_viewModel.DropDownName.Subscribe(OnDropDownNameChanged, true);
			_viewModel.DropDownOptions.Subscribe(OnDropdownOptionsChanged, true);
			_viewModel.DropDownValue.Subscribe(OnDropdownValueChanged, true);

			_viewModel.RadiusName.Subscribe(_radius.SetName, true);
			_viewModel.RadiusValue.Subscribe(_radius.SetText, true);
			
			_viewModel.EdgeDistanceName.Subscribe(_edgeDistance.SetName, true);
			_viewModel.EdgeDistanceValue.Subscribe(_edgeDistance.SetText, true);
			
			_viewModel.ForceName.Subscribe(_force.SetName, true);
			_viewModel.ForceValue.Subscribe(_force.SetText, true);

			_viewModel.RadiusActive.Subscribe(_radius.Activate, true);
			_viewModel.EdgeActive.Subscribe(_edgeDistance.Activate, true);
			_viewModel.ForceActive.Subscribe(_force.Activate, true);
			
			_radius.LoadSliderValue(_viewModel.RadiusValue.Value);
			_edgeDistance.LoadSliderValue(_viewModel.EdgeDistanceValue.Value);
			_force.LoadSliderValue(_viewModel.ForceValue.Value);
			
			_dropdown.onValueChanged.AddListener(_viewModel.TypeChangeByView);
			_radius.OnValueChangedAction += _viewModel.RadiusChangeByView;
			_edgeDistance.OnValueChangedAction += _viewModel.EdgeChangeByView;
			_force.OnValueChangedAction += _viewModel.ForceChangeByView;
		}

		private void OnDropDownNameChanged(string value) => _dropDownName.SetText(value);
		
		private void OnDropdownValueChanged(int value) => _dropdown.SetValueWithoutNotify(value);

		private void OnDropdownOptionsChanged(List<string> options)
		{
			_dropdown.ClearOptions();
			_dropdown.AddOptions(options);
		}

		private void OnDestroy()
		{
			_viewModel.DropDownName.Unsubscribe(OnDropDownNameChanged);
			_viewModel.DropDownOptions.Unsubscribe(OnDropdownOptionsChanged);
			_viewModel.DropDownValue.Unsubscribe(OnDropdownValueChanged);

			_viewModel.RadiusName.Unsubscribe(_radius.SetName);
			_viewModel.RadiusValue.Unsubscribe(_radius.SetText);
			
			_viewModel.EdgeDistanceName.Unsubscribe(_edgeDistance.SetName);
			_viewModel.EdgeDistanceValue.Unsubscribe(_edgeDistance.SetText);
			
			_viewModel.ForceName.Unsubscribe(_force.SetName);
			_viewModel.ForceValue.Unsubscribe(_force.SetText);

			_viewModel.RadiusActive.Unsubscribe(_radius.Activate);
			_viewModel.EdgeActive.Unsubscribe(_edgeDistance.Activate);
			_viewModel.ForceActive.Unsubscribe(_force.Activate);
			
			_dropdown.onValueChanged.RemoveListener(_viewModel.TypeChangeByView);
			_radius.OnValueChangedAction -= _viewModel.RadiusChangeByView;
			_edgeDistance.OnValueChangedAction -= _viewModel.EdgeChangeByView;
			_force.OnValueChangedAction -= _viewModel.ForceChangeByView;
		}
	}
}