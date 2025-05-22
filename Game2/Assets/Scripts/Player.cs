using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 8f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private int maxJumps = 2;
    [SerializeField] private float jumpBufferTime = 0.2f;
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float coyoteTime = 2f;
    [SerializeField] private float dashSpeed = 16f;
    [SerializeField] private float dashDuration = 0.5f;
    [SerializeField] private float dashBufferTime = 0.7f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Boundary")]
    [SerializeField] private float fallLimitY = -10f;

    private int jumpsLeft;
    private Rigidbody2D rb;
    private float horizontalInput;
    private bool isGrounded;
    private bool facingRight = true;
    private float jumpBufferCounter = 0;
    private float coyoteTimeCounter = 0;
    private float dashBufferCounter = 0;
    private bool isDashing = false;
    private Vector2 respawnPosition;

    private void Start()
    {
        respawnPosition = transform.position;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y < fallLimitY)
        {
            ResetPlayer();
        }

        if (Input.GetButtonDown("Reset"))
        {
            ResetPlayer();
        }

        if (isGrounded && rb.velocity.y <= 0)
        {
            jumpsLeft = maxJumps;
        }

        if (isGrounded)
            coyoteTimeCounter = coyoteTime;
        else
            coyoteTimeCounter -= Time.deltaTime;

        if (jumpBufferCounter > 0)
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && jumpBufferCounter <= 0 && (coyoteTimeCounter > 0 || jumpsLeft == 1))
        {
            Jump();
            jumpBufferCounter = jumpBufferTime;
            coyoteTimeCounter = 0;
        }
        
        if(dashBufferCounter > 0)
        {
            dashBufferCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Fire3") && dashBufferCounter <= 0)
        {
            startDash();
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");

        if ((horizontalInput > 0 && !facingRight) || (horizontalInput < 0 && facingRight))
        {
            Flip();
        }

        rb.gravityScale = (rb.velocity.y < 0) ? fallMultiplier : 1;

        Vector2 lookDirection = facingRight ? Vector2.right : Vector2.left;

        if (Input.GetKeyDown(KeyCode.W))
        {
            RaycastHit2D hit = Physics2D.Raycast(rb.position, lookDirection, 1.5f, LayerMask.GetMask("Interaction"));
 
            if (hit.collider != null)
            {
                Debug.Log("检测到交互: " + hit.collider.name);
                GameObject hitObject = hit.collider.gameObject;

                if(hitObject.tag == "Door")
                {
                    Debug.Log("检测到门: " + hitObject.name);
                    RoomDoor roomDoor = hitObject.GetComponent<RoomDoor>();
                    if (roomDoor != null)
                    {
                        Destroy(gameObject);
                        roomDoor.Event();   
                    }
                }

                else if(hitObject.tag == "Locked")
                {
                    Debug.Log("上锁了。");
                }

                else if(hitObject.tag == "NPC")
                {
                    Debug.Log("检测到NPC: " + hitObject.name);
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (character != null)
                    {
                        character.DisplayDialog();
                    }
                }
                
            }
        }

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (!isDashing)
            rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        jumpsLeft--;
        rb.velocity = new Vector2(rb.velocity.x, 0); 
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    private void startDash()
    {
        rb.velocity = new Vector2(facingRight ? dashSpeed : -dashSpeed, rb.velocity.y);
        isDashing = true;
        dashBufferCounter = dashBufferTime;
        Invoke(nameof(stopDash), dashDuration);
    }

    private void stopDash()
    {
        isDashing = false;
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
    }

    private void ResetPlayer()
    {
        //transform.position = respawnPosition;
        //rb.velocity = Vector2.zero;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Hazad"))
        {
            Debug.Log("玩家失败!");
            ResetPlayer();
        }

        if (other.CompareTag("FallTrap"))
        {
            FallingTrigger fallingTrigger = other.GetComponent<FallingTrigger>();
            if (fallingTrigger != null)
            {
                fallingTrigger.ActivateTrap();
            }
        }

        if (other.CompareTag("HorizonTrap"))
        {
            HorizonTrigger horizonTrigger = other.GetComponent<HorizonTrigger>();
            if (horizonTrigger != null)
            {
                horizonTrigger.ActivateTrap();
            }
        }

        if (other.CompareTag("Protal"))
        {
            Debug.Log("正在传送...");
            SceneProtal sceneProtal = other.GetComponent<SceneProtal>();
            if (sceneProtal != null)
            {
                sceneProtal.Teleport();
            }
        }

        if(other.CompareTag("TeleportTrigger"))
        {
            Debug.Log("正在移动...");
            TeleportTrigger teleportTrigger = other.GetComponent<TeleportTrigger>();
            if (teleportTrigger != null)
            {
                if (!teleportTrigger._hasTriggered)
                {
                    teleportTrigger.TeleportMove();
                    teleportTrigger._hasTriggered = true;
                }
            }
        }

        if (other.CompareTag("SmoothTrigger"))
        {
            Debug.Log("正在移动...");
            SmoothTrigger smoothTrigger = other.GetComponent<SmoothTrigger>();
            if (smoothTrigger != null)
            {
                if (!smoothTrigger._hasTriggered)
                {
                    smoothTrigger.StartMove();
                }
            }
        }

        if (other.CompareTag("Destroy"))
        {
            DestroyTrigger destroyTrigger = other.GetComponent<DestroyTrigger>();
            if (destroyTrigger != null)
            {
                destroyTrigger.DestroyObject();
                
            }
        }
    }

    private void Flip()
    {
        Debug.Log("翻转角色");
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}