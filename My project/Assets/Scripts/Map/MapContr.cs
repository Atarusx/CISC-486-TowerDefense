using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MapContr : NetworkBehaviour
{



    public List<GameObject> terrainChunks;
    public GameObject player;
    public float radiusCheck;
    Vector3 noTerrainPos;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    NewMonoBehaviourScript pm;

    [Header("Opt")]
    public List<GameObject> spawnedChunks;
    GameObject latestChunk;
    public float maxOpDist;
    float opDist;
    float opCooldown;
    public float optCooldownDur;


    [Header("Chunk Limiter")]
    public int maxChunksFromCenter = 3;
    private Vector2 centerChunkPos;
    private float chunkSize;
    private bool centerSet = false;

    private HashSet<Vector3> spawnedPositions = new HashSet<Vector3>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //pm = FindFirstObjectByType<NewMonoBehaviourScript>();
    }

    // Update is called once per frame
    void Update()
    {
        //ChunkCheck();
        //chunkOpt();
        if (IsServer)
        {
            CheckAllPlayersForChunks();
            chunkOpt();
        }

    }

    void CheckAllPlayersForChunks()
    {
        NewMonoBehaviourScript[] players = FindObjectsOfType<NewMonoBehaviourScript>();
            
        foreach (NewMonoBehaviourScript pm in players)
        {
            if (pm == null) continue;
                
            GameObject playerCurrentChunk = GetPlayerCurrentChunk(pm.transform.position);
            if (playerCurrentChunk == null) continue;

            ChunkCheck(pm, playerCurrentChunk);
        }
    }

    GameObject GetPlayerCurrentChunk(Vector3 playerPos)
    {
        foreach (GameObject chunk in spawnedChunks)
        {
            if (chunk == null) continue;
            
            if (Vector3.Distance(playerPos, chunk.transform.position) < 20f)
            {
                return chunk;
            }
        }
        return currentChunk; 
    }

    void ChunkCheck(NewMonoBehaviourScript pm, GameObject checkChunk)
    {
        if (!checkChunk) return;

        Vector2 moveDir = pm.moveDir;
        
        if (moveDir == Vector2.zero)
        {
            Rigidbody2D rb = pm.GetComponent<Rigidbody2D>();
            if (rb != null && rb.linearVelocity.magnitude > 0.1f)
            {
                moveDir = rb.linearVelocity.normalized;
            }
        }

        Transform spawnPoint = null;

        if (moveDir.x > 0 && moveDir.y == 0) // right
        {
            spawnPoint = checkChunk.transform.Find("right");
        }
        else if (moveDir.x < 0 && moveDir.y == 0) // left
        {
            spawnPoint = checkChunk.transform.Find("left");
        }
        else if (moveDir.x == 0 && moveDir.y > 0) // up
        {
            spawnPoint = checkChunk.transform.Find("up");
        }
        else if (moveDir.x == 0 && moveDir.y < 0) // down
        {
            spawnPoint = checkChunk.transform.Find("down");
        }
        else if (moveDir.x > 0 && moveDir.y > 0) // right up
        {
            spawnPoint = checkChunk.transform.Find("right up");
        }
        else if (moveDir.x > 0 && moveDir.y < 0) // right down
        {
            spawnPoint = checkChunk.transform.Find("right down");
        }
        else if (moveDir.x < 0 && moveDir.y > 0) // left up
        {
            spawnPoint = checkChunk.transform.Find("left up");
        }
        else if (moveDir.x < 0 && moveDir.y < 0) // left down
        {
            spawnPoint = checkChunk.transform.Find("left down");
        }

        if (spawnPoint != null)
        {
            Vector3 spawnPos = spawnPoint.position;
            
            if (!spawnedPositions.Contains(spawnPos) && 
                !Physics2D.OverlapCircle(spawnPos, radiusCheck, terrainMask))
            {
                noTerrainPos = spawnPos;
                SpawnChunk();
            }
        }
    }

    void SpawnChunk()
    {
        
        int seed = GetSeedFromPosition(noTerrainPos);
        Random.InitState(seed);
        
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPos, Quaternion.identity);
        
        
        NetworkObject netObj = latestChunk.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.Spawn();
        }
        
        spawnedChunks.Add(latestChunk);
        spawnedPositions.Add(noTerrainPos);
        
        Debug.Log($"Spawned chunk at {noTerrainPos} with seed {seed}, chunk index {rand}");
    }

    int GetSeedFromPosition(Vector3 pos)
    {
        
        int x = Mathf.RoundToInt(pos.x * 100);
        int y = Mathf.RoundToInt(pos.y * 100);
        return x * 73856093 ^ y * 19349663;
    }

    void chunkOpt()
    {
        opCooldown -= Time.deltaTime;

        if (opCooldown <= 0f)
        {
            opCooldown = optCooldownDur;
        }
        else
        {
            return;
        }

        NewMonoBehaviourScript[] players = FindObjectsOfType<NewMonoBehaviourScript>();
        Vector3 closestPlayerPos = Vector3.zero;
        float closestDist = Mathf.Infinity;

        foreach (NewMonoBehaviourScript p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < closestDist)
            {
                closestDist = dist;
                closestPlayerPos = p.transform.position;
            }
        }

        foreach (GameObject chunk in spawnedChunks)
        {
            if (chunk == null) continue;
            
            opDist = Vector3.Distance(closestPlayerPos, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }

/*
    void ChunkCheck()
    {

        if (!currentChunk)
        {
            return;
        }


        if (pm.moveDir.x > 0 && pm.moveDir.y == 0) //movement to the right
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("right").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("right").position;
                SpawnChunk();
            }
        }
        else if (pm.moveDir.x < 0 && pm.moveDir.y == 0) //movement to the left
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("left").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("left").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y > 0) //movement upwards
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("up").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("up").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x == 0 && pm.moveDir.y < 0) //movement downwards
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("down").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("down").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x > 0 && pm.moveDir.y > 0) //movement right upwards
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("right up").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("right up").position;
                SpawnChunk();
            }
        }


        else if (pm.moveDir.x > 0 && pm.moveDir.y < 0) //movement right downwards
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("right down").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("right down").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x < 0 && pm.moveDir.y > 0) //movement left upwards
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("left up").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("left up").position;
                SpawnChunk();
            }
        }

        else if (pm.moveDir.x < 0 && pm.moveDir.y < 0) //movement left downwards
        {
            if (!Physics2D.OverlapCircle(currentChunk.transform.Find("left down").position, radiusCheck, terrainMask))
            {
                noTerrainPos = currentChunk.transform.Find("left down").position;
                SpawnChunk();
            }
        }
    }


    // GOING TO INTEGRATE CHUNK LIMITER TO 3x3 to RETAIN TOWER DEFENSE ELEMENTS AND NOT CREATE A ROGUELIKE 
    bool IsWithinChunkLimt(Vector3 spawnPos)
    {
        float distanceX = Mathf.Abs(spawnPos.x - centerChunkPos.x);
        float distanceY = Mathf.Abs(spawnPos.y - centerChunkPos.y);

        int chunksAwayX = Mathf.RoundToInt(distanceX / chunkSize);
        int chunksAwayY = Mathf.RoundToInt(distanceY / chunkSize);

        return chunksAwayX <= maxChunksFromCenter && chunksAwayY <= maxChunksFromCenter;
    }



    void SpawnChunk()
    {
        int rand = Random.Range(0, terrainChunks.Count);
        latestChunk = Instantiate(terrainChunks[rand], noTerrainPos, Quaternion.identity);
        spawnedChunks.Add(latestChunk);
    }


    void chunkOpt()
    {
        opCooldown -= Time.deltaTime;

        if (opCooldown <= 0f)
        {
            opCooldown = optCooldownDur;
        }
        else
        {
            return;
        }


        foreach (GameObject chunk in spawnedChunks)
        {
            opDist = Vector3.Distance(player.transform.position, chunk.transform.position);
            if (opDist > maxOpDist)
            {
                chunk.SetActive(false);
            }
            else
            {
                chunk.SetActive(true);
            }
        }
    }
*/
}
