using System.Collections;
using UnityEngine;

namespace TowerDefense.Turret
{
    public class Turret : MonoBehaviour
    {
        private Transform target;
        public float range = 15f;
        public string enemyTag = "Enemy";
        public float turnSpeed = 10f;
        public Transform partToRotate;

        [SerializeField]
        private float howOftenTurretLocksOn = 0.5f;

        // Start is called just before any of the Update methods is called the first time
        private void Start()
        {
            // Runs every howOftenTurretLocksOn seconds from 0 seconds.
            InvokeRepeating("UpdateTarget", 0f, howOftenTurretLocksOn);
        }

        // Update is called every frame, if the MonoBehaviour is enabled
        private void Update()
        {
            if (target == null)
            {
                return;
            }
            targetLockOn();
        }

        // This will be used to lock on to the closest target.
        void targetLockOn()
        {
            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            // Lerp will make this all happen slowly.
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * turnSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }

        // This is used every howOftenTurretLocksOn per second to lock on to an enemy.
        void UpdateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = enemy;
                }
            }
            if (nearestEnemy != null && shortestDistance <= range)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
        }

        // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}