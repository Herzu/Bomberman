using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBomb : MonoBehaviour
{
    private int cooldown = 30;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (cooldown < 0 && collision.gameObject.GetComponentInParent<Character>()!=null&& collision.gameObject.GetComponentInParent<Character>().push)
        {
            if(Mathf.Abs(this.transform.position.x-collision.gameObject.transform.position.x)> Mathf.Abs(this.transform.position.z - collision.gameObject.transform.position.z))
                gameObject.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
            else
                gameObject.transform.parent.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionX;
            int xSpeed, zSpeed;
            if (this.transform.position.x > collision.gameObject.transform.position.x)
                xSpeed = 10;
            else
                xSpeed = -10;
            if (this.transform.position.z > collision.gameObject.transform.position.z)
                zSpeed = 10;
            else
                zSpeed = -10;
            gameObject.transform.parent.GetComponent<Rigidbody>().velocity = new Vector3(xSpeed, 0, zSpeed);
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        cooldown--;
    }
}
