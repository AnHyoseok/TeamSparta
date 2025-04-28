using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ZombieMove : MonoBehaviour
{
    private Animator animator;
    public float moveSpeed = 2f;
    public float jumpForce = 3.8f;
    public float detectRange = 1.0f;
    public float jumpDelay = 1.0f;
    public float towerDetectRange = 2.5f;

    private Rigidbody2D rb;
    private float jumpTimer;
    private bool isJumping = false;
    private float normalSpeed;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.freezeRotation = true;
        normalSpeed = moveSpeed;
        jumpTimer = Random.Range(0.5f, jumpDelay);
    }

    private void FixedUpdate()
    {
        if (isJumping) return;
        if (IsTowerBlocked())
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(-moveSpeed, rb.linearVelocity.y);
        }

        if (isJumping && rb.linearVelocity.y <= 0f && IsGrounded())
        {
            moveSpeed = normalSpeed;
            isJumping = false;

        }
    }

    private void Update()
    {
        jumpTimer -= Time.deltaTime;

        if (IsNearTower())
        {
            if (jumpTimer <= 0f)
            {
                AttemptJump();
                jumpTimer = Random.Range(0.5f, jumpDelay);
            }
        }
    }

    private void AttemptJump()
    {
        Vector2 rayOrigin = transform.position + new Vector3(0f, 0.4f, 0f);
        Vector2 rayDir = (Vector2.left + Vector2.up * 0.5f).normalized;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDir, detectRange, LayerMask.GetMask("Zombie1", "Zombie2", "Zombie3"));
        Debug.DrawRay(rayOrigin, rayDir * detectRange, Color.red);

        if (hit.collider != null)
        {
            Jump();
        }
    }

    private void Jump()
    {
      
        Vector2 jumpDir = (Vector2.left * 0.3f + Vector2.up * 0.9f).normalized;
        rb.AddForce(jumpDir * jumpForce, ForceMode2D.Impulse);

        moveSpeed = normalSpeed * 1.5f; 
        isJumping = true;


    }

    private bool IsGrounded()
    {
        Vector2 rayOrigin = transform.position + new Vector3(0f, -0.3f, 0f);
        return Physics2D.Raycast(rayOrigin, Vector2.down, 0.2f, LayerMask.GetMask("Platform1", "Platform2", "Platform3"));
    }

    private bool IsTowerBlocked()
    {
        Vector2 rayOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, 0.5f, LayerMask.GetMask("Tower"));
        Debug.DrawRay(rayOrigin, Vector2.left * 0.5f, Color.blue);
        return hit.collider != null;
    }

    private bool IsNearTower()
    {
        Vector2 rayOrigin = transform.position;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.left, towerDetectRange, LayerMask.GetMask("Tower"));
        Debug.DrawRay(rayOrigin, Vector2.left * towerDetectRange, Color.green);
        return hit.collider != null;
    }
}
