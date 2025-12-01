using Unity.Netcode;
using UnityEngine;

public class ChunkTrigger : NetworkBehaviour
{

    MapContr mc;
    public GameObject targetMap;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mc = FindFirstObjectByType<MapContr>();
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsServer) return;

        if (collision.CompareTag("Player"))
        {
            mc.currentChunk = targetMap;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!IsServer) return;
        
        if (collision.CompareTag("Player"))
        {
            if (mc.currentChunk == targetMap)
            {
                mc.currentChunk = null;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
