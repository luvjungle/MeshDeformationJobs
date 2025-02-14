using System.Collections.Generic;
using Code.Utils;

namespace Code.MeshDeformation.Infrastructure
{
	public interface IMeshDeformViewModel
	{
		Observable<string> DropDownName { get; }
		Observable<List<string>> DropDownOptions { get; }
		Observable<int> DropDownValue { get; }

		Observable<string> RadiusName { get; }
		Observable<float> RadiusValue { get; }

		Observable<string> EdgeDistanceName { get; }
		Observable<float> EdgeDistanceValue { get; }

		Observable<string> ForceName { get; }
		Observable<float> ForceValue { get; }

		Observable<bool> RadiusActive { get; }
		Observable<bool> EdgeActive { get; }
		Observable<bool> ForceActive { get; }

		void TypeChangeByView(int value);
		void RadiusChangeByView(float value);
		void EdgeChangeByView(float value);
		void ForceChangeByView(float value);
	}
}