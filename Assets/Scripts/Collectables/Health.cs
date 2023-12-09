using Player;

namespace Collectables
{
    public class Health : Collectable
    {
        public int HealAmount;

        public override void SetSpecificProperty(CollectableProperty property)
        {
            HealAmount = property.healAmount;
        }

        public override void GetCollected()
        {
            PlayerController.Instance.HealthController.Heal(HealAmount);
        }
    }
}