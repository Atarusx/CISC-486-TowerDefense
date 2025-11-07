using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

public class ProjectileBeh : ProjectileWepBeh

{


    [SerializeField] private float bulletSpeed = 5f;


    [SerializeField] private Rigidbody2D rb;

    TurretController tc;

    private Transform target;


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;

        rb.linearVelocity = direction * bulletSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);

    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }


}
