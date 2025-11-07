using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    [Header("Targets")]
    public Transform homeTile;
    public Transform portal;


    Transform player;
    [Header("Movement")]
    public float moveSpeed;
    public float runSpeed;


    [Header("Detection Ranges")]
    public float detectionRange = 10f;
    public float runRange = 3f;

    private Animator animator;
    private float currentSpeed;

    private enum TargetType { HomeTile, Player, Tower, Portal }
    private TargetType currentTarget;
    private Transform lockedTarget;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<NewMonoBehaviourScript>().transform;
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
        currentTarget = TargetType.HomeTile;
    }

    // Update is called once per frame
    void Update()
    {
        

        Debug.Log("Current Target: " + currentTarget + " | Speed: " + currentSpeed);

        Transform targetToChase = DetermineTarget();

        if (targetToChase != null)
        {
            float distanceToTarget = Vector2.Distance(transform.position, targetToChase.position);

            if (distanceToTarget <= runRange)
            {
                animator.SetBool("Move", true);
                animator.SetBool("Run", true);
                currentSpeed = runSpeed;
                
            }
            else if (distanceToTarget <= detectionRange)
            {
                animator.SetBool("Move", true);
                animator.SetBool("Run", false);
                currentSpeed = moveSpeed;
                
            }
            else
            {
                animator.SetBool("Move", false);
                animator.SetBool("Run", false);
            }


            
            transform.position = Vector2.MoveTowards(transform.position, targetToChase.position, currentSpeed * Time.deltaTime);
            
            


        }

    }
    Transform DetermineTarget()
    {

        Transform closestTower = FindClosestTower();
        float distToPortal = Vector2.Distance(transform.position, portal.position);
        float distToHome = homeTile != null ? Vector2.Distance(transform.position, homeTile.position) : Mathf.Infinity;
        float distToPlayer = player != null ? Vector2.Distance(transform.position, player.position) : Mathf.Infinity;
        float distToTower = closestTower != null ? Vector2.Distance(transform.position, closestTower.position) : Mathf.Infinity;


        if (lockedTarget != null)
        {
            if (lockedTarget.gameObject == null)
            {
                lockedTarget = null;
                currentTarget = TargetType.HomeTile;
            }
            else
            {
                if (player != null & distToPlayer <= detectionRange && distToPlayer < Vector2.Distance(transform.position, lockedTarget.position))
                {
                    Debug.Log("Player is Closer! No longer Targeting: " + lockedTarget.name);
                    lockedTarget = null;
                }
                else
                {
                    return lockedTarget;
                }
            }
        }

        
        
        if (portal != null)
        {
            if (distToPortal < distToPlayer && distToPortal < distToTower)
            {
                lockedTarget = portal;
                currentTarget = TargetType.Portal;
                Debug.Log("Locked onto PORTAL");
                return portal;
            }
        }

        if (closestTower != null && distToTower <= detectionRange)
        {
            if (distToTower < distToPlayer)
            {
                lockedTarget = closestTower;
                currentTarget = TargetType.Tower;
                Debug.Log("Locked onto TOWER: " + closestTower.name);
                return closestTower;
            }
        }

        if (player != null)
        {
            if (distToPlayer <= detectionRange)
            {
                currentTarget = TargetType.Player;
                Debug.Log("Chasing PLAYER");
                return player;
            }
        }

        currentTarget = TargetType.HomeTile;
        Debug.Log("Going to HOME TILE");
        return homeTile;


    }

    Transform FindClosestTower()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower"); 
        
        if (towers.Length == 0)
            return null;

        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject tower in towers)
        {
            float distance = Vector2.Distance(transform.position, tower.transform.position);
            
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = tower.transform;
            }
        }

        return closest;
    }


}
