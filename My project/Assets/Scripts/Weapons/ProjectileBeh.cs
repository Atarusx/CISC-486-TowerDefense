using Unity.Netcode;
using UnityEngine;

public class ProjectileBeh : NetworkBehaviour
{
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float damage = 10f; 

    private Transform target;
    private NetworkVariable<Vector2> targetPosition = new NetworkVariable<Vector2>();

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Destroy(gameObject, 10f);
    }

    void FixedUpdate()
    {
        if (!target)
        {
            return;
        }

        if (IsServer)
        {
            Vector2 direction = (target.position - transform.position).normalized;
            rb.linearVelocity = direction * bulletSpeed;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (!IsServer) return;

        EnemyHealth enemyHealth = collision.gameObject.GetComponent<EnemyHealth>();
        
        if (enemyHealth != null)
        {
            Debug.Log($"Projectile hit enemy! Dealing {damage} damage.");
            enemyHealth.TakeDamageServerRpc(damage);
        }


        if (NetworkObject != null && NetworkObject.IsSpawned)
        {
            NetworkObject.Despawn();
        }
        Destroy(gameObject);
    }
}