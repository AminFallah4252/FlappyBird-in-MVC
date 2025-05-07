using Models;

namespace Signals
{
    public struct ScoreUpdatedSignal
    {
        public int Score;
        public int BestScore;
    }

    public struct StartGameRequestSignal
    {
    }

    public struct GameOverSignal
    {
    }

    public struct GameStateChangedSignal
    {
        public GameState NewState;
    }

    public struct PlayerFlapSignal
    {
    }

    public struct AddScoreSignal
    {
    }
}