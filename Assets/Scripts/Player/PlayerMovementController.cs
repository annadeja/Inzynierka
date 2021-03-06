﻿using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
//!Skrypt obsługujący ruch gracza.
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] public Animator animator; //!<Pole to nie może stanowić właściwości, gdyż inaczej nie byłoby możliwe przypisanie odpowiedniej referencji w edytorze Unity.
    private CharacterController charControl; //!<Kontroler ruchu postaci.
    private Transform transform; //!<Pozycja i transformacje pozycji gracza.
    private Vector3 displacement; //!<Ostatnia zmiana pozycji gracza.

    [SerializeField] private Vector3 camDiff; //!<Różnica pomiędzy pozycją kamery a gracza.
    [SerializeField] private Camera mainCamera; //!<Referencja kamery w scenie.

    [Header("Speed settings")]
    [SerializeField] private int walkingSpeed; //!<Szybkość chodu.
    [SerializeField] private int runningSpeed; //!<Szybkość biegu.
    [SerializeField] private int rotationSpeed = 100; //!<Szybkość obrotu.
    private int currentSpeed; //!<Obecna szybkość.
    public bool CanMove { get; set; } //!<Czy gracz może się poruszać?

    void Start()
    {
        CanMove = true;
        charControl = gameObject.GetComponentInChildren<CharacterController>();
        transform = gameObject.GetComponentInChildren<Transform>();
        displacement = new Vector3(0, 0, 0);
        currentSpeed = walkingSpeed;
        Cursor.visible = false;
    }

    void Update()
    {
        processInputs();
        calculateDisplacement();
        if(CanMove)
        {
            movePlayer();
            moveCamera();
        }
    }
    //!Interpretuje dane wprowadzane przez gracza.
    private void processInputs()
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
    }
    //!Oblicza zmianę pozycji.
    private void calculateDisplacement()
    {
        displacement.x = Input.GetAxis("Horizontal") * currentSpeed;
        if (charControl.isGrounded)
            displacement.y = 0;
        else
            displacement.y -= 4 * Time.deltaTime;
        displacement.z = Input.GetAxis("Vertical") * currentSpeed;
        displacement = transform.TransformDirection(displacement);
    }
    //!Porusza graczem.
    private void movePlayer()
    {
        charControl.Move(displacement * Time.deltaTime);
        transform.Rotate(0, Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, 0);
    }
    //!Porusza kamerą.
    private void moveCamera()
    {
        if (mainCamera)
        {
            camDiff = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime, Vector3.up) * camDiff;
            mainCamera.transform.position = transform.position + camDiff;
            mainCamera.transform.LookAt(transform.position);
        }
    }
}
