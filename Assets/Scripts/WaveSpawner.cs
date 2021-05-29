using UnityEngine;
using System.Collections;
using TMPro;
using Sirenix.OdinInspector;

namespace TowerDefense.Enemy
{
    public class WaveSpawner : MonoBehaviour
    {
        public Transform enemyPrefab;

        [Header("Spawn Variables")]
        public float timeBetweenWaves = 5f;
        public float timeBetweenEnemies = 0.5f;
        [LabelText("Spawn Location")]public Transform spawnPoint;

        [Header("Spawn TextBoxes")]
        public TMP_Text waveCountdownText;
        public TMP_Text waveNumberText;


        private int waveNumber = 0;
        private bool enemiesSpawning;
        private float countdown = 2f;


        private void Start()
        {
            enemiesSpawning = false;
        }
        private void Update()
        {
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
                waveNumberText.text = ("Round: " + Mathf.Round(waveNumber).ToString());
            }
        }

        IEnumerator SpawnWave()
        {
            enemiesSpawning = true;
            waveCountdownText.gameObject.SetActive(false);
            waveNumber++;
            for (int i = 0; i < waveNumber; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(timeBetweenEnemies);
            }
            yield return new WaitForSeconds(5);
            enemiesSpawning = false;
            waveCountdownText.gameObject.SetActive(true);
        }

        void SpawnEnemy()
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}