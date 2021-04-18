using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    int lifetime = 100;
    List<GameObject> blockCollisions;
    List<Character> characterCollisions;
    // Start is called before the first frame update
    void Start()
    {
        blockCollisions = new List<GameObject>();
        characterCollisions = new List<Character>();
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Block") {
            blockCollisions.Add(col.gameObject);
        }
        if(col.gameObject.tag == "Character") {
            characterCollisions.Add(col.gameObject.GetComponent(typeof(Character)) as Character);
        }
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Block") {
            blockCollisions.Remove(col.gameObject);
        }
        if(col.gameObject.tag == "Character") {
            characterCollisions.Remove(col.gameObject.GetComponent(typeof(Character)) as Character);
        }
    }
        // Update is called once per frame
    void Update()
    {
        lifetime--;
        if(lifetime == 0)
        {
            foreach (GameObject gObject in blockCollisions)
            {
                Destroy(gObject.gameObject);
            }
            foreach (Character character in characterCollisions) {
                character.getHit();
            }
            Destroy(this.gameObject.transform.parent.gameObject);
        }
    }
}
