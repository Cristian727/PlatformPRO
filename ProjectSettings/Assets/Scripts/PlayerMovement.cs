using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private SpriteRenderer sp;
    [HideInInspector] public Stats stats;
    [SerializeField] bool isGrounded = false;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float distanceToGround;
    [SerializeField] Transform[] groundCheckPoints;
    float currentJumpPressTime;
    [SerializeField] int performedJumpCount;
    LayerMask playerLayer;
    LayerMask arbolLayer;
    Animator animator;

    [SerializeField] Gradient StrengthColor;
    [SerializeField] ParticleSystem trailParticles;
    ParticleSystem.MainModule trailMainModule;
    float timeOnAir;

    float actualAcceleration;
    float actualFriction;
    float actualMaxSpeedX;

    private void OnDrawGizmosSelected()
    {
#if UNITY_EDITOR

        Gizmos.color = Color.red;
        Handles.color = Color.red;

        for (int i = 0; i < groundCheckPoints.Length; i++)
        {
            Gizmos.DrawLine(
                groundCheckPoints[i].position,
                groundCheckPoints[i].position + Vector3.down * distanceToGround
                );

            Handles.DrawWireDisc(
                groundCheckPoints[i].position,
                Vector3.back,
                0.02f
                );
        }
#endif
    }

    // Start is called before the first frame update
    void Start()
    {
        performedJumpCount = 0;
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        playerLayer = LayerMask.NameToLayer("Player");
        arbolLayer= LayerMask.NameToLayer("Arbol");

        trailMainModule = trailParticles.main;
    }
    //CapsuleCollider2D.enabled = true;
    // Update is called once per frame
    void Update()
    {

        Vector2 movementInput = Vector2.zero;

        //que el movimiento vaya por el new input sistem

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            movementInput.x += 1;
            sp.flipX = false;
            if (isGrounded)
                animator.SetTrigger("run");
        }
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            movementInput.x -= 1;
            sp.flipX = true;
            if (isGrounded)
                animator.SetTrigger("run");
        }

        if (rb.velocity.magnitude < 0.01f && isGrounded)
        {
            animator.SetTrigger("idle");
            trailMainModule.startColor = Color.white;
        }
        else if (rb.velocity.y < -5)
        {
            animator.SetTrigger("falling");
            trailMainModule.startColor = Color.red;
        }


        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            currentJumpPressTime = 0;
            performedJumpCount += 1;
            animator.SetTrigger("jump");
            trailMainModule.startColor = Color.green;

        }

        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) && performedJumpCount <= stats.onAirJumps)
        {
            if (currentJumpPressTime <= stats.maxJumpPressTime)
            {
                currentJumpPressTime += Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, stats.jumpStrength);
                if (!isGrounded)
                    animator.SetTrigger("jump");

            }
        }


        if (isGrounded)
        {
            actualAcceleration = stats.groundAcceleration;
            actualFriction = stats.groundFriction;
            actualMaxSpeedX = stats.groundMaxSpeedX;
        }
        else
        {
            actualAcceleration = stats.airAcceleration;
            actualFriction = stats.airFriction;
            actualMaxSpeedX = stats.airMaxSpeedX;
        }

        if (movementInput.x != 0)
            rb.velocity += movementInput * actualAcceleration * Time.deltaTime;
        else
            rb.velocity = new Vector2(rb.velocity.x / actualFriction, rb.velocity.y);


        if (rb.velocity.x > actualMaxSpeedX)
        {
            rb.velocity = new Vector2(actualMaxSpeedX, rb.velocity.y);
        }
        if (rb.velocity.x < -actualMaxSpeedX)
        {
            rb.velocity = new Vector2(-actualMaxSpeedX, rb.velocity.y);
        }
        if (rb.velocity.y < -stats.maxFallSpeed)
        {
            rb.velocity = new Vector2(rb.velocity.x, -stats.maxFallSpeed);
        }


        if (rb.velocity.y < stats.yVelocityLowGravityThreshold && rb.velocity.y > -stats.yVelocityLowGravityThreshold)
        {
            rb.gravityScale = stats.peakGravity;
            trailMainModule.startColor = Color.yellow;
        }
        else if (rb.velocity.y > 0)
        {
            rb.gravityScale = stats.upGravity;
            trailMainModule.startColor = Color.green;
        }
        else
        {

            rb.gravityScale = stats.downGravity;
            trailMainModule.startColor = Color.red;
        }

        isGrounded = false;

        for (int i = 0; i < groundCheckPoints.Length; i++)
        {
            bool hit = Physics2D.Raycast(
                groundCheckPoints[i].position,
                Vector2.down,
                distanceToGround,
                groundLayer);


            if (hit)
            {
                timeOnAir = 0;
                isGrounded = true;
                trailMainModule.startColor = Color.white;
                performedJumpCount = 0;
                rb.gravityScale = stats.upGravity;
                break;
            }
        }
        if (isGrounded != true)
        {
            timeOnAir = timeOnAir += Time.deltaTime;

            if (movementInput.x != 0)
            {
                rb.velocity += movementInput * stats.airAcceleration * Time.deltaTime;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x / stats.airFriction, rb.velocity.y);
            }

            if (rb.velocity.x > stats.airMaxSpeedX)
            {
                rb.velocity = new Vector2(stats.airMaxSpeedX, rb.velocity.y);
            }
            if (rb.velocity.x < -stats.airMaxSpeedX)
            {
                rb.velocity = new Vector2(-stats.airMaxSpeedX, rb.velocity.y);
            }

        }



        Physics2D.IgnoreLayerCollision(playerLayer, arbolLayer, rb.velocity.y > 0);




        sp.color = StrengthColor.Evaluate(1f * performedJumpCount / stats.onAirJumps);


    }
}
