using Models;
using Signals;

namespace Interfaces
{
    public interface IHUDController
    {
        void OnGameStateChanged(GameStateChangedSignal state);
    }
}