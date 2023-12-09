using Enemy.EnemyTypes;
using UnityEngine;
using Weapons.Enums;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyProperty", menuName = "EnemyProperty", order = 0)]
    public class EnemyProperty : ScriptableObject
    {
        public EnemyType type;
        public float health;
        public float speed;
        public TargetBaseWeaponType weaponType;
        public float damageMultiplier = 1;
        public float rangeMultiplier = 1.2f;
        public bool attackWithAnimationTiming;
    }
}