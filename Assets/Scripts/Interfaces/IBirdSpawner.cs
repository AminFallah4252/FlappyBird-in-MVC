using Signals;

namespace Interfaces
{
    public interface IBirdSpawner
    {
        void OnGameStateChanged(GameStateChangedSignal obj);
        void Spawn();
    }
}