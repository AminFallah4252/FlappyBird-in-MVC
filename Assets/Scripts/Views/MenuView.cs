using System;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Views
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private GameObject gameOverBadge,newBestBadge;
        [SerializeField] private TMP_Text scoreText, bestScoreText;

        [Inject] private SignalBus signalBus;
        [InjectOptional] private DataModel data;

        private void Start()
        {
            startBtn.onClick.RemoveAllListeners();
            startBtn.onClick.AddListener(CallStartGame);
            SetPanel();
        }

        private void SetPanel()
        {
            if (data == null)
            {
                gameOverBadge.SetActive(false);
                newBestBadge.SetActive(false);
                scoreText.gameObject.SetActive(false);
                bestScoreText.gameObject.SetActive(false);
            }
            else
            {
                gameOverBadge.SetActive(true);
                scoreText.gameObject.SetActive(true);
                bestScoreText.gameObject.SetActive(true);

                scoreText.text = "Score: " + data.Score;
                bestScoreText.text = "Best: " + data.BestScore;
                
                newBestBadge.SetActive(data.Score >= data.BestScore);
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(scoreText.transform.parent as RectTransform);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        private void CallStartGame()
        {
            signalBus.Fire<StartGameRequestSignal>();
        }

        public class StartPanelFactory : PlaceholderFactory<MenuView>
        {
        }

        public class GameOverPanelFactory : PlaceholderFactory<DataModel, MenuView>
        {
        }

        public class DataModel
        {
            public int Score;
            public int BestScore;
        }
    }
}