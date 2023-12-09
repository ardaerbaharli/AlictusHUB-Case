using Managers;
using Player;
using UnityEngine;
using Weapons.Ammos;
using Weapons.Enums;

namespace Weapons.TargetBaseWeapons.Ranged
{
    public class RangedWeapon : TargetBaseWeapon
    {
        private PlayerController _player;
        protected PooledObjectType AmmoPooledObjectType;
        private Transform ammoSpawnPoint;

        public PlayerController Player
        {
            get
            {
                if (_player == null) _player = PlayerController.Instance;
                return _player;
            }
        }


        public override void Attack()
        {
            var projectilePooledObject = ObjectPool.Instance.GetPooledObject(AmmoPooledObjectType);
            var projectile = projectilePooledObject.gameObject.GetComponent<Ammo>();
            projectile.PooledObject = projectilePooledObject;

            projectile.SetAmmoProperty(Damage * DamageMultiplier, Speed, Range, WeaponTarget);
            projectile.transform.position = ammoSpawnPoint.position;

            Vector3 direction;
            if (WeaponTarget == WeaponTarget.Enemy)
            {
                direction = EnemySpawnManager.Instance.GetClosestEnemyDirection(Player.transform.position);
                if (direction == Vector3.zero)
                {
                    projectilePooledObject.ReturnToPool();
                    return;
                }
            }
            else
            {
                direction = (Player.transform.position - transform.position).normalized;
            }


            projectile.gameObject.SetActive(true);
            projectile.Shoot(direction);
        }

        protected override void SetSpecificSettings()
        {
            ammoSpawnPoint = GetComponentInChildren<AmmoSpawnPoint>().transform;
        }
    }
}