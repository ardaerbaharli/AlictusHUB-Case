using UnityEngine;
using Weapons.Enums;

namespace Weapons.Ammos
{
    public class Ammo : MonoBehaviour
    {
        public PooledObject PooledObject;
        private float _damage;
        private float _range;
        private Rigidbody _rigidbody;
        private float _speed;
        private WeaponTarget _weaponTarget;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Ground"))
            {
                ReturnToPool();
                return;
            }

            if (_weaponTarget == WeaponTarget.Player)
            {
                if (!other.CompareTag("Player")) return;
                other.GetComponent<Target>().TakeDamage(_damage);
                ReturnToPool();
            }
            else if (_weaponTarget == WeaponTarget.Enemy)
            {
                if (!other.CompareTag("Enemy")) return;
                other.GetComponent<Target>().TakeDamage(_damage);
                ReturnToPool();
            }
        }

        public void SetAmmoProperty(float damage, float speed, float range, WeaponTarget weaponTarget)
        {
            _damage = damage;
            _speed = speed;
            _range = range;
            _weaponTarget = weaponTarget;
        }

        public void ReturnToPool()
        {
            StopAllCoroutines();
            PooledObject.ReturnToPool();
        }

        public virtual void Shoot(Vector3 direction)
        {
            _rigidbody.velocity = direction * _speed;
        }
    }
}