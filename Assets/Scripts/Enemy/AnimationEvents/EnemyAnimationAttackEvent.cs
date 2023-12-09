using System;
using UnityEngine;

namespace Enemy.AnimationEvents
{
    public class EnemyAnimationAttackEvent : MonoBehaviour
    {
        public Action OnAttack;

        public void Attack()
        {
            OnAttack?.Invoke();
        }
    }
}