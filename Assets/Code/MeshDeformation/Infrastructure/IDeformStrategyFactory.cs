namespace Code.MeshDeformation.Infrastructure
{
	public interface IDeformStrategyFactory
	{
		IDeformStrategy GetStrategy(DeformType type);
	}
}