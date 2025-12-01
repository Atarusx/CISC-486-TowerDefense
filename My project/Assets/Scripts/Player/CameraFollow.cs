using Unity.Netcode;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0,0,-10);
    public float smoothSpeed = 0.125f;

/*
    void Start()
    {
        NetworkObject[] players = FindObjectsOfType<NetworkObject>();
        foreach(NetworkObject player in players)
        {
            if (player.IsOwner && player.CompareTag("Player"))
            {
                target = player.transform;
                break;
            }
        }

    }
*/



public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        Debug.Log("Camera now following:" + newTarget.name);
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

/*
    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
*/
}
