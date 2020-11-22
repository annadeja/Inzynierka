using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator animator;
    private CharacterController charControl;
    private Transform transform;
    private Vector3 displacement;

    [SerializeField] Vector3 camDiff;
    public Camera mainCamera;

    [Header("Speed settings")]
    [SerializeField] int walkingSpeed;
    [SerializeField] int runningSpeed;
    [SerializeField] int rotationSpeed = 100;
    private int currentSpeed;
    public bool canMove = true;

    // Start is called before the first frame update
    void Start()
    {
        charControl = gameObject.GetComponentInChildren<CharacterController>();
        transform = gameObject.GetComponentInChildren<Transform>();
        displacement = new Vector3(0, 0, 0);
        currentSpeed = walkingSpeed;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(animator)
        {
            if ((displacement.x != 0 || displacement.z > 0) && Input.GetKey("left shift"))
            {
                animator.SetBool("isRunning", true);
                animator.SetBool("isWalking", false);
                currentSpeed = runningSpeed;
}
            else if (displacement.x != 0 || displacement.z != 0)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                currentSpeed = walkingSpeed;
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }

        displacement.x = Input.GetAxis("Horizontal") * currentSpeed;
        if (charControl.isGrounded)
            displacement.y = 0;
        else
            displacement.y -= 4 * Time.deltaTime;
        displacement.z = Input.GetAxis("Vertical") * currentSpeed;
        displacement = transform.TransformDirection(displacement);

        if(canMove)
        {
            charControl.Move(displacement * Time.deltaTime);
            transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0);
            if (mainCamera)
            {
                camDiff = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, Vector3.up) * camDiff;
                mainCamera.transform.position = transform.position + camDiff;
                mainCamera.transform.LookAt(transform.position);
            }
        }
    }
}
