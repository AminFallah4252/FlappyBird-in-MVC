using Controllers;
using TMPro;
using UnityEngine;
using Zenject;
using Signals;

namespace Views
{
    public class HUDView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText, bestScoreText;

        [Inject] private SignalBus signalBus;
        [Inject] private DataModel data;

        private void Start()
        {
            signalBus.Subscribe<ScoreUpdatedSignal>(UpdateScore);
            scoreText.text = $"Score: 0";
            bestScoreText.text = $"Best: {data.BestScore}";
        }

        private void OnDestroy()
        {
            signalBus.Unsubscribe<ScoreUpdatedSignal>(UpdateScore);
        }

        private void UpdateScore(ScoreUpdatedSignal signal)
        {
            scoreText.text = $"Score: {signal.Score}";
            bestScoreText.text = $"Best: {signal.BestScore}";
        }

        public class Factory : PlaceholderFactory<DataModel, HUDView>
        {
        }

        public struct DataModel
        {
            public int BestScore;
        }
    }
}