using System;
using System.Collections;
using Player;
using UnityEngine;
using Weapons.Enums;
using Weapons.ScriptableObjects;

namespace Weapons.TargetBaseWeapons
{
    public abstract class TargetBaseWeapon : Weapon
    {
        public bool targetInRange;
        public Target target;
        public Transform weaponHolder;

        public float Damage;
        [SerializeField] public float Range;
        private bool attackWithAnimationTiming;

        private LayerMask detectionLayer;

        private Collider[] results;
        public Action<bool> TargetInRangeChanged;
        private WeaponTarget weaponTarget;


        public WeaponTarget WeaponTarget
        {
            get => weaponTarget;
            set
            {
                weaponTarget = value;
                detectionLayer = WeaponTarget == WeaponTarget.Enemy
                    ? LayerMask.GetMask("Enemy")
                    : LayerMask.GetMask("Player");

                if (WeaponTarget == WeaponTarget.Player)
                    target = PlayerController.Instance;
            }
        }

        public float Speed { get; private set; }
        public float AttackInterval { get; set; }


        public void OnEnable()
        {
            results = new Collider[10];
            StartCoroutine(TargetInRangeCoroutine());
            if (!attackWithAnimationTiming)
                StartCoroutine(AttackCoroutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(weaponHolder.position, Range);
        }

        private IEnumerator AttackCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(AttackInterval);
                if (targetInRange) Attack();
            }
        }

        private IEnumerator TargetInRangeCoroutine()
        {
            targetInRange = false;

            while (true)
            {
                yield return new WaitForSeconds(0.1f);
                // check if target is in range by using overlap circle for 3d  
                var size = Physics.OverlapSphereNonAlloc(weaponHolder.position, Range, results, detectionLayer);
                if (size > 0)
                {
                    var col = results[0];
                    if (WeaponTarget == WeaponTarget.Enemy)
                        target = col.GetComponent<Target>();
                    if (!targetInRange)
                    {
                        targetInRange = true;
                        TargetInRangeChanged?.Invoke(true);
                    }
                }
                else
                {
                    if (targetInRange)
                    {
                        targetInRange = false;
                        TargetInRangeChanged?.Invoke(false);
                    }
                }
            }
        }

        public abstract void Attack();

        public void SetWeapon(WeaponProperty property, bool attackWithAnimationTiming)
        {
            Property = property;
            Type = WeaponType.TargetBase;
            TargetBaseWeaponType = property.TargetBaseWeaponProperty.WeaponType;

            Level = 1;
            MaxLevel = Property.MaxLevel;

            var data = Property.TargetBaseWeaponProperty.GetUpgradeData(Level);
            SetData(data);
            SetSpecificSettings();

            weaponHolder = GetComponentInParent<Target>().transform;
            this.attackWithAnimationTiming = attackWithAnimationTiming;
        }

        protected abstract void SetSpecificSettings();

        private void SetData(TargetBaseWeaponUpgradeData data)
        {
            Damage = data.Damage;
            Speed = data.Speed;
            Range = data.Range;
            AttackInterval = data.AttackInterval;
            UpgradeDescription = data.UpgradeDescription;
        }

        public override void Upgrade()
        {
            Level++;
            var upgradeData = Property.TargetBaseWeaponProperty.GetUpgradeData(Level);
            SetData(upgradeData);
        }
    }
}