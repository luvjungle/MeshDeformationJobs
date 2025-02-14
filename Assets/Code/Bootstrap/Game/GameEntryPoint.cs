using VContainer.Unity;

namespace Code.Bootstrap.Game
{
	public class GameEntryPoint : IStartable
	{
		private readonly MonoGameEntryPoint _entryPoint;
		
		public GameEntryPoint(MonoGameEntryPoint entryPoint)
		{
			_entryPoint = entryPoint;
		}

		public void Start()
		{
			_entryPoint.OnStart();
		}
	}
}