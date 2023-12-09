using Managers;
using TMPro;
using UnityEngine;

namespace UI
{
    public class GamePanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinText;
        [SerializeField] private TextMeshProUGUI killsText;
        [SerializeField] private GameObject restartPanel;


        private void Start()
        {
            coinText.text = MoneyManager.Instance.Money.ToString("0");
            killsText.text = ScoreManager.Instance.Score.ToString("0");

            MoneyManager.Instance.OnMoneyChanged += OnMoneyChanged;
            ScoreManager.Instance.OnScoreChanged += OnScoreChanged;
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState obj)
        {
            if (obj == GameState.GameOver)
            {
                restartPanel.SetActive(true);
            }
        }

        private void OnScoreChanged(float kills)
        {
            killsText.text = kills.ToString("0");
        }

        private void OnMoneyChanged(float coins)
        {
            coinText.text = coins.ToString("0");
        }

        public void RestartButton()
        {
            GameManager.Instance.RestartGame();
        }
    }
}