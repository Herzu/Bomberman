using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Player : MonoBehaviour
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
        Camera.main.transform.position = this.transform.position + new Vector3(0, 9, 0);
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            GameObject bomb = Instantiate(bombPrefab, this.transform.position + this.transform.forward + new Vector3(0, 1, 0), Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = this.gameObject.transform.forward * speed;
            cooldown = 10;
        }
        cooldown--;
    }
}
