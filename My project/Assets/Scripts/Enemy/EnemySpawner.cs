using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EnemySpawner : NetworkBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public float spawnInterval;
        public int spawnCount;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int currentWaveCount;

    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesAlive;
    public int maxEnemiesAllowed;
    public bool maxEnemiesReached = false;
    public float waveInterval;

    [Header("Spawn Location")]
    public Transform spawnCenter; 
    public float spawnRadius = 10f; 

    void Start()
    {

        if (spawnCenter == null)
        {
            GameObject homeTile = GameObject.Find("Home Tile");
            if (homeTile != null)
            {
                spawnCenter = homeTile.transform;
            }
            else
            {
                Debug.LogWarning("No spawn center assigned! Using spawner position.");
                spawnCenter = transform;
            }
        }

        if (IsServer)
        {
            CalculateWaveQuota();
        }
    }

    void Update()
    {

        if (!IsServer) return;

        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount >= waves[currentWaveCount].waveQuota)
        {

            if (enemiesAlive == 0)
            {
                StartCoroutine(BeginNextWave());
            }
        }


        if (currentWaveCount < waves.Count && waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            spawnTimer += Time.deltaTime;
            if (spawnTimer >= waves[currentWaveCount].spawnInterval)
            {
                spawnTimer = 0f;
                SpawnEnemies();
            }
        }
    }

    IEnumerator BeginNextWave()
    {
        Debug.Log($"Wave {currentWaveCount} complete! Starting next wave in {waveInterval} seconds...");
        yield return new WaitForSeconds(waveInterval);
        
        if (currentWaveCount < waves.Count - 1)
        {
            currentWaveCount++;
            CalculateWaveQuota();
            Debug.Log($"Starting Wave {currentWaveCount}: {waves[currentWaveCount].waveName}");
        }
        else
        {
            Debug.Log("All waves complete!");
        }
    }

    void CalculateWaveQuota()
    {
        int currentWaveQuota = 0;
        foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
        {
            currentWaveQuota += enemyGroup.enemyCount;
            enemyGroup.spawnCount = 0; 
        }

        waves[currentWaveCount].waveQuota = currentWaveQuota;
        waves[currentWaveCount].spawnCount = 0; 
        
        Debug.Log($"Wave {currentWaveCount} quota: {currentWaveQuota} enemies");
    }

    void SpawnEnemies()
    {

        if (enemiesAlive >= maxEnemiesAllowed)
        {
            maxEnemiesReached = true;
            return;
        }

        if (waves[currentWaveCount].spawnCount < waves[currentWaveCount].waveQuota)
        {
            foreach (var enemyGroup in waves[currentWaveCount].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {

                    Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
                    Vector2 spawnPosition = (Vector2)spawnCenter.position + randomOffset;


                    NetworkObject prefabNetObj = enemyGroup.enemyPrefab.GetComponent<NetworkObject>();
                    if (prefabNetObj == null)
                    {
                        Debug.LogError($"Enemy prefab '{enemyGroup.enemyPrefab.name}' is missing NetworkObject component!");
                        continue;
                    }


                    GameObject enemyObj = Instantiate(enemyGroup.enemyPrefab, spawnPosition, Quaternion.identity);
                    NetworkObject netObj = enemyObj.GetComponent<NetworkObject>();
                    
                    if (netObj != null)
                    {
                        netObj.Spawn(true); 
                        

                        EnemyHealth enemyHealth = enemyObj.GetComponent<EnemyHealth>();
                        if (enemyHealth != null)
                        {

                            enemyHealth.OnDeath += OnEnemyKilled;
                        }
                    }

                    enemyGroup.spawnCount++;
                    waves[currentWaveCount].spawnCount++;
                    enemiesAlive++;

                    Debug.Log($"Spawned {enemyGroup.enemyName} ({enemyGroup.spawnCount}/{enemyGroup.enemyCount}) - Total alive: {enemiesAlive}");
                    
                    return; 
                }
            }
        }

        maxEnemiesReached = false;
    }

    public void OnEnemyKilled()
    {
        if (!IsServer) return;
        
        enemiesAlive--;
        Debug.Log($"Enemy killed! Remaining: {enemiesAlive}");
        
        if (enemiesAlive < maxEnemiesAllowed)
        {
            maxEnemiesReached = false;
        }
    }


}