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
    [Header("MANCHA DE GRASA")]
    public GameObject manchaPantalla;
    public float duracionMancha = 3f;
    public float multiplicadorGrasa = 0.5f;

    private Coroutine efectoGrasa;
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
        if (isDead) return;

        isDead = true;
        canMove = false;
        yVelocity = 0f;

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("IsJumping", false);

            animator.ResetTrigger("IsDead");
            animator.SetTrigger("IsDead");
        }

        // ESTA LÍNEA FALTABA
        StartCoroutine(EsperarMuerte());
    }
    private IEnumerator EsperarMuerte()
    {
        if (animator == null)
        {
            yield return new WaitForSeconds(deathDelay);
            RegresarAlInicio();
            yield break;
        }

        // Dar tiempo a Unity para procesar el Trigger.
        yield return null;

        float tiempoEspera = 0f;
        float limiteEspera = 2f;

        // Esperar a que entre en el estado de muerte.
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")
               && tiempoEspera < limiteEspera)
        {
            tiempoEspera += Time.deltaTime;
            yield return null;
        }

        // Si no entró en Dead, evitar que quede congelado.
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Dead"))
        {
            Debug.LogError(
                "No se encontró el estado Dead. Revisa la transición del Animator."
            );

            RegresarAlInicio();
            yield break;
        }

        // Esperar a que termine de entrar al estado.
        while (animator.IsInTransition(0))
        {
            yield return null;
        }

        // Esperar a que termine toda la animación.
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("Dead")
               && animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        RegresarAlInicio();
    }

    // =====================================================
    // REGRESAR AL START POS
    // =====================================================

    private void RegresarAlInicio()
    {
        CharacterController controller = GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        // Regresar a la posición inicial
        transform.position = startPos;
        transform.rotation = startRotation;

        yVelocity = 0f;

        if (controller != null)
            controller.enabled = true;

        if (animator != null)
        {
            animator.ResetTrigger("IsDead");
            animator.SetBool("isRunning", false);
            animator.SetBool("IsJumping", false);
        }

        isDead = false;
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

 //Mari animacion recoger//
 // =====================================================
    // DETECTAR INGREDIENTES / OBJETOS Y ACTIVAR ANIMACIÓN
    // =====================================================


    public void TriggerPickUp()
    {
        if (animator != null)
        {
            animator.SetTrigger("IsPickUp");
        }
    }
    // =====================================================
    // ACTIVAR ANIMACIÓN DE VICTORIA AL LLEGAR A LA META
    // =====================================================
    public void ActivarGrasa()
    {
        // Evitar reiniciar el efecto si ya está activo
        if (efectoGrasa != null)
            return;

        efectoGrasa = StartCoroutine(EfectoGrasa());
    }

    private IEnumerator EfectoGrasa()
    {
        float velocidadOriginal = speed;

        // Reducir velocidad
        speed = velocidadOriginal * multiplicadorGrasa;

        if (manchaPantalla != null)
        {
            manchaPantalla.SetActive(true);

            // Obtener la imagen del Canvas
            UnityEngine.UI.Image imagen =
                manchaPantalla.GetComponent<UnityEngine.UI.Image>();

            // Si la imagen está dentro de otro objeto
            if (imagen == null)
            {
                imagen =
                    manchaPantalla.GetComponentInChildren<UnityEngine.UI.Image>();
            }

            if (imagen != null)
            {
                Color color = imagen.color;

                // Aparecer completamente visible
                color.a = 1f;
                imagen.color = color;

                float tiempo = 0f;

                while (tiempo < duracionMancha)
                {
                    tiempo += Time.deltaTime;

                    // Desvanecer durante los 3 segundos
                    color.a = Mathf.Lerp(
                        1f,
                        0f,
                        tiempo / duracionMancha
                    );

                    imagen.color = color;

                    yield return null;
                }
            }
            else
            {
                // Si no hay Image, esperar normalmente
                yield return new WaitForSeconds(duracionMancha);
            }

            manchaPantalla.SetActive(false);
        }
        else
        {
            yield return new WaitForSeconds(duracionMancha);
        }

        // Restaurar velocidad
        speed = velocidadOriginal;

        efectoGrasa = null;
    }
    public void Ganar()
    {
        canMove = false;
        yVelocity = 0f;

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("IsJumping", false);
            animator.SetTrigger("IsWin");
        }
    
    }
    
}