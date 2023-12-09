using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Weapons.Enums;
using Weapons.ScriptableObjects;
using Weapons.TargetBaseWeapons;
using Weapons.TargetBaseWeapons.Melee;
using Weapons.TargetBaseWeapons.Ranged;

namespace Managers
{
    public class WeaponManager : MonoBehaviour
    {
        public static WeaponManager Instance;
        private List<WeaponProperty> _weaponProperties;

        private void Awake()
        {
            Instance = this;
            _weaponProperties = new List<WeaponProperty>();

            // load weaponproperties from Resources/WeaponProperties
            var weaponProperties = Resources.LoadAll<WeaponProperty>("WeaponProperties");
            _weaponProperties.AddRange(weaponProperties);
        }

        public TargetBaseWeapon AddWeapon(GameObject to, TargetBaseWeaponType type,
            bool attackWithAnimationTiming = false, bool asChild = false)
        {
            Type t;
            switch (type)
            {
                case TargetBaseWeaponType.Bow:
                    t = typeof(Bow);
                    break;
                case TargetBaseWeaponType.Wand:
                    t = typeof(Wand);
                    break;
                case TargetBaseWeaponType.Axe:
                case TargetBaseWeaponType.Mace:
                    t = typeof(MeleeWeapon);
                    break;
                case TargetBaseWeaponType.Boomerang:
                    t = typeof(Boomerang);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            TargetBaseWeapon w;
            var pooledObjectType = GetPooledObjectType(type);
            var weapon = ObjectPool.Instance.GetPooledObject(pooledObjectType);
            if (asChild)
            {
                weapon.transform.SetParent(to.transform, false);
                weapon.transform.localPosition = Vector3.zero;
                w = (TargetBaseWeapon) weapon.gameObject.AddComponent(t);
            }
            else
            {
                w = (TargetBaseWeapon) weapon.gameObject.AddComponent(t);
            }


            w.SetWeapon(GetWeaponProperty(type), attackWithAnimationTiming);
            return w;
        }

        private PooledObjectType GetPooledObjectType(TargetBaseWeaponType type)
        {
            switch (type)
            {
                case TargetBaseWeaponType.Bow:
                    return PooledObjectType.Bow;
                case TargetBaseWeaponType.Wand:
                    return PooledObjectType.Wand;
                case TargetBaseWeaponType.Axe:
                    return PooledObjectType.Axe;
                case TargetBaseWeaponType.Mace:
                    return PooledObjectType.Mace;
                case TargetBaseWeaponType.Boomerang:
                    return PooledObjectType.BoomerangParent;

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        private WeaponProperty GetWeaponProperty(AreaWeaponType type)
        {
            return _weaponProperties.Where(x => x.WeaponType == WeaponType.Area).FirstOrDefault(weaponProperty =>
                weaponProperty.AreaWeaponProperty.WeaponType == type);
        }

        private WeaponProperty GetWeaponProperty(TargetBaseWeaponType type)
        {
            return _weaponProperties.Where(x => x.WeaponType == WeaponType.TargetBase).FirstOrDefault(weaponProperty =>
                weaponProperty.TargetBaseWeaponProperty.WeaponType == type);
        }

        public List<WeaponProperty> GetWeaponProperties()
        {
            return _weaponProperties.ToList();
        }
    }
}