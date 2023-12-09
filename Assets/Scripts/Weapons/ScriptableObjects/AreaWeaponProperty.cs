using System;
using System.Collections.Generic;
using UnityEngine;
using Weapons.Enums;

namespace Weapons.ScriptableObjects
{
    [Serializable]
    public class AreaWeaponProperty
    {
        public AreaWeaponType WeaponType;
        public GameObject WeaponPrefab;
        public List<AreaWeaponUpgradeData> UpgradeData;

        public AreaWeaponUpgradeData GetUpgradeData(int level)
        {
            return UpgradeData[level - 1];
        }
    }
}