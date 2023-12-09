using System;
using System.Collections;
using System.Collections.Generic;
using Collectables;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Managers
{
    public class CollectableManager : MonoBehaviour
    {
        public static CollectableManager Instance;
        private List<CollectableProperty> collectableProperties;
        [SerializeField] private int initialNumberOfCoins;
        

        private void Awake()
        {
            Instance = this;
            collectableProperties = new List<CollectableProperty>();
            collectableProperties.AddRange(Resources.LoadAll<CollectableProperty>("CollectableProperties"));
        }


        private IEnumerator Start()
        {
            yield return new WaitUntil(() => ObjectPool.IsSet);
            for (var i = 0; i < initialNumberOfCoins; i++)
            {
                var pos = GetPosition();
                pos.y = 1.5f;
                SpawnCollectable(CollectableType.Coin, pos);
            }
        }

        public void SpawnCollectable(CollectableType type, Vector3 pos)
        {
            var pooledObject = GetPooledObjectType(type);

            var collectablePooledObject = ObjectPool.Instance.GetPooledObject(pooledObject);
            collectablePooledObject.transform.position = pos;

            Collectable collectable = null;
            switch (type)
            {
                case CollectableType.Health:
                    collectable = collectablePooledObject.gameObject.AddComponent<Health>();
                    break;
                case CollectableType.Coin:
                    collectable = collectablePooledObject.gameObject.AddComponent<Coin>();
                    break;
            }

            if (collectable == null)
            {
                collectablePooledObject.ReturnToPool();
                return;
            }

            collectable.pooledObject = collectablePooledObject;
            collectable.gameObject.SetActive(true);
            collectable.SetCollectableProperty(GetProperty(type));
        }

        private PooledObjectType GetPooledObjectType(CollectableType type)
        {
            switch (type)
            {
                case CollectableType.Health:
                    return PooledObjectType.HealthCollectable;
                case CollectableType.Coin:
                    return PooledObjectType.CoinCollectable;
            }

            return PooledObjectType.Collectable;
        }

        private CollectableProperty GetProperty(CollectableType type)
        {
            return collectableProperties.Find(x => x.type == type);
        }
        
        private Vector3 GetPosition()
        {
            // random position must be out of the screen in 3d space with max distance of 25 and min distance of 15
            var origin = PlayerController.Instance.transform.position;
            var randomPosition = Random.insideUnitCircle.normalized * Random.Range(15, 25);
            var position = new Vector3(randomPosition.x, 0f, randomPosition.y) + origin;
            return position;

            
        }

    }
}