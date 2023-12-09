using System;
using UnityEngine;

namespace Managers
{
    public class MoneyManager : MonoBehaviour
    {
        private const string MoneyPlayerPref = "Money";
        public static MoneyManager Instance;
        private float money;

        public Action<float> OnMoneyChanged;

        public float Money
        {
            get => money;
            set
            {
                money = value;
                PlayerPrefs.SetFloat(MoneyPlayerPref, money);
                OnMoneyChanged?.Invoke(money);
            }
        }

        private void Awake()
        {
            Instance = this;
            Money = PlayerPrefs.GetFloat(MoneyPlayerPref, 0);
        }
    }
}