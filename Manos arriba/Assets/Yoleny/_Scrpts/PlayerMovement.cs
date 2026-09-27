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

        // Si bloqueamos al jugador,
        // nos aseguramos de que no esté en Running.
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
        // SI PUEDE MOVERSE
        // =================================================

        if (canMove)
        {
            // -----------------------------
            // JUGADOR 1
            // -----------------------------

            if (isPlayer1)
            {
                horizontal = _oldInput.horizontalP1;
                vertical = _oldInput.verticalP1;
                jump = _oldInput.jumpP1;
            }

            // -----------------------------
            // JUGADOR 2
            // -----------------------------

            else
            {
                horizontal = _oldInput.horizontalP2;
                vertical = _oldInput.verticalP2;
                jump = _oldInput.jumpP2;
            }
        }

        // =================================================
        // ANIMATOR
        // =================================================

        float movementAmount =
            Mathf.Abs(horizontal) +
            Mathf.Abs(vertical);

        if (animator != null)
        {
            animator.SetBool(
                "isRunning",
                canMove && movementAmount > 0.1f
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

        // Convertir dirección según la rotación
        // del personaje
        move = transform.TransformDirection(move);

        // Aplicar velocidad
        move *= speed;

        // =================================================
        // SUELO Y SALTO
        // =================================================

        if (_characterController.isGrounded &&
            yVelocity < 0)
        {
            // Mantener al personaje pegado al suelo
            yVelocity = -2f;

            // Solo puede saltar si tiene el movimiento activado
            if (canMove && jump)
            {
                yVelocity =
                    Mathf.Sqrt(
                        jumpHeight * -2f * gravity
                    );

                // Sonido de salto
                if (AudioManager.Instance != null)
                {
                    AudioManager.Instance.PlaySFX(
                        AudioManager.Instance.jumpSFX
                    );
                }
            }
        }

        // =================================================
        // GRAVEDAD
        // =================================================

        yVelocity += gravity * Time.deltaTime;

        // Movimiento vertical
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

        Debug.Log("Boost vertical: " + force);
    }
}