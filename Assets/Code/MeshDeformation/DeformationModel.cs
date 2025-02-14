using Code.MeshDeformation.Infrastructure;
using Code.Utils;

namespace Code.MeshDeformation
{
	public class DeformationModel : IDeformationModel
	{
		public Observable<DeformType> DeformType { get; }
		public Observable<IDeformableMeshProvider> Mesh { get; }
		public Observable<float> BrushRadius { get; }
		public Observable<float> EdgeDistance { get; }
		public Observable<float> Force { get; }

		public DeformationModel(DeformationConfigSO config)
		{
			DeformType = new Observable<DeformType>(config.StartType);
			Mesh = new Observable<IDeformableMeshProvider>(null);
			BrushRadius = new Observable<float>(config.BrushRadius);
			EdgeDistance = new Observable<float>(config.EdgeDistance);
			Force = new Observable<float>(config.Force);
		}
	}
}