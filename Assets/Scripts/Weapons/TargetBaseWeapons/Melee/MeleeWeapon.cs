using System.Collections;
using UnityEngine;

namespace Weapons.TargetBaseWeapons.Melee
{
    public class MeleeWeapon : TargetBaseWeapon
    {
        public override void Attack()
        {
            StartCoroutine(StartAttacking());
        }

        protected override void SetSpecificSettings()
        {
        }

        private IEnumerator StartAttacking()
        {
            while (true)
            {
                yield return new WaitForSeconds(AttackInterval);
                if (targetInRange)
                    target.TakeDamage(Damage * DamageMultiplier);
            }
        }
    }
}