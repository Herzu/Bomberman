using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Player : Character
{
    public GameObject bombPrefab;
    int cooldown = 0;
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    void FixedUpdate()
    {
        Camera.main.transform.position = transform.position + new Vector3(0, 9, 0);
        if (Input.GetKeyDown(KeyCode.LeftAlt) && cooldown == 0 && bombs > 0)
        {
            PutBomb();
            cooldown = 10;
            placeBomb();
        }
        isAlive();
        checkBomb();
        checkImmunity();
        if(cooldown!=0)
            cooldown--;
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
        bomb.GetComponent<Bomb>().is3D = false;
        bomb.GetComponent<Bomb>().range = range;
    }
}
