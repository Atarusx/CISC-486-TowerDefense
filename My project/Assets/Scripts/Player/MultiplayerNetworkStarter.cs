using Unity.Netcode;
using UnityEngine;

public class MultiplayerNetworkStarter : MonoBehaviour
{
    void OnGUI()
    {
        float a = 200f, b = 40f;
        float x = 10f, y = 10f;

        if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
        {
            if (GUI.Button(new Rect(x, y, a, b), "Host"))
            {
                NetworkManager.Singleton.StartHost();
                Debug.Log("Started as Host");
            }
            
            if (GUI.Button(new Rect(x, y + b + 10, a, b), "Client"))
            {
                NetworkManager.Singleton.StartClient();
                Debug.Log("Started as Client");
            }
            
            if (GUI.Button(new Rect(x, y + 2 * (b + 10), a, b), "Server"))
            {
                NetworkManager.Singleton.StartServer();
                Debug.Log("Started as Server");
            }
        }
        else
        {
            string status = "";
            if (NetworkManager.Singleton.IsHost) status = "HOST";
            else if (NetworkManager.Singleton.IsServer) status = "SERVER";
            else if (NetworkManager.Singleton.IsClient) status = "CLIENT";
            
            GUI.Label(new Rect(x, y, a, b), $"Status: {status}");
            GUI.Label(new Rect(x, y + 30, a, b), $"Client ID: {NetworkManager.Singleton.LocalClientId}");
            GUI.Label(new Rect(x, y + 60, a, b), $"Connected Clients: {NetworkManager.Singleton.ConnectedClients.Count}");
            
            if (GUI.Button(new Rect(x, y + 100, a, b), "Disconnect"))
            {
                NetworkManager.Singleton.Shutdown();
                Debug.Log("Disconnected");
            }
        }
    }
}