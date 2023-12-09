using System;
using UnityEngine;

namespace Enemy.AnimationEvents
{
    public class EnemyAnimationStartRunningEvent : MonoBehaviour
    {
        public Action OnStartRunning;

        public void StartRunning()
        {
            OnStartRunning?.Invoke();
        }
    }
}