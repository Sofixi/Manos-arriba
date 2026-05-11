
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private OldInput _oldInput;
    private CharacterController _characterController;

    public float speed;
    public float rotationSpeed;

    public float gravity = -9.81f;

    private float _currentlookingPos;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        _oldInput = GetComponent<OldInput>();
        _characterController = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWalk();
        PlayerRotation();
        CamAnim();
    }

    public void PlayerWalk()
    {
        Vector3 inputVector = new Vector3(0, 0, _oldInput.vertical);

        inputVector = transform.TransformDirection(inputVector);

        Vector3 movementVector = (inputVector * speed) + (Vector3.up * gravity);

        _characterController.Move(movementVector * Time.deltaTime);
    }


    public void PlayerRotation()
    {
        float rotationInput = _oldInput.horizontal * rotationSpeed * Time.deltaTime;

        _currentlookingPos += rotationInput;

        transform.localRotation = Quaternion.AngleAxis(_currentlookingPos, transform.up);
    }

    public void CamAnim()
    {
        animator.SetBool("isWalking", _oldInput.vertical != 0);
    }
}
