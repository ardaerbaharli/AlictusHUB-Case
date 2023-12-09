using System.Collections;
using UnityEngine;

namespace Collectables
{
    public abstract class Collectable : MonoBehaviour
    {
        public CollectableType type;
        public PooledObject pooledObject;
        private BoxCollider boxCollider;

        private void Awake()
        {
            boxCollider = GetComponent<BoxCollider>();
        }

        private void OnEnable()
        {
            StartCoroutine(Float());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player")) GetCollected();
        }

        private IEnumerator Float()
        {
            // Rotate around y-axis 360 degrees over 2 seconds and go up to y 2 and back to 1.5
            while (true)
            {
                var elapsedTime = 0f;
                var duration = 6f;
                var y = transform.position.y;
                while (elapsedTime < duration)
                {
                    elapsedTime += Time.deltaTime;
                    var t = elapsedTime / duration;
                    var rotation = transform.rotation.eulerAngles;
                    rotation.y += 360 * Time.deltaTime / duration;
                    transform.rotation = Quaternion.Euler(rotation);
                    transform.position = new Vector3(transform.position.x, Mathf.Lerp(y, y + 0.5f, t),
                        transform.position.z);
                    yield return null;
                }

                yield return null;
            }
        }

        public void SetCollectableProperty(CollectableProperty property)
        {
            type = property.type;
            boxCollider.size = property.colliderSize;

            SetSpecificProperty(property);
        }


        public abstract void SetSpecificProperty(CollectableProperty property);

        public void ReturnToPool()
        {
            pooledObject.ReturnToPool();
        }

        public abstract void GetCollected();
    }
}