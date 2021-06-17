using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Turret
{
    public class Bullet : MonoBehaviour
    {
        private Transform target;
        public float speed = 90;
        public GameObject impactEffect;

        public void Seek(Transform _target)
        {
            target = _target;
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
        }

        void HitTarget()
        {


            GameObject effectIns = Instantiate(impactEffect, transform.position, transform.rotation);

            Destroy(effectIns, 2f);

            Destroy(target.gameObject);

            Destroy(gameObject);
        }
    }
}