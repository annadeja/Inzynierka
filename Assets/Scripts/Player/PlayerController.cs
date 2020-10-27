using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private CharacterController charControl;
    private Transform transform;
    [SerializeField] Vector3 displacement;
    [SerializeField] Vector3 camDiff;
    public Camera mainCamera;
    private int speed = 7;

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
                speed = 14;
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
        displacement.y -= 4 * Time.deltaTime;
        displacement.z = Input.GetAxis("Vertical") * speed;

        charControl.Move(displacement * Time.deltaTime);

        if (mainCamera)
            mainCamera.transform.position = transform.position + camDiff;

    }
}
