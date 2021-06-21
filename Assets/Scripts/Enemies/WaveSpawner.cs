using UnityEngine;
using System.Collections;
using TMPro;
using Sirenix.OdinInspector;

using TowerDefense.Player;

namespace TowerDefense.Enemy
{
    public class WaveSpawner : MonoBehaviour
    {
        public static int EnemiesAlive = 0;

        public Transform enemyPrefab;

        [Header("Spawn Variables")]
        public float timeBetweenWaves = 5f;
        public float timeBetweenEnemies = 0.5f;
        [LabelText("Spawn Location")] public Transform spawnPoint;

        [Header("Spawn TextBoxes")]
        public TMP_Text waveCountdownText;


        private int waveNumber = 0;
        private bool enemiesSpawning;
        private float countdown = 2f;


        public PlayerManager playerManager;

        [System.Serializable]
        public class WaveComponent
        {
            public GameObject enemy;
            public int count;
            public float rate;
        }

        public WaveComponent[] waves;
        private int waveIndex = 0;

        private void Start()
        {
            waveNumber = 0;
            enemiesSpawning = false;
            print(waves.Length);
        }
        private void Update()
        {
            if (EnemiesAlive > 0)
            {
                return;
            }

            if (waveIndex >= 14)
            {
                playerManager.WinGame();
                this.enabled = false;
            }
            // Spawn wave on count down.
            if (countdown <= 0)
            {
                StartCoroutine(SpawnWave());
                countdown = timeBetweenWaves;
            }

            if (!enemiesSpawning)
            {
                countdown -= Time.deltaTime;
                // Display the waves and when they next Spawn
                waveCountdownText.text = ("Next Wave In: " + Mathf.Round(countdown).ToString());
            }
        }

        IEnumerator SpawnWave()
        {
            enemiesSpawning = true;
            waveCountdownText.gameObject.SetActive(false);
            playerManager.IncreaseRound();

            WaveComponent wave = waves[waveNumber];

            for (int i = 0; i < wave.count; i++)
            {
                print("Enemy Spawned");
                SpawnEnemy(wave.enemy);
                yield return new WaitForSeconds(wave.rate);
            }
            yield return new WaitForSeconds(5);
            enemiesSpawning = false;
            waveCountdownText.gameObject.SetActive(true);
            waveNumber++;
        }

        void SpawnEnemy(GameObject _enemy)
        {
            Instantiate(_enemy, spawnPoint.position, spawnPoint.rotation);
        }
    }
}