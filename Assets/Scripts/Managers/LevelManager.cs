using System;
using NaughtyAttributes;
using UnityEngine;

namespace Managers
{
    public class LevelManager : MonoBehaviour
    {
        public static LevelManager Instance;
        [SerializeField] private float pointsToLevelUp;
        private int level;

        private float previousPointsToLevelUp;
        private float remainingPointsToLevelUp;

        public Action<int> OnLevelUp;
        public Action<float> RemainingPointsChanged;

        public int Level
        {
            get => level;
            private set
            {
                level = value;
                OnLevelUp?.Invoke(level);
            }
        }

        private void Awake()
        {
            Instance = this;
            Level = 1;

            remainingPointsToLevelUp = pointsToLevelUp;
            previousPointsToLevelUp = pointsToLevelUp;
        }

        public void PointsReceived(float points)
        {
            remainingPointsToLevelUp -= points;
            var remainingPercentage = remainingPointsToLevelUp / previousPointsToLevelUp;
            RemainingPointsChanged?.Invoke(remainingPercentage);
            if (remainingPointsToLevelUp <= 0) LevelUp();
        }

        [Button]
        private void LevelUp()
        {
            Level++;
            remainingPointsToLevelUp = previousPointsToLevelUp * 1.3f;
            previousPointsToLevelUp = remainingPointsToLevelUp;
            // UpgradeManager.Instance.ShowUpgradeMenu();
        }
    }
}