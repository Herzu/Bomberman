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
        if (col.gameObject.CompareTag("Player")|| col.gameObject.CompareTag("Bot"))
        {
            switch (powerupID)
            {
                case 1:
                    col.gameObject.GetComponent<Character>().range++;
                    break;
                case 2:
                    col.gameObject.GetComponent<Character>().speed += 5;
                    col.gameObject.GetComponent<Character>().updateSpeed();
                    break;
                case 3:
                    col.gameObject.GetComponent<Character>().lifes++;
                    break;
                case 4:
                    col.gameObject.GetComponent<Character>().isImmune = true;
                    col.gameObject.GetComponent<Character>().immunityTimer = 800;
                    break;
                case 5:
                    col.gameObject.GetComponent<Character>().bombs++;
                    break;
                case 6:
                    col.gameObject.GetComponent<Character>().jumpSpeed += 3;
                    col.gameObject.GetComponent<Character>().updateSpeed();
                    break;
                case 7:
                    col.gameObject.GetComponent<Character>().push = true;
                    break;
            }
            Destroy(this.gameObject);
        }
    }
}
