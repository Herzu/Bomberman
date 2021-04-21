﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion: MonoBehaviour
{
    int lifetime;
    List<GameObject> currentCollisions;
    List<GameObject> walls;
    public int type; //1=x, 2=y, 3=z
    float lowerWallLimit = float.MinValue, upperWallLimit = float.MaxValue;
    // Start is called before the first frame update
    void Start()
    {
        currentCollisions = new List<GameObject>();
        walls = new List<GameObject>();
        lifetime = this.gameObject.transform.parent.gameObject.GetComponent<Bomb>().maxLifetime;
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Block") || col.gameObject.CompareTag("Powerup"))
            currentCollisions.Add(col.gameObject);
        else if (col.gameObject.CompareTag("Wall"))
            walls.Add(col.gameObject);
    }
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Block") || col.gameObject.CompareTag("Powerup"))
            currentCollisions.Remove(col.gameObject);
        else if (col.gameObject.CompareTag("Wall"))
            walls.Remove(col.gameObject);
    }
        // Update is called once per frame
    void Update()
    {
        lifetime--;
        if(lifetime == 0)
        {
            foreach(GameObject wall in walls)
            {
                float relativePos = GetRelativePos(wall.transform.position);
                if (relativePos < 0)
                    lowerWallLimit = Mathf.Max(relativePos, lowerWallLimit);
                else
                    upperWallLimit = Mathf.Min(relativePos, upperWallLimit);
            }
            foreach (GameObject gObject in currentCollisions)
            {
                if (gObject != null)
                {
                    float relativePos = GetRelativePos(gObject.transform.position);
                    if (relativePos < upperWallLimit && relativePos > lowerWallLimit)
                        Destroy(gObject.gameObject);
                }
            }
            Destroy(this.gameObject);
        }
    }
    float GetRelativePos(Vector3 position)
    {
        switch (type)
        {
            case 1:
                return position.x - this.transform.position.x;
            case 2:
                return position.y - this.transform.position.y;
            case 3:
                return position.z - this.transform.position.z;
            default:
                return 0.0f;
        }
    }
}
