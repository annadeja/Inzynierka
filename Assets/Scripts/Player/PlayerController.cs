using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private CharacterController charControl;
    private Transform transform;
    private Vector3 displacement;
    [SerializeField] Vector3 camDiff;
    public Camera mainCamera;
    [SerializeField] int speed = 7;
    [SerializeField] int rotationSpeed = 15;

    // Start is called before the first frame update
    void Start()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        charControl = gameObject.GetComponentInChildren<CharacterController>();
        transform = gameObject.GetComponentInChildren<Transform>();
        displacement = new Vector3(0, 0, 0);
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
                speed = 20;
            }
            else if (displacement.x != 0 || displacement.z != 0)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isRunning", false);
                speed = 7;
            }
            else
            {
                animator.SetBool("isWalking", false);
                animator.SetBool("isRunning", false);
            }
        }

        displacement.x = Input.GetAxis("Horizontal") * speed;
        if (charControl.isGrounded)
            displacement.y = 0;
        else
            displacement.y -= 4 * Time.deltaTime;
        displacement.z = Input.GetAxis("Vertical") * speed;

        displacement = transform.TransformDirection(displacement);
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
