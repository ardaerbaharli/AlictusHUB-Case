using System;
using System.Collections.Generic;
using Managers;
using Player.Data;
using UnityEngine;
using Weapons;
using Weapons.Enums;
using Weapons.TargetBaseWeapons;

namespace Player
{
    public enum PlayerState
    {
        Idle,
        Running,
        Dead
    }

    public class PlayerController : Target
    {
        private const float DamageMultiplier = 100;
        
        private static readonly int RunAnimation = Animator.StringToHash("Run");
        private static readonly int DieAnimation = Animator.StringToHash("Die");
        private static readonly int IdleAnimation = Animator.StringToHash("Idle");
        
        [SerializeField] private PlayerHealthData healthData;
        [SerializeField] public GameObject weaponParent;
        [SerializeField] private float attackSpeedMultiplier;
        [SerializeField] private TargetBaseWeaponType firstWeaponType;
        [SerializeField] private Transform rangeDisplay;

        public PlayerState state;
        private Animator animator;

        private TargetBaseWeapon firstWeapon;
        [NonSerialized] public HealthController HealthController;
        private List<Weapon> weapons;

        public static PlayerController Instance { get; private set; }

        private void Awake()
        {
            Instance = this;

            HealthController = GetComponent<HealthController>();
            HealthController.SetHealthData(healthData);
            HealthController.OnDied += () => SetState(PlayerState.Dead);

            weapons = new List<Weapon>();

            animator = GetComponentInChildren<Animator>();
        }

        private void Start()
        {
            firstWeapon = WeaponManager.Instance.AddWeapon(weaponParent, firstWeaponType, false, true);
            AddWeapon(firstWeapon);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (state == PlayerState.Dead) return;
            if (other.collider.CompareTag("Enemy")) HealthController.TakeDamage(HealthController.currentHealth);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (state == PlayerState.Dead) return;
            if (other.CompareTag("Enemy")) HealthController.TakeDamage(HealthController.currentHealth);
        }

        public void AddWeapon(Weapon weapon)
        {
            if (weapon.Type == WeaponType.TargetBase)
            {
                var targetBaseWeapon = (TargetBaseWeapon) weapon;
                targetBaseWeapon.WeaponTarget = WeaponTarget.Enemy;
                targetBaseWeapon.AttackInterval /= attackSpeedMultiplier;
                weapon.SetDamageMultiplier(DamageMultiplier / 100);

                rangeDisplay.localScale = Vector3.one * targetBaseWeapon.Range / 4;
            }

            weapons.Add(weapon);
            weapon.gameObject.SetActive(true);
        }

        public override void TakeDamage(float damage)
        {
            HealthController.TakeDamage(damage);
        }

        public void SetState(PlayerState playerState)
        {
            if (state == playerState) return;

            state = playerState;

            switch (state)
            {
                case PlayerState.Idle:
                    animator.SetTrigger(IdleAnimation);
                    break;
                case PlayerState.Running:
                    animator.SetTrigger(RunAnimation);
                    break;
                case PlayerState.Dead:
                    Die();
                    break;
            }
        }

        private void Die()
        {
            animator.SetTrigger(DieAnimation);
            GameManager.Instance.GameOver();
        }
    }
}