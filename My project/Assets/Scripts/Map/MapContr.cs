using System.Collections.Generic;
using UnityEngine;

public class MapContr : MonoBehaviour
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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pm = FindFirstObjectByType<NewMonoBehaviourScript>();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkCheck();
        chunkOpt();
    }

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

}
