using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private OldInput _oldInput;
    private CharacterController _characterController;
    private Animator animator;

    [Header("MOVIMIENTO")]
    public float speed = 5f;
    public float rotationSpeed = 10f;

    [Header("GRAVEDAD")]
    public float gravity = -9.81f;
    public float yVelocity;
    public float jumpHeight = 0.5f;

    [Header("JUGADOR")]
    public bool isPlayer1;

    [Header("CONTROL DEL JUGADOR")]
    public bool canMove = true;

    private float _currentlookingPos;

    // =====================================================
    // START
    // =====================================================

    void Start()
    {
        animator = GetComponent<Animator>();

        _oldInput = GetComponent<OldInput>();

        if (_oldInput == null)
        {
            _oldInput = gameObject.AddComponent<OldInput>();
        }

        _characterController = GetComponent<CharacterController>();
    }

    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        PlayerWalk();
    }

    // =====================================================
    // ACTIVAR / DESACTIVAR MOVIMIENTO
    // =====================================================

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled && animator != null)
        {
            animator.SetBool("isRunning", false);
        }
    }

    // =====================================================
    // MOVIMIENTO
    // =====================================================

    public void PlayerWalk()
    {
        float horizontal = 0f;
        float vertical = 0f;
        bool jump = false;

        // =================================================
        // INPUT
        // =================================================

        if (canMove)
        {
            if (isPlayer1)
            {
                horizontal = _oldInput.horizontalP1;
                vertical = _oldInput.verticalP1;
                jump = _oldInput.jumpP1;
            }
            else
            {
                horizontal = _oldInput.horizontalP2;
                vertical = _oldInput.verticalP2;
                jump = _oldInput.jumpP2;
            }
        }

        // =================================================
        // SUELO
        // =================================================

        bool grounded = _characterController.isGrounded;

        // =================================================
        // ANIMACIÓN DE MOVIMIENTO
        // =================================================

        float movementAmount =
            Mathf.Abs(horizontal) +
            Mathf.Abs(vertical);

        if (animator != null)
        {
            animator.SetBool(
                "isRunning",
                canMove &&
                grounded &&
                movementAmount > 0.1f
            );

            // Está saltando cuando NO está en el suelo
            animator.SetBool(
                "IsJumping",
                !grounded
            );
        }

        // =================================================
        // MOVIMIENTO HORIZONTAL
        // =================================================

        Vector3 move =
            new Vector3(
                horizontal,
                0,
                vertical
            );

        move = transform.TransformDirection(move);

        move *= speed;

        // =================================================
        // SALTO
        // =================================================

        if (grounded)
        {
            // Mantener al jugador pegado al suelo
            if (yVelocity < 0)
            {
                yVelocity = -2f;
            }

            // Saltar
            if (canMove && jump)
            {
                yVelocity =
                    Mathf.Sqrt(
                        jumpHeight * -2f * gravity
                    );

                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(
                        AudioManager.Instance.jumpSFX
                    );
                }

                // Activar animación inmediatamente
                if (animator != null)
                {
                    animator.SetBool("IsJumping", true);
                }
            }
        }

        // =================================================
        // GRAVEDAD
        // =================================================

        yVelocity += gravity * Time.deltaTime;

        // Evitar que la velocidad de caída se vuelva absurda
        if (yVelocity < -20f)
        {
            yVelocity = -20f;
        }

        move.y = yVelocity;

        // =================================================
        // MOVIMIENTO FINAL
        // =================================================

        _characterController.Move(
            move * Time.deltaTime
        );
    }

    // =====================================================
    // BOOST / TRAMPOLÍN
    // =====================================================

    public void JumpBoost(float force)
    {
        yVelocity = force;

        if (animator != null)
        {
            animator.SetBool("IsJumping", true);
        }

        Debug.Log("Boost vertical: " + force);
    }
}