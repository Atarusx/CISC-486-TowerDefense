using Unity.Netcode;
using UnityEngine;

public class NewMonoBehaviourScript : NetworkBehaviour
{

    public float moveSpeed;
    Rigidbody2D rb;
    [HideInInspector]
    public float lastHorVector;
    [HideInInspector]
    public float lastVertVector;
    [HideInInspector]
    public Vector2 moveDir;



    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        gameObject.tag = "Player";

        if (IsOwner)
        {
            Camera.main.GetComponent<CameraFollow>()?.SetTarget(transform);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

        if (!IsOwner) return;
        InputManagement();
    }

    void FixedUpdate()
    {
        if (IsOwner)
        {
            Move();
        }
    }

    void InputManagement()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        moveDir = new Vector2(moveX, moveY).normalized;

        if (moveDir.x != 0)
        {
            lastHorVector = moveDir.x;
        }

        if (moveDir.y != 0)
        {
            lastVertVector = moveDir.y;
        }
    }

    void Move()
    {
        rb.linearVelocity = new Vector2(moveDir.x * moveSpeed, moveDir.y * moveSpeed);
    }

}
