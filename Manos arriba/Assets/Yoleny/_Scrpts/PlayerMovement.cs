using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private OldInput _oldInput;
    private CharacterController _characterController;

    public float speed;
    public float rotationSpeed;

    public float gravity = -9.81f;
    private float yVelocity;
    public float jumpHeight = 0.5f;
    private float _currentLookingPos;

    public bool isPlayer1;

    float horizontal;
    float vertical;

    //public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _oldInput = GetComponent<OldInput>();
        if (_oldInput == null)
        {
            _oldInput = gameObject.AddComponent<OldInput>();
        }
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWalk();
        PlayerRotation();

        //CamAnim();
    }

    public void PlayerWalk()
    {
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

        // Movimiento horizontal y vertical
        Vector3 inputVector = new Vector3(0, 0, vertical);

        // Convierte el movimiento según la dirección del jugador
        inputVector = transform.TransformDirection(inputVector);

        // Revisar si está tocando el suelo
        if (_characterController.isGrounded)
        {
            // Mantiene al jugador pegado al piso
            yVelocity = -2f;

            // Si presiona salto
            if (jump)
            {
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }

        // Aplicar gravedad
        yVelocity += gravity * Time.deltaTime;

        // Agregar movimiento vertical
        inputVector.y = yVelocity;

        // Mover jugador
        _characterController.Move(inputVector * speed * Time.deltaTime);
    }

    public void PlayerRotation() // Método para rotar al jugador
    {
        float rotationInput = horizontal * rotationSpeed * Time.deltaTime; // Calculo la rotación con input horizontal

        _currentLookingPos += rotationInput; // Acumulo la rotación

        transform.localRotation = Quaternion.AngleAxis(_currentLookingPos, transform.up); // Aplico la rotación al jugador
    }
}