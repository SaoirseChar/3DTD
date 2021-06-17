using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Enemy
{
    public class EnemyWaypoint : MonoBehaviour
    {
        // Lambda - Inline functions - Functions without actual definitions 
        // like when we use private void - Just points to something // LAMBDAS ROCK
        // In this case points to transform.position
        public Vector3 Position => transform.position;

        // Simply a way to find any object marked as a waypoint

        // Implement this OnDrawGizmos if you want to draw gizmos that are also pickable and always drawn
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, 0.1f);
        }
    }
}