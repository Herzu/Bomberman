using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class TomController : MonoBehaviour
{
    public CharacterController characterController;
    public float speed = 3;
    public GameObject bombPrefab;
    int cooldown = 0;
    public int range;

    public Animator animator;

    private void Move()
    {
        if (animator.GetBool("isPlanting") == true)
            return;
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        //Vector3 move = /*transform.forward **/ horizontalMove + /*transform.right * -*/ verticalMove + transform.up * -1;
        Vector3 move = new Vector3(1f * horizontalMove, 0f, 1f * verticalMove);
        //transform.rotation = Quaternion.Euler(0f, 90f + horizontalMove*90f+verticalMove*90f, 0f);
        if (move != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(move) * Quaternion.Euler(0f, -90f, 0f);

        move += transform.up * -1;

        characterController.Move(speed * Time.deltaTime * move);

        Camera.main.transform.position = this.transform.position + new Vector3(0, 9, 0);

        animator.SetBool("isWalking", horizontalMove != 0 || verticalMove != 0);
    }

    private void Bomb()
    {
        if (Input.GetKeyDown(KeyCode.LeftAlt) && cooldown == 0)
        {
            animator.SetBool("isPlanting", true);
            cooldown = 150;
        }
        if (cooldown == 75)
        {
            Vector3Int intVector = new Vector3Int((int)this.transform.position.x, (int)this.transform.position.y, (int)this.transform.position.z);
            Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 + (intVector.z) % 2);
            GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * 0;
            bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * range, 1, 1);
            bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * range, 1);
            bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * range);
            bomb.GetComponent<Bomb>().is3D = false;
            bomb.GetComponent<Bomb>().range = range;
        }
        if (cooldown != 0)
            cooldown--;
        if (cooldown == 0)
        {
            animator.SetBool("isPlanting", false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Bomb();
    }
}