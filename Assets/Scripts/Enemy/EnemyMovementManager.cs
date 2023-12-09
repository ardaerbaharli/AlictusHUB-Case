using System.Collections;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    public class EnemyMovementManager : MonoBehaviour
    {
        private EnemyAttackManager enemyAttackManager;
        private NavMeshAgent navMeshAgent;
        private PlayerController player;
        private float range;
        private float speed;

        private void Awake()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            enemyAttackManager = GetComponent<EnemyAttackManager>();
        }


        public void StartFollowing(PlayerController player, float speed)
        {
            this.player = player;
            this.speed = speed;
            range = enemyAttackManager.weapon.Range;
            StartCoroutine(FollowPlayer());
        }

        private IEnumerator FollowPlayer()
        {
            navMeshAgent.speed = speed;
            var targetPosition = player.transform.position;
            navMeshAgent.stoppingDistance = range;
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(targetPosition);
            yield return new WaitForSeconds(0.2f);

            var lastPosition = transform.position;
            var stuckCheckInterval = 1f;
            while (navMeshAgent.remainingDistance - range > 0f)
            {
                stuckCheckInterval -= Time.deltaTime;
                if (stuckCheckInterval < 0f)
                {
                    if (Vector3.Distance(transform.position, lastPosition) < 0.5f)
                    {
                        targetPosition = player.transform.position;
                        navMeshAgent.isStopped = false;
                        navMeshAgent.SetDestination(targetPosition);
                        yield return new WaitForSeconds(0.3f);
                    }

                    lastPosition = transform.position;
                    stuckCheckInterval = 1f;
                }

                yield return null;
            }

            var distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance > enemyAttackManager.weapon.Range) StartCoroutine(FollowPlayer());
        }


        public void StopFollowing()
        {
            navMeshAgent.isStopped = true;
        }
    }
}