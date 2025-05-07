using Signals;

namespace Interfaces
{
    public interface IPipesController
    {
        void OnGameStateChanged(GameStateChangedSignal signal);
        void SpawnPipe();
        void MovePipes();
    }
}