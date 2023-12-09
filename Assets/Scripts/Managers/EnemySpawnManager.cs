using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemy;
using Enemy.EnemyTypes;
using Levels;
using Player;
using UnityEngine;
using Utils;

namespace Managers
{
    public class EnemySpawnManager : MonoBehaviour
    {
        public static EnemySpawnManager Instance;
        [SerializeField] private LevelProperty levelProperty;

        private List<EnemyController> activeEnemies;

        private bool isGameOver;

        private PlayerController player;

        private void Awake()
        {
            Instance = this;
            activeEnemies = new List<EnemyController>();
            isGameOver = false;
        }

        private void Start()
        {
            GameManager.Instance.OnGameStarted += OnGameStarted;
            GameManager.Instance.OnGameStateChanged += OnGameStateChanged;
        }

        private void OnGameStateChanged(GameState state)
        {
            if (state == GameState.GameOver)
            {
                isGameOver = true;
                activeEnemies.ForEach(enemy => enemy.ReturnToPool());
            }
        }

        private void OnGameStarted()
        {
            player = PlayerController.Instance;
            StartCoroutine(StartWaves());
        }

        private IEnumerator StartWaves()
        {
            if (levelProperty.infiniteWaves)
            {
                var wave = levelProperty.waves[0];
                while (true)
                {
                    if (isGameOver) yield break;
                    yield return StartCoroutine(StartWave(wave));
                    yield return new WaitForSeconds(levelProperty.interval);
                }
            }

            foreach (var wave in levelProperty.waves)
            {
                if (isGameOver) yield break;
                yield return StartCoroutine(StartWave(wave));
                yield return new WaitForSeconds(levelProperty.interval);
            }
        }

        private IEnumerator StartWave(WaveProperty wave)
        {
            var enemies = wave.Enemies.dictionary;

            foreach (var enemy in enemies)
                for (var i = 0; i < enemy.value; i++)
                {
                    if (isGameOver) yield break;
                    SpawnEnemy(enemy.key);
                    yield return new WaitForSeconds(wave.SpawnInterval);
                }
        }

        private void SpawnEnemy(EnemyProperty enemyProperty)
        {
            var pooledObjectType = GetPooledObjectType(enemyProperty.type);
            var enemyPooledObject = ObjectPool.Instance.GetPooledObject(pooledObjectType);
            var enemy = enemyPooledObject.gameObject.GetComponent<EnemyController>();
            enemy.pooledObject = enemyPooledObject;

            var position = GetPosition();
            enemy.transform.position = position;

            activeEnemies.Add(enemy);

            enemy.OnEnemyDied += () => activeEnemies.Remove(enemy);
            enemy.isDead = false;
            enemy.gameObject.SetActive(true);

            enemy.SetEnemyProperty(enemyProperty, player);
        }

        private PooledObjectType GetPooledObjectType(EnemyType enemyPropertyType)
        {
            switch (enemyPropertyType)
            {
                case EnemyType.SkeletonBrute:
                    return PooledObjectType.SkeletonBrute;
            }

            return PooledObjectType.SkeletonBrute;
        }

        private Vector3 GetPosition()
        {
            // random position must be out of the screen in 3d space with max distance of 35 and min distance of 25
            var origin = PlayerController.Instance.transform.position;
            var randomPosition = Random.insideUnitCircle.normalized * Random.Range(25, 35);
            var position = new Vector3(randomPosition.x, 0f, randomPosition.y) + origin;
            return position;

            
        }

        public Vector3 GetClosestEnemyDirection(Vector3 target)
        {
            var closestEnemy = activeEnemies.Where(x => !x.isDead)
                .OrderBy(enemy => Vector3.Distance(enemy.transform.position, target))
                .FirstOrDefault();
            if (closestEnemy == null) return Vector3.zero;

            return (closestEnemy.transform.position - target).normalized;
        }
    }
}