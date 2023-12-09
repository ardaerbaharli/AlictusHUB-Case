using System.Collections;
using Player;
using UnityEngine;

namespace Weapons.Ammos
{
    public class BoomerangAmmo : Ammo
    {
        public float speed = 50f;
        public float rotationSpeed = 300f;
        public float throwTime = 5f;

        public override void Shoot(Vector3 direction)
        {
            var player = PlayerController.Instance.gameObject.transform;
            StartCoroutine(ShootCoroutine(direction, player));
        }

        private IEnumerator ShootCoroutine(Vector3 direction, Transform player)
        {
            var time = 0f;
            while (time < throwTime)
            {
                time += Time.deltaTime;
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                transform.position += direction * (speed * Time.deltaTime);
                yield return null;
            }

            // Return to initial position with rotation and speed
            while (Vector3.Distance(transform.position, player.transform.position) > 0.1f)
            {
                transform.position =
                    Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
                yield return null;
            }

            ReturnToPool();
        }
    }
}