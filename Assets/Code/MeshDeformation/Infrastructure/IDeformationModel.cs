using Code.Utils;

namespace Code.MeshDeformation.Infrastructure
{
	public interface IDeformationModel
	{
		Observable<DeformType> DeformType { get; }
		Observable<IDeformableMeshProvider> Mesh { get; }
		Observable<float> BrushRadius { get; }
		Observable<float> EdgeDistance { get; }
		Observable<float> Force { get; }
	}
}