using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private OldInput _oldInput;
    private CharacterController _characterController;

    public float speed;
    public float rotationSpeed;
    private Animator animator;
    public float gravity = -9.81f;

    // Ahora p�blica para debug y boosts
    public float yVelocity;

    public float jumpHeight = 0.5f;

    private float _currentlookingPos;

    public bool isPlayer1;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        _oldInput = GetComponent<OldInput>();

        if (_oldInput == null)
        {
            _oldInput = gameObject.AddComponent<OldInput>();
        }

        _characterController =
        GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWalk();
    }

    // M�todo para trampolines / boosts
    public void JumpBoost(float force)
    {
        yVelocity = force;

        Debug.Log("Boost vertical: " + force);
    }

    public void PlayerWalk()
    {
        float horizontal;
        float vertical;
        bool jump;

        // Inputs dependiendo del jugador
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
        float movementAmount =
    Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        animator.SetBool(
            "isRunning",
            movementAmount > 0.1f
        );
        // Movimiento base
        Vector3 move =
        new Vector3(horizontal, 0, vertical);

        // Convertir direcci�n
        move = transform.TransformDirection(move);

        // Aplicar velocidad horizontal
        move *= speed;

        // Revisar suelo
        if (_characterController.isGrounded
            && yVelocity < 0)
        {
            // Mantener pegado al suelo
            yVelocity = -2f;

            // Salto normal
            if (jump)
            {
                yVelocity =
                Mathf.Sqrt(jumpHeight * -2f * gravity);

                AudioManager.Instance.PlaySFX(
                AudioManager.Instance.jumpSFX
                );
            }
        }

        // Aplicar gravedad
        yVelocity += gravity * Time.deltaTime;

        // Movimiento vertical
        move.y = yVelocity;

        // Movimiento final
        _characterController.Move(
            move * Time.deltaTime
        );
    }
}