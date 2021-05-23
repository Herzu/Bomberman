using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za możliwość popchnięcia bomby
public class PushBomb : MonoBehaviour
{
    private int cooldown = 30;  //!< tymczasowe wyłączenie efektu aby uniknąć natychmiastowego popchnięcia
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //sprawdzenie kolizji z graczem oraz czy gracz odblokował możliwość popchnięcia bomby
        if (cooldown < 0 && collision.gameObject.GetComponentInParent<Character>()!=null&& collision.gameObject.GetComponentInParent<Character>().push)
        {
            //sprawdzenie osi w której nastąpiła kolizja
            if(Mathf.Abs(this.transform.position.x-collision.gameObject.transform.position.x)> Mathf.Abs(this.transform.position.z - collision.gameObject.transform.position.z))
                //jeżeli w osi X włącz możliwość ruchu w osi X
                gameObject.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
            else
                //jeżeli w osi Y włącz możliwość ruchu w osi Y
                gameObject.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
            int xSpeed, zSpeed; //prędkości w osi X i Y
            //obliczenie prędkości w zależności od tego z której strony znajduje się gracz
            if (this.transform.position.x > collision.gameObject.transform.position.x)
                xSpeed = 10;
            else
                xSpeed = -10;
            if (this.transform.position.z > collision.gameObject.transform.position.z)
                zSpeed = 10;
            else
                zSpeed = -10;
            //nadanie bombie prędkości
            gameObject.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(xSpeed, 0, zSpeed);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //redukcja czasu do aktywacji
        cooldown--;
    }
}
