using Unity.Netcode;
using UnityEngine;

public class TurretController : WeaponController
{
    [Header("Turret Specific")]
    public float detectionRange = 10f;
    private Transform nearestEnemy;
    [SerializeField] private LayerMask enemyMask;
    [SerializeField] private Transform turretRotationPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firingPoint;
    [SerializeField] private float bps = 1f;

    private float timeUntilFire;
    

    private NetworkVariable<float> networkRotation = new NetworkVariable<float>(
        0f, 
        NetworkVariableReadPermission.Everyone, 
        NetworkVariableWritePermission.Server
    );

    protected override void Start()
    {
        base.Start();
        

        networkRotation.OnValueChanged += OnRotationChanged;
    }
    
    private void OnDestroy()
    {
        networkRotation.OnValueChanged -= OnRotationChanged;
    }
    
    private void OnRotationChanged(float oldValue, float newValue)
    {

        if (!IsServer && turretRotationPoint != null)
        {
            turretRotationPoint.rotation = Quaternion.Euler(0f, 0f, newValue);
        }
    }

    protected override void Update()
    {
        if (!IsServer)
        {
            return;
        }

        if (nearestEnemy == null)
        {
            FindNearestEnemy();
            return;
        }

        RotateTurretTowards();

        if (!CheckTargetIsInRange())
        {
            nearestEnemy = null;
            return;
        }
        else
        {
            timeUntilFire += Time.deltaTime;

            if (timeUntilFire >= 1f / bps)
            {
                Attack();
                timeUntilFire = 0f;
            }
        }
    }

    protected override void Attack()
    {

        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity);
        

        NetworkObject netObj = bulletObj.GetComponent<NetworkObject>();
        if (netObj != null)
        {
            netObj.Spawn();
        }
        
        ProjectileBeh bulletScript = bulletObj.GetComponent<ProjectileBeh>();
        if (bulletScript != null)
        {
            bulletScript.SetTarget(nearestEnemy);
            bulletScript.SetDamage(damage); 
        }

        Debug.Log($"Turret fired at {nearestEnemy.name}");
    }

    private void FindNearestEnemy()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(
            transform.position, 
            detectionRange, 
            (Vector2)transform.position, 
            0f, 
            enemyMask
        );

        if (hits.Length > 0)
        {
            nearestEnemy = hits[0].transform;
        }
    }

    private void RotateTurretTowards()
    {
        float angle = Mathf.Atan2(
            nearestEnemy.position.y - transform.position.y, 
            nearestEnemy.position.x - transform.position.x
        ) * Mathf.Rad2Deg + 90f;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = targetRotation;
        

        if (IsServer)
        {
            networkRotation.Value = angle;
        }
    }

    private bool CheckTargetIsInRange()
    {
        return Vector2.Distance(nearestEnemy.position, transform.position) <= detectionRange;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}