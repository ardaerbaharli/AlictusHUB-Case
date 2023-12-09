using UnityEngine;

namespace Collectables
{
    [CreateAssetMenu(fileName = "CollectableProperty", menuName = "CollectableProperty", order = 0)]
    public class CollectableProperty : ScriptableObject
    {
        public CollectableType type;
        public Vector3 colliderSize;
        [Header("Specific Properties")] public float coinValue;
        public int healAmount;
    }
}