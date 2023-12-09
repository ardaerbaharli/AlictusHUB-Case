using System;
using System.Collections;
using Collectables;
using Enemy.AnimationEvents;
using Managers;
using Player;
using UnityEngine;
using Weapons.Enums;

namespace Enemy
{
    public enum EnemyState
    {
        Running,
        Died,
        Attack
    }

    public class EnemyController : Target
    {
        private static readonly int DieAnimation = Animator.StringToHash("Die");
        private static readonly int RunAnimation = Animator.StringToHash("Run");
        private static readonly int AttackAnimation = Animator.StringToHash("Attack");
        
        [SerializeField] private GameObject weaponParent;
        [SerializeField] private Transform rangeDisplay;

        public EnemyState state;
        public PooledObject pooledObject;
        public PlayerController player;
        public bool isDead;

        private bool cached;

        private Animator animator;
        private EnemyAnimationStartRunningEvent enemyAnimationStartRunningEvent;
        private EnemyAttackManager enemyAttackManager;
        private EnemyMovementManager enemyMovementManager;
        private HealthController healthController;

        private EnemyProperty enemyProperty;

        public Action OnEnemyDied;
        private float rangeAdvantage;

        private void Awake()
        {
            enemyAttackManager = GetComponent<EnemyAttackManager>();

            enemyMovementManager = GetComponent<EnemyMovementManager>();
            animator = GetComponentInChildren<Animator>();

            healthController = GetComponent<HealthController>();
            healthController.OnDied += Die;

            enemyAnimationStartRunningEvent = GetComponentInChildren<EnemyAnimationStartRunningEvent>();
            enemyAnimationStartRunningEvent.OnStartRunning += StartFollowing;
        }

        private void OnDisable()
        {
            if (!cached) return;
            StopAllCoroutines();

            if (animator != null)
                animator.enabled = true;
        }


        public void SetState(EnemyState s)
        {
            state = s;
            switch (state)
            {
                case EnemyState.Running:
                    animator.SetTrigger(RunAnimation);
                    // StartFollowing();
                    break;
                case EnemyState.Died:
                    animator.SetTrigger(DieAnimation);
                    break;
                case EnemyState.Attack:
                    animator.SetTrigger(AttackAnimation);
                    StartCoroutine(LookAtTarget());
                    StopFollowing();
                    break;
            }
        }

        private IEnumerator LookAtTarget()
        {
            while (state == EnemyState.Attack)
            {
                transform.LookAt(player.transform);
                yield return null;
            }
        }

        public void StartFollowing()
        {
            enemyMovementManager.StartFollowing(player, enemyProperty.speed);
        }


        public void StopFollowing()
        {
            enemyMovementManager.StopFollowing();
        }

        public void ReturnToPool()
        {
            pooledObject.ReturnToPool();
        }

        public void SetEnemyProperty(EnemyProperty enemyProperty, PlayerController playerController)
        {
            this.enemyProperty = enemyProperty;
            healthController.maxHealth = enemyProperty.health;
            healthController.currentHealth = healthController.maxHealth;

            var w = WeaponManager.Instance.AddWeapon(weaponParent, enemyProperty.weaponType,
                enemyProperty.attackWithAnimationTiming, true);
            w.Damage *= enemyProperty.damageMultiplier;
            w.Range *= enemyProperty.rangeMultiplier;
            w.WeaponTarget = WeaponTarget.Player;
            w.gameObject.SetActive(true);

            rangeDisplay.localScale = Vector3.one * w.Range / 4;
            player = playerController;

            enemyAttackManager.SetEnemyProperty();

            cached = true;
        }

        public override void TakeDamage(float damage)
        {
            healthController.TakeDamage(damage);
        }

        private void Die()
        {
            LevelManager.Instance.PointsReceived(healthController.maxHealth);
            ScoreManager.Instance.Score++;
            CollectableManager.Instance.SpawnCollectable(CollectableType.Coin, transform.position);
            isDead = true;
            SetState(EnemyState.Died);

            OnEnemyDied?.Invoke();
            // TODO: Delay returning for animation
            ReturnToPool();
        }
    }
}