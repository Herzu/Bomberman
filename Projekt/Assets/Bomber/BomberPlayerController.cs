using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class BomberPlayerController : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 3;


    public Animator animator;

    void Update()
    {
        Move();
    }

    private void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        Vector3 move = transform.forward + transform.right * horizontalMove;
        characterController.Move(speed * Time.deltaTime * move);

        animator.SetBool("isWalking", horizontalMove != 0);




    }
}