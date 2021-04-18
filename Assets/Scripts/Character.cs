using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    int lifes = 3;
    int bombs = 5;
    int bombCooldown = 300;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void getHit() {
        this.lifes--;
    }

    public void checkBomb() {
        if(bombCooldown == 0) {
            this.bombs++;
            bombCooldown = 300;
        }
        bombCooldown--;
    }

    public void isAlive() {
        if(lifes == 0) {
            Destroy(this.gameObject);
        }
    }

    public int getBombs() {
        return this.bombs;
    }

    public void setBombs(int b) {
        this.bombs = b;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
