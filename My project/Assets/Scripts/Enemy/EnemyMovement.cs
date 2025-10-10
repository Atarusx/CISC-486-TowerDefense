using UnityEngine;

public class EnemyMovement : MonoBehaviour
{


    Transform player;
    public float moveSpeed;
    public float runSpeed;


    [Header("Detection Ranges")]
    public float detectionRange = 10f;
    public float runRange = 3f;

    private Animator animator;
    private float currentSpeed;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<NewMonoBehaviourScript>().transform;
        animator = GetComponent<Animator>();
        currentSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= runRange)
        {
            animator.SetBool("Move", true);
            animator.SetBool("Run", true);
            currentSpeed = runSpeed;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);
        }
        else if (distanceToPlayer <= detectionRange)
        {
            animator.SetBool("Move", true);
            animator.SetBool("Run", false);
            currentSpeed = moveSpeed;
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, currentSpeed * Time.deltaTime);
        }
        else
        {
            animator.SetBool("Move", false);
            animator.SetBool("Run", false);
        }
        
        
        
        
    }
}
