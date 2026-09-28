using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("MUERTE")]
    public float deathDelay = 2f;

    // Posición inicial
    private Vector3 startPos;
    private Quaternion startRotation;

    // Evita activar la muerte varias veces
    private bool isDead = false;

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

        // Guardar posición inicial
        startPos = transform.position;
        startRotation = transform.rotation;
    }

    // =====================================================
    // UPDATE
    // =====================================================

    void Update()
    {
        if (isDead)
        {
            return;
        }

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
        // ANIMACIÓN
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
            if (yVelocity < 0)
            {
                yVelocity = -2f;
            }

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

                if (animator != null)
                {
                    animator.SetBool(
                        "IsJumping",
                        true
                    );
                }
            }
        }

        // =================================================
        // GRAVEDAD
        // =================================================

        yVelocity += gravity * Time.deltaTime;

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
    // COLISIÓN CON WALL
    // =====================================================

    private void OnControllerColliderHit(
        ControllerColliderHit hit
    )
    {
        if (hit.gameObject.CompareTag("Wall"))
        {
            Morir();
        }
    }

    // =====================================================
    // MORIR
    // =====================================================

    private void Morir()
    {
        if (isDead)
        {
            return;
        }

        isDead = true;
        canMove = false;
        yVelocity = 0f;

        // Animación de muerte
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("IsJumping", false);
            animator.SetBool("IsDead", true);
        }

        // Esperar antes de regresar al inicio
        StartCoroutine(RegresarAlInicio());
    }

    // =====================================================
    // REGRESAR AL START POS
    // =====================================================

    private IEnumerator RegresarAlInicio()
    {
        yield return new WaitForSeconds(deathDelay);

        // Desactivar temporalmente el CharacterController
        _characterController.enabled = false;

        // Volver a la posición inicial
        transform.position = startPos;
        transform.rotation = startRotation;

        // Reactivar CharacterController
        _characterController.enabled = true;

        // Resetear gravedad
        yVelocity = 0f;

        // Quitar estado de muerte
        isDead = false;

        // Quitar animación de muerte
        if (animator != null)
        {
            animator.SetBool("IsDead", false);
            animator.SetBool("IsJumping", false);
        }

        // Volver a permitir movimiento
        canMove = true;
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