using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion: MonoBehaviour
{
    int lifetime;
    List<GameObject> objectCollisions;
    List<Character> characterCollisions;
    // Start is called before the first frame update
    void Start()
    {
        objectCollisions = new List<GameObject>();
        characterCollisions = new List<Character>();
        lifetime = this.gameObject.transform.parent.gameObject.GetComponent<Bomb>().maxLifetime;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Block") || col.gameObject.CompareTag("Powerup")) {
            objectCollisions.Add(col.gameObject);
        }
        if(col.gameObject.CompareTag("Player")) {
            characterCollisions.Add(col.gameObject.GetComponent(typeof(Character)) as Character);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Block") || col.gameObject.CompareTag("Powerup")) {
            objectCollisions.Remove(col.gameObject);
        }
        if(col.gameObject.CompareTag("Player")) {
            characterCollisions.Remove(col.gameObject.GetComponent(typeof(Character)) as Character);
        }
    }
        // Update is called once per frame
        void Update()
    {
        lifetime--;
        if(lifetime == 0)
        {
            foreach (GameObject gObject in objectCollisions) {
                Destroy(gObject.gameObject);
            }
            foreach(Character character in characterCollisions) {
                if(!character.isImmune) {
                    character.lifes--;
                }
            }
            Destroy(this.gameObject);
        }
    }
}
