using System.Collections;
using UnityEngine;
using TowerDefense.Enemy;

namespace TowerDefense.Turret
{
    public class Turret : MonoBehaviour
    {

        [Header("Attributes")]
        public float range = 15f;
        public float fireRate = 1f;
        private float fireCountdown = 0f;

        [Header("Unity Setup Fields")]
        public string enemyTag = "Enemy";

        private Transform target;
        public float turnSpeed = 10f;

        public Transform partToRotate;

        public GameObject bulletPrefab;
        public Transform firePoint;

        public int turrentDamage = 1;
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

            if (fireCountdown <= 0f)
            {
                Shoot();
                fireCountdown = 1f / fireRate;
            }

            fireCountdown -= Time.deltaTime;
        }
        private void Shoot()
        {
            GameObject tempBullet = (GameObject)Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Bullet bullet = tempBullet.GetComponent<Bullet>();

            if (bullet != null)
            {
                bullet.Seek(target, turrentDamage);
            }
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
            if ((nearestEnemy != null || !nearestEnemy.GetComponent<EnemyAI>().amIDead) && shortestDistance <= range)
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