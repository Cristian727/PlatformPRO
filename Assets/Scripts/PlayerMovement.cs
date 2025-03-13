using System.Collections;
using System.Collections.Generic;
using UnityEditor;

using UnityEngine;
using UnityEngine.InputSystem; 

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

    // Referencias para las nuevas acciones
    private InputAction movementAction;
    private InputAction jumpAction;

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
        arbolLayer = LayerMask.NameToLayer("Arbol");

        trailMainModule = trailParticles.main;

        // Inicializar el sistema de Input Actions
        var controls = new Controls();
        movementAction = controls.Player.Movement; 
        jumpAction = controls.Player.Jump;

        // Habilitar las acciones
        movementAction.Enable();
        jumpAction.Enable();
    }

    private void OnDisable()
    {
        // Deshabilitar las acciones cuando el script se desactive
        movementAction.Disable();
        jumpAction.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 movementInput = Vector2.zero;

       
        float horizontalInput = movementAction.ReadValue<float>(); 

        
        if (horizontalInput > 0) 
        {
            movementInput.x = 1;
            sp.flipX = false;
            if (isGrounded)
                animator.SetTrigger("run");
        }
        else if (horizontalInput < 0)
        {
            movementInput.x = -1;
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
        //Salto
        if (jumpAction.triggered) // Si se ha presionado el botón de salto (Space o South en el gamepad)
        {
            currentJumpPressTime = 0;
            performedJumpCount += 1;
            animator.SetTrigger("jump");
            trailMainModule.startColor = Color.green;
        }

        // Salto prolongado: Si el jugador mantiene presionado el botón de salto
        if (jumpAction.ReadValue<float>() > 0 && performedJumpCount <= stats.onAirJumps)
        {
            if (currentJumpPressTime <= stats.maxJumpPressTime)
            {
                currentJumpPressTime += Time.deltaTime;
                rb.velocity = new Vector2(rb.velocity.x, stats.jumpStrength);
                if (!isGrounded)
                    animator.SetTrigger("jump");
            }
        }

        // Determinar el tipo de movimiento en el aire o en el suelo
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

        // Aplicar el movimiento
        if (movementInput.x != 0)
            rb.velocity += movementInput * actualAcceleration * Time.deltaTime;
        else
            rb.velocity = new Vector2(rb.velocity.x / actualFriction, rb.velocity.y);

        // Limitar la velocidad máxima en X
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

        // Configuración de gravedad y colores según la velocidad en Y
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

        // Comprobar si está tocando el suelo
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
            timeOnAir += Time.deltaTime;

            if (movementInput.x != 0)
            {
                rb.velocity += movementInput * stats.airAcceleration * Time.deltaTime;
            }
            else
            {
                rb.velocity = new Vector2(rb.velocity.x / stats.airFriction, rb.velocity.y);
            }

            // Limitar la velocidad máxima en X en el aire
            if (rb.velocity.x > stats.airMaxSpeedX)
            {
                rb.velocity = new Vector2(stats.airMaxSpeedX, rb.velocity.y);
            }
            if (rb.velocity.x < -stats.airMaxSpeedX)
            {
                rb.velocity = new Vector2(-stats.airMaxSpeedX, rb.velocity.y);
            }
        }

        // Evitar colisiones con ciertos objetos
        Physics2D.IgnoreLayerCollision(playerLayer, arbolLayer, rb.velocity.y > 0);

        // Cambiar el color del personaje según el número de saltos realizados
        sp.color = StrengthColor.Evaluate(1f * performedJumpCount / stats.onAirJumps);
    }
}
