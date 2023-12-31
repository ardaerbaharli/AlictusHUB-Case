using UnityEngine;
using Weapons.Enums;
using Weapons.ScriptableObjects;

namespace Weapons
{
    public abstract class Weapon : MonoBehaviour
    {
        public string Name;
        public string UpgradeDescription;
        public WeaponType Type;
        public AreaWeaponType AreaWeaponType;
        public TargetBaseWeaponType TargetBaseWeaponType;
        public int Level;
        public int MaxLevel;

        public WeaponProperty Property;

        public float DamageMultiplier = 1f;

        public virtual void SetDamageMultiplier(float multiplier)
        {
            DamageMultiplier = multiplier;
        }

        public abstract void Upgrade();
    }
}