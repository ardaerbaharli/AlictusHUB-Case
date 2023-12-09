using NaughtyAttributes;
using UnityEngine;

namespace Player.Data
{
    [CreateAssetMenu(fileName = "PlayerHealthData", menuName = "PlayerHealthData", order = 0)]
    public class PlayerHealthData : ScriptableObject
    {
        public float maxHealth;

        [Header("Regeneration")] public bool hasRegeneration;
        [ShowIf("hasRegeneration")] public float regenerationPerSecond;
    }
}