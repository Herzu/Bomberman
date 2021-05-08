using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrownBomb : MonoBehaviour
{
    public bool is3D;
    public GameObject bombPrefab;
    public int bombLifetime;
    public int range;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        PutBomb();
        Destroy(this.gameObject);
    }
    private void PutBomb()
    {
        Vector3Int intVector = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 - (intVector.z) % 2);
        GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
        bomb.GetComponent<Bomb>().maxLifetime = bombLifetime;
        bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * range, 1, 1);
        bomb.transform.GetChild(1).GetComponent<BombExplosion>().lifetime = bombLifetime;
        bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * range, 1);
        bomb.transform.GetChild(2).GetComponent<BombExplosion>().lifetime = bombLifetime;
        bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * range);
        bomb.transform.GetChild(3).GetComponent<BombExplosion>().lifetime = bombLifetime;
        if (is3D)
            bomb.GetComponent<Bomb>().is3D = true;
        else
            bomb.GetComponent<Bomb>().is3D = false;
        bomb.GetComponent<Bomb>().range = range;
    }
}
