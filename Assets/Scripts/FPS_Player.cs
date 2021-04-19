using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPS_Player : MonoBehaviour
{
    public GameObject bombPrefab;
    int cooldown = 0;
    const int speed = 10;
    public int range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)&&cooldown==0)
        {
            Vector3Int intVector = new Vector3Int((int)this.transform.position.x, (int)(this.transform.position.y+0.1f), (int)this.transform.position.z);
            Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 - (intVector.z) % 2);
            GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * speed;
            bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * range, 1, 1);
            bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * range, 1);
            bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * range);
            bomb.GetComponent<Bomb>().is3D = true;
            bomb.GetComponent<Bomb>().range = range;
            cooldown = 100;
        }
        if (cooldown != 0)
            cooldown--;
    }
}
