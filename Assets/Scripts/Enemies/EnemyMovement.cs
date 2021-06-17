using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Enemy
{
    public class EnemyMovement : MonoBehaviour
    {
        public float speed = 10f;

        private Transform target;
        private int wavepointIndex = 0;
        private void Start()
        {
            target = Waypoints.points[0];
        }

        private void Update()
        {
            Vector3 dir = target.position - transform.position;
            // normalized will make the vector normally

            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);

            if (Vector3.Distance(transform.position, target.position) <= 0.4)
            {
                GetNextWaypoint();
            }
        }

        void GetNextWaypoint()
        {
            if (wavepointIndex >= Waypoints.points.Length - 1)
            {
                Destroy(gameObject);
                return;
            }

            wavepointIndex++;
            target = Waypoints.points[wavepointIndex];
        }
    }
}