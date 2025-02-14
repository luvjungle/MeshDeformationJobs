using System;
using System.Collections.Generic;
using System.Linq;
using Code.MeshDeformation.Infrastructure;
using Code.Utils;

namespace Code.MeshDeformation.UI
{
	public class MeshDeformViewModel : IDisposable, IMeshDeformViewModel
	{
		public Observable<string> DropDownName { get; }
		public Observable<List<string>> DropDownOptions { get; }
		public Observable<int> DropDownValue { get; }

		public Observable<string> RadiusName { get; }
		public Observable<float> RadiusValue { get; }

		public Observable<string> EdgeDistanceName { get; }
		public Observable<float> EdgeDistanceValue { get; }

		public Observable<string> ForceName { get; }
		public Observable<float> ForceValue { get; }

		public Observable<bool> RadiusActive { get; }
		public Observable<bool> EdgeActive { get; }
		public Observable<bool> ForceActive { get; }

		private readonly IDeformationModel _model;

		public MeshDeformViewModel(IDeformationModel model)
		{
			_model = model;

			DropDownName = new Observable<string>("Type");
			DropDownOptions = new Observable<List<string>>(Enum.GetNames(typeof(DeformType)).ToList());
			DropDownValue = new Observable<int>((int)_model.DeformType.Value);

			RadiusName = new Observable<string>("Radius");
			RadiusValue = new Observable<float>(_model.BrushRadius.Value);

			EdgeDistanceName = new Observable<string>("Edge Distance");
			EdgeDistanceValue = new Observable<float>(_model.EdgeDistance.Value);

			ForceName = new Observable<string>("Force");
			ForceValue = new Observable<float>(_model.Force.Value);

			RadiusActive = new Observable<bool>(true);
			EdgeActive = new Observable<bool>(DropDownValue.Value == (int)DeformType.MoveVertexDown);
			ForceActive = new Observable<bool>(DropDownValue.Value == (int)DeformType.PushVertexOutside);

			_model.DeformType.Subscribe(TypeChangedByModel, true);
			_model.BrushRadius.Subscribe(RadiusValueChangedByModel, true);
			_model.EdgeDistance.Subscribe(EdgeDistanceValueChangedByModel, true);
			_model.Force.Subscribe(ForceValueChangedByModel, true);
		}

		private void TypeChangedByModel(DeformType type)
		{
			DropDownValue.Value = (int)type;
			EdgeActive.Value = type == DeformType.MoveVertexDown;
			ForceActive.Value = type == DeformType.PushVertexOutside;
		}

		private void RadiusValueChangedByModel(float value) => RadiusValue.Value = value;
		private void EdgeDistanceValueChangedByModel(float value) => EdgeDistanceValue.Value = value;
		private void ForceValueChangedByModel(float value) => ForceValue.Value = value;

		public void TypeChangeByView(int value) => _model.DeformType.Value = (DeformType)value;
		public void RadiusChangeByView(float value) => _model.BrushRadius.Value = value;
		public void EdgeChangeByView(float value) => _model.EdgeDistance.Value = value;
		public void ForceChangeByView(float value) => _model.Force.Value = value;

		public void Dispose()
		{
			_model.DeformType.Unsubscribe(TypeChangedByModel);
			_model.BrushRadius.Unsubscribe(RadiusValueChangedByModel);
			_model.EdgeDistance.Unsubscribe(EdgeDistanceValueChangedByModel);
			_model.Force.Unsubscribe(ForceValueChangedByModel);
		}
	}
}