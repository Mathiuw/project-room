using System.Collections.Generic;
using UnityEngine;

namespace MaiNull
{
    public class WaveEnemySpawner : MonoBehaviour
    {
        [SerializeField] private List<Enemy> enemies = new List<Enemy>();
        public int CurrentWave { get; private set; } = 1;
        private int waveValue = 0;
        public List<GameObject> enemiesToSpawn = new List<GameObject>();

        public Transform[] spawnLocation;
        public int spawnIndex;

        [field: SerializeField] public int WaveDuration { get; private set; } = 30;
        [SerializeField] private int maxEnemyCountPerWave = 50;
        private float waveTimer;
        private float spawnInterval;
        private float spawnTimer;

        public List<GameObject> spawnedEnemies = new List<GameObject>();


        void Start()
        {
            GenerateWave();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if (spawnTimer <= 0)
            {
                //spawn an enemy
                if (enemiesToSpawn.Count > 0)
                {
                    GameObject enemy = Instantiate(enemiesToSpawn[0], spawnLocation[spawnIndex].position, Quaternion.identity); // spawn first enemy in our list
                    enemiesToSpawn.RemoveAt(0); // and remove 
                    spawnedEnemies.Add(enemy);

                    // Add wave enemy tracker to spawned enemy
                    enemy.AddComponent<EnemyWaveTracker>();

                    spawnTimer = spawnInterval;

                    if (spawnIndex + 1 <= spawnLocation.Length - 1)
                    {
                        spawnIndex++;
                    }
                    else
                    {
                        spawnIndex = 0;
                    }
                }
                else
                {
                    waveTimer = 0; // if no enemies remain, end wave
                }
            }
            else
            {
                spawnTimer -= Time.fixedDeltaTime;
                waveTimer -= Time.fixedDeltaTime;
            }

            if (waveTimer <= 0 && spawnedEnemies.Count <= 0)
            {
                CurrentWave++;
                GenerateWave();
            }
        }

        public void GenerateWave()
        {
            waveValue = CurrentWave * 10;
            GenerateEnemies();

            spawnInterval = WaveDuration / enemiesToSpawn.Count; // gives a fixed time between each enemies
            waveTimer = WaveDuration; // wave duration is read only
        }

        public void GenerateEnemies()
        {
            // Create a temporary list of enemies to generate
            // 
            // in a loop grab a random enemy 
            // see if we can afford it
            // if we can, add it to our list, and deduct the cost.

            // repeat... 

            //  -> if we have no points left, leave the loop

            List<GameObject> generatedEnemies = new List<GameObject>();

            while (waveValue > 0 || generatedEnemies.Count < maxEnemyCountPerWave)
            {
                int randEnemyId = Random.Range(0, enemies.Count);
                int randEnemyCost = enemies[randEnemyId].cost;

                if (waveValue - randEnemyCost >= 0)
                {
                    generatedEnemies.Add(enemies[randEnemyId].enemyPrefab);
                    waveValue -= randEnemyCost;
                }
                else if (waveValue <= 0)
                {
                    break;
                }
            }
            enemiesToSpawn.Clear();
            enemiesToSpawn = generatedEnemies;
        }


        [System.Serializable]
        private class Enemy
        {
            public GameObject enemyPrefab;
            public int cost;
        }
    }


}