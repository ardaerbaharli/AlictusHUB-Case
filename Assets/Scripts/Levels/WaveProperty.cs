using Enemy;
using UnityEngine;
using Utils;

namespace Levels
{
    [CreateAssetMenu(fileName = "WaveProperty", menuName = "WaveProperty", order = 0)]
    public class WaveProperty : ScriptableObject
    {
        public float SpawnInterval;

        public DictionaryUnity<EnemyProperty, int> Enemies;
    }
}