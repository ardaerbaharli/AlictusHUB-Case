using System;

namespace Weapons.ScriptableObjects
{
    [Serializable]
    public class TargetBaseWeaponUpgradeData
    {
        public string UpgradeDescription;
        public float Damage;
        public float Speed;
        public float Range;
        public float AttackInterval;
    }
}