using UnityEngine;

public class ProjectileWepBeh : MonoBehaviour
{

    protected Vector3 direction;
    public float destroyAfterSeconds;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {
        Destroy(gameObject, destroyAfterSeconds);
    }

    // Update is called once per frame

    public void DirectionCheck(Vector3 dir)
    {
        direction = dir;
    }

}
