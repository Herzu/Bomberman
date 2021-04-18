using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Player : Character
{
    public GameObject bombPrefab;
    int cooldown = 0;
    const int speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(this.getBombs() > 0) {
                GameObject bomb = Instantiate(bombPrefab, this.transform.position+this.transform.forward+new Vector3(0,1,0), Quaternion.identity);
                bomb.GetComponent<Rigidbody>().velocity = this.transform.GetChild(0).gameObject.transform.forward * speed;
                cooldown = 10;
                this.setBombs(this.getBombs() - 1);
            }
        }
        isAlive();
        checkBomb();
        cooldown--;
    }
}
