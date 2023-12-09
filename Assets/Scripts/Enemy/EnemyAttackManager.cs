using Enemy.AnimationEvents;
using UnityEngine;
using Weapons.TargetBaseWeapons;

namespace Enemy
{
    public class EnemyAttackManager : MonoBehaviour
    {
        public TargetBaseWeapon weapon;


        private EnemyAnimationAttackEvent enemyAnimationAttackEvent;
        private EnemyController enemyController;

        private void Awake()
        {
            enemyController = GetComponent<EnemyController>();
            enemyAnimationAttackEvent = GetComponentInChildren<EnemyAnimationAttackEvent>();
        }

        public void SetEnemyProperty()
        {
            weapon = GetComponentInChildren<TargetBaseWeapon>();

            weapon.TargetInRangeChanged += OnTargetInRangeChanged;
            enemyAnimationAttackEvent.OnAttack += weapon.Attack;
            enemyController.SetState(EnemyState.Running);
        }

        private void OnTargetInRangeChanged(bool value)
        {
            if (value)
                enemyController.SetState(EnemyState.Attack);
            else
                enemyController.SetState(EnemyState.Running);
        }
    }
}