using System;
using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public enum GameState
    {
        Waiting,
        Playing,
        Paused,
        GameOver
    }

    public class GameManager : MonoBehaviour
    {
        public bool startOnAwake;

        public Action<GameState> OnGameStateChanged;
        public static GameManager Instance { get; private set; }
        public GameState gameState { get; private set; }
        public Action OnGameStarted { get; set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            if (startOnAwake) StartGame();
        }


        public void SetGameState(GameState state)
        {
            gameState = state;
            OnGameStateChanged?.Invoke(state);
        }

        [Button]
        public void StartGame()
        {
            OnGameStarted?.Invoke();
        }

        [Button]
        public void GameOver()
        {
            SetGameState(GameState.GameOver);
        }


        [Button]
        public void RestartGame()
        {
            SceneManager.LoadScene(0);
        }
    }
}