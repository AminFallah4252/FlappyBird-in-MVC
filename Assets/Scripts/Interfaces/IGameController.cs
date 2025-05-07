namespace Interfaces
{
    public interface IGameController
    {
        void StartGame();
        void GameOver();
        void AddScore(int value);
    }
}