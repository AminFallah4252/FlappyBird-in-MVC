using Signals;

namespace Interfaces
{
    public interface IMenuController
    {
        void OnGameStateChanged(GameStateChangedSignal signal);
    }
}