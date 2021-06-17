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

        [Header("Enemy Variables")]
        public int Health = 5;

        [SerializeField] private float runSpeed;
        // Start is called before the first frame update
        void Start()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
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
                GameOver();
            }

            // If running, play running animation.
            AgentAnimator.SetBool("Running", agent.velocity.magnitude > runSpeed);
        }
    public void GameOver()
    { 
                print("GAME OVER");
            Destroy(transform.parent.gameObject);
    }
    }

}