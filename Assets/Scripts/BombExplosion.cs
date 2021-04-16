using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion: MonoBehaviour
{
    int lifetime;
    List<GameObject> currentCollisions;
    // Start is called before the first frame update
    void Start()
    {
        currentCollisions = new List<GameObject>();
        lifetime = this.gameObject.transform.parent.gameObject.GetComponent<Bomb>().maxLifetime;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Block")
            currentCollisions.Add(col.gameObject);
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Block")
            currentCollisions.Remove(col.gameObject);
    }
        // Update is called once per frame
        void Update()
    {
        lifetime--;
        if(lifetime == 0)
        {
            foreach (GameObject gObject in currentCollisions)
            {
                Destroy(gObject.gameObject);
            }
            Destroy(this.gameObject);
        }
    }
}
