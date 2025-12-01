using Unity.Netcode;
using UnityEngine;

public class NetworkManagerSetup : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform[] spawnPoints; 

    private void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    private void OnServerStarted()
    {
        Debug.Log("Server started!");
    }

    private void OnClientConnected(ulong clientId)
    {
        Debug.Log($"Client {clientId} connected!");
        
        Vector3 spawnPosition = Vector3.zero;
        
        if (spawnPoints != null && spawnPoints.Length > 0)
        {
            int spawnIndex = (int)clientId % spawnPoints.Length;
            spawnPosition = spawnPoints[spawnIndex].position;
        }
        else
        {
            spawnPosition = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
        }

        GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        NetworkObject netObj = playerInstance.GetComponent<NetworkObject>();
        
        if (netObj != null)
        {
            netObj.SpawnAsPlayerObject(clientId, true);
            Debug.Log($"Spawned player for client {clientId} at {spawnPosition}");
        }
        else
        {
            Debug.LogError("Player prefab is missing NetworkObject component!");
        }
    }

    private void OnDestroy()
    {
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnServerStarted -= OnServerStarted;
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }
}