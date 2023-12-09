using System;
using System.Collections.Generic;
using Weapons.Enums;

namespace Weapons.ScriptableObjects
{
    [Serializable]
    public class TargetBaseWeaponProperty
    {
        public TargetBaseWeaponType WeaponType;
        public List<TargetBaseWeaponUpgradeData> UpgradeData;

        public TargetBaseWeaponUpgradeData GetUpgradeData(int level)
        {
            return UpgradeData[level - 1];
        }
    }
}