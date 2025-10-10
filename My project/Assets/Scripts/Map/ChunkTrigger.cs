using UnityEngine;

public class ChunkTrigger : MonoBehaviour
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
        if (collision.CompareTag("Player"))
        {
            mc.currentChunk = targetMap;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
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
