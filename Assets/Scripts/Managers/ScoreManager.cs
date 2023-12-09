using System;
using UnityEngine;

namespace Managers
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance;
        private float score;
        public Action<float> OnScoreChanged;

        public float Score
        {
            get => score;
            set
            {
                score = value;
                OnScoreChanged?.Invoke(score);
            }
        }

        private void Awake()
        {
            Instance = this;
        }
    }
}