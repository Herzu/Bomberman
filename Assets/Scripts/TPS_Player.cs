using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Player : Character
{
    public GameObject bombPrefab;
    int cooldown = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = this.transform.position + new Vector3(0, 9, 0);
        if (Input.GetKeyDown(KeyCode.LeftAlt) && this.cooldown == 0 && this.bombs > 0)
        {
            Vector3Int intVector = new Vector3Int((int)this.transform.position.x,(int)this.transform.position.y,(int)this.transform.position.z);
            Vector3 bombPlacement = intVector + new Vector3(1-(intVector.x)%2, 1, 1-(intVector.z) % 2);
            GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * this.speed;
            bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * this.range, 1, 1);
            bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * this.range, 1);
            bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * this.range);
            bomb.GetComponent<Bomb>().is3D = false;
            bomb.GetComponent<Bomb>().range = this.range;
            this.cooldown = 100;
            this.bombs -= 1;
        }
        this.isAlive();
        this.checkBomb();
        this.checkImmunity();
        if(this.cooldown!=0)
            this.cooldown--;
    }
}
