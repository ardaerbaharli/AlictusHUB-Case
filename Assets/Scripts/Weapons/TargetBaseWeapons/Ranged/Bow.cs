namespace Weapons.TargetBaseWeapons.Ranged
{
    public class Bow : RangedWeapon
    {
        private void Awake()
        {
            AmmoPooledObjectType = PooledObjectType.Arrow;
        }
    }
}