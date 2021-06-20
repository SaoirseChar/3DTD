using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TowerDefense.Player;

namespace TowerDefense.Enemy
{
    public class EnemyAI : MonoBehaviour
    {
        private NavMeshAgent agent;
        private EnemyWaypoint waypoint;
        private EnemyMidPoint midpoint;
        private Animator AgentAnimator;

        [Header("Enemy Variables")]
        public int maxHealth = 5;
        public int health = 5;

        public int moneyOnDeath = 3;

        public bool amIDead = false;


        [SerializeField] private float runSpeed;
        // Start is called before the first frame update
        void Start()
        {
            agent = gameObject.GetComponent<NavMeshAgent>();
            // FindObjectsOfType gets every instance of this component in the scene
            waypoint = FindObjectOfType<EnemyWaypoint>();
            midpoint = FindObjectOfType<EnemyMidPoint>();
            AgentAnimator = GetComponentInChildren<Animator>();
            agent.SetDestination(midpoint.Position);
        }

        // Update is called once per frame
        void Update()
        {
            if (!agent.pathPending && Vector3.Distance(gameObject.transform.position, midpoint.transform.position) < 2f)
            {
                agent.SetDestination(waypoint.Position);
            }
            if (!agent.pathPending && Vector3.Distance(gameObject.transform.position, waypoint.transform.position) < 2f)
            {
                GameOver();
            }
            // If running, play running animation.
            AgentAnimator.SetBool("Running", agent.velocity.magnitude > runSpeed);
        }
        public void GameOver()
        {
            print("GAME OVER");
            PlayerManager.GameOver();
            Destroy(transform.parent.gameObject);
        }

        public void TakeDamage(int amount)
        {
            health -= amount;

            if (health <= 0 && !amIDead)
            {
                EndThisLife();
            }
        }

        private void EndThisLife()
        {
            amIDead = true;
            PlayerManager.currentMoney += moneyOnDeath;
            AgentAnimator.SetTrigger("Dying");
            //WaveSpawner.EnemiesAlive--;
            agent.isStopped = true;
            Destroy(transform.parent.gameObject, 2);
        }
    }
}