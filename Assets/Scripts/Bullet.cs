using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TowerDefense.Enemy;
namespace TowerDefense.Turret
{
    public class Bullet : MonoBehaviour
    {
        private Transform target;
        public float speed = 90;
        public GameObject impactEffect;

        private int impactDamage;

        public void Seek(Transform _target, int _ImpactDamage)
        {
            target = _target;
            impactDamage = _ImpactDamage;
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null)
            {
                Destroy(gameObject);
                return;
            }

            Vector3 dir = target.position - transform.position;
            float distanceThisFrame = speed * Time.deltaTime;

            if (dir.magnitude <= distanceThisFrame)
            {
                HitTarget();
                return;
            }
            transform.Translate(dir.normalized * distanceThisFrame, Space.World);
            transform.LookAt(target);
        }

        void HitTarget()
        {
            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);

            Destroy(effectIns, 2f);

            EnemyTakesDamage(target);

            Destroy(gameObject);
        }

        void EnemyTakesDamage(Transform enemy)
        {
            EnemyAI thisEnemy = enemy.GetComponent<EnemyAI>();

            if (thisEnemy != null)
            {
                thisEnemy.TakeDamage(impactDamage);
            }
        }

    }
}