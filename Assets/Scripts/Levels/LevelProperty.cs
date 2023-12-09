using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = "LevelProperty", menuName = "LevelProperty", order = 0)]
    public class LevelProperty : ScriptableObject
    {
        public List<WaveProperty> waves;
        public float interval;
        public bool infiniteWaves;
    }
}