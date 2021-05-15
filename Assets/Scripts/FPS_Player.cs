using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Player : Character
{
    public GameObject bombPrefab;
    public GameObject thrownBombPrefab;
    int bombSpeed = 10;
    int cooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && this.cooldown == 0 && this.bombs > 0)
        {
            GameObject bomb = Instantiate(thrownBombPrefab, gameObject.transform.position+new Vector3(0,0.3f,0), Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = Camera.main.gameObject.transform.forward * bombSpeed;
            bomb.GetComponent<ThrownBomb>().bombPrefab = bombPrefab;
            bomb.GetComponent<ThrownBomb>().bombLifetime = bombLifetime;
            bomb.GetComponent<ThrownBomb>().range = range;
            bomb.GetComponent<ThrownBomb>().is3D = true;
            this.cooldown = 10;
            this.placeBomb();
        }
        this.isAlive();
        this.checkBomb();
        this.checkImmunity();
        if (this.cooldown != 0)
            this.cooldown--;
    }
}
