using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace TowerDefense.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        private NavMeshAgent agent;
        private EnemyWaypoint waypoint;
        private Animator AgentAnimator;
        [SerializeField] private float runSpeed;
        private EnemyMovement thisEnemyMovement;
        // Start is called before the first frame update
        void Start()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            thisEnemyMovement = gameObject.GetComponent<EnemyMovement>();
            // FindObjectsOfType gets every instance of this component in the scene
            waypoint = FindObjectOfType<EnemyWaypoint>();
            AgentAnimator = GetComponent<Animator>();
            agent.SetDestination(waypoint.Position);
        }

        // Update is called once per frame
        void Update()
        {
            if (!agent.pathPending && agent.remainingDistance < 0.3f)
            {
                print("GAME OVER");
            }

            // If running, play running animation.
            AgentAnimator.SetBool("Running", agent.velocity.magnitude > runSpeed);
        }
    }
}