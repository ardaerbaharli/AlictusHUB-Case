namespace Weapons.TargetBaseWeapons.Ranged
{
    public class Wand : RangedWeapon
    {
        private void Awake()
        {
            AmmoPooledObjectType = PooledObjectType.WandAmmo;
        }
    }
}