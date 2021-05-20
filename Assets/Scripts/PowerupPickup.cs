using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickup : MonoBehaviour
{
    public int powerupID;   //id powerupu oznaczające typ powerupu
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //efekt obracania się powerupu
        this.gameObject.transform.Rotate(0.0f, 1.0f, 0.0f,Space.Self);
    }
    private void OnTriggerEnter(Collider col)
    {
        //podniesienie powerupu przez gracza lub bota
        if (col.gameObject.CompareTag("Player")|| col.gameObject.CompareTag("Bot"))
        {
            switch (powerupID)
            {
                case 1:
                    //zwiększenie zasiegu
                    col.gameObject.GetComponent<Character>().range++;
                    break;
                case 2:
                    //zwiększenie prędkości
                    col.gameObject.GetComponent<Character>().speed += 5;
                    //przekazanie prędkości do kontrollera
                    col.gameObject.GetComponent<Character>().updateSpeed();
                    break;
                case 3:
                    //zwiększenie liczby żyć
                    col.gameObject.GetComponent<Character>().lifes++;
                    break;
                case 4:
                    //włączenie nieśmiertelności
                    col.gameObject.GetComponent<Character>().isImmune = true;
                    //ustawienie czasu nieśmiertelności
                    col.gameObject.GetComponent<Character>().immunityTimer = 800;
                    break;
                case 5:
                    //zwiększenie liczby bomb
                    col.gameObject.GetComponent<Character>().bombs++;
                    break;
                case 6:
                    //zwiększenie zasięgu skoku
                    col.gameObject.GetComponent<Character>().jumpSpeed += 3;
                    col.gameObject.GetComponent<Character>().updateSpeed();
                    break;
                case 7:
                    //włączenie możliwości przesuwania bomb
                    col.gameObject.GetComponent<Character>().push = true;
                    break;
            }
            //zniszczenie powerupu
            Destroy(this.gameObject);
        }
    }
}
