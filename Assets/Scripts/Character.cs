using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour {
    public int range;
    public int speed;
    public int lifes;
    public bool isImmune = false;
    public int immunityTimer = 800;
    public int bombs;
    public int bombLifetime;
    List<int> bombCooldowns;

    void Start() {
        bombCooldowns = new List<int>();
    }

    void Update() {

    }
    protected void Init()
    {
        bombCooldowns = new List<int>();
    }

    public void checkBomb() {
        for(int i=0;i<bombCooldowns.Count;i++)
        {
            if(--bombCooldowns[i]==0)
            {
                bombs++;
            }
        }
        bombCooldowns.RemoveAll(item => item == 0);
    }
    public void placeBomb()
    {
        this.bombs -= 1;
        bombCooldowns.Add(bombLifetime);
    }

    public bool isAlive() {
        return this.lifes < 1;
    }

    public void handleGameover() {
        if(this.gameObject) {
            Destroy(this.gameObject);
        }
    }

    public void checkImmunity() {
        if(this.isImmune) {
            if(this.immunityTimer == 0) {
                this.isImmune = false;
                this.immunityTimer = 800;
            }
            else
                this.immunityTimer--;
        }
    }
}