using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public int powerupID;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Rotate(0.0f, 1.0f, 0.0f,Space.Self);
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            switch (powerupID)
            {
                case 1:
                    if (col.gameObject.GetComponent<TPS_Player>() != null)
                        col.gameObject.GetComponent<TPS_Player>().range++;
                    if (col.gameObject.GetComponent<FPS_Player>() != null)
                        col.gameObject.GetComponent<FPS_Player>().range++;
                    break;
                case 2:
                    //movespeed up
                    break;
                case 3:
                    //hp up
                    break;
                case 4:
                    //temporal immunity
                    break;
                case 5:
                    //more bombs
                    break;
                case 6:
                    //additional points
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
