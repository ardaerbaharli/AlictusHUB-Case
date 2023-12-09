namespace Weapons.TargetBaseWeapons.Ranged
{
    public class Boomerang : RangedWeapon
    {
        private void Awake()
        {
            AmmoPooledObjectType = PooledObjectType.Boomerang;
        }
    }
}