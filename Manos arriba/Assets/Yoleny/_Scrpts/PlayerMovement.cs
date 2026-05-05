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

    private float _currentlookingPos;

    public bool isPlayer1;

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
        PlayerWalkHorizontal();
        
        //CamAnim();
    }

    public void PlayerWalk()
    {
        if (isPlayer1)
        {
            Vector3 inputVector = new Vector3(0, 0, _oldInput.verticalP1);

            inputVector = transform.TransformDirection(inputVector);

            Vector3 movementVector = (inputVector * speed) + (Vector3.up * gravity);

            _characterController.Move(movementVector * Time.deltaTime);
        }
        else
        {
            Vector3 inputVector = new Vector3(0, 0, _oldInput.verticalP2);

            inputVector = transform.TransformDirection(inputVector);

            Vector3 movementVector = (inputVector * speed) + (Vector3.up * gravity);

            _characterController.Move(movementVector * Time.deltaTime);
        }

    }

    public void PlayerWalkHorizontal()
    {
        if (isPlayer1)
        {
            Vector3 inputVector = new Vector3(_oldInput.horizontalP1, 0, 0);

            inputVector = transform.TransformDirection(inputVector);

            Vector3 movementVector = (inputVector * speed) + (Vector3.up * gravity);

            _characterController.Move(movementVector * Time.deltaTime);
        }
        else
        {
            Vector3 inputVector = new Vector3(_oldInput.horizontalP2, 0, 0);

            inputVector = transform.TransformDirection(inputVector);

            Vector3 movementVector = (inputVector * speed) + (Vector3.up * gravity);

            _characterController.Move(movementVector * Time.deltaTime);

        }
    }

    public void PlayerRotation()
    {
        if (isPlayer1)
        {
            float rotationInput = _oldInput.horizontalP1 * rotationSpeed * Time.deltaTime;

            _currentlookingPos += rotationInput;

            transform.localRotation = Quaternion.AngleAxis(_currentlookingPos, transform.up);
        }
        else
        {
            float rotationInput = _oldInput.horizontalP2 * rotationSpeed * Time.deltaTime;

            _currentlookingPos += rotationInput;

            transform.localRotation = Quaternion.AngleAxis(_currentlookingPos, transform.up);
        }
    }
}