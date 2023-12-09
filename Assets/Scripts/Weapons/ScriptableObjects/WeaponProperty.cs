using NaughtyAttributes;
using UnityEngine;
using Weapons.Enums;

namespace Weapons.ScriptableObjects
{
    [CreateAssetMenu(fileName = "WeaponProperty", menuName = "WeaponProperty", order = 0)]
    public class WeaponProperty : ScriptableObject
    {
        public WeaponType WeaponType;
        public int MaxLevel;

        [ShowIf("WeaponType", WeaponType.TargetBase)]
        public TargetBaseWeaponProperty TargetBaseWeaponProperty;

        [ShowIf("WeaponType", WeaponType.Area)]
        public AreaWeaponProperty AreaWeaponProperty;
    }
}