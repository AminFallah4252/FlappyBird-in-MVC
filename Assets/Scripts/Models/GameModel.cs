using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Models
{
    [CreateAssetMenu(fileName = "GameModel", menuName = "FlappyBird/GameModel")]
    public class GameModel : ScriptableObject
    {
        private const string BestScoreKey = "BestScore";
        public GameState CurrentState { get; set; } = GameState.Waiting;

        public int BestScore
        {
            get => PlayerPrefs.GetInt(BestScoreKey, 0);
            private set => PlayerPrefs.SetInt(BestScoreKey, value);
        }

        public int Score { get; private set; } = 0;

        public void ResetScore()
        {
            Score = 0;
        }

        public void AddScore()
        {
            Score++;
            SetBestScore();
        }

        public void AddScore(int value)
        {
            Score += value;
            SetBestScore();
        }

        private void SetBestScore()
        {
            if (Score > BestScore)
                BestScore = Score;
        }
    }

    public enum GameState
    {
        Waiting,
        Playing,
        GameOver
    }
}