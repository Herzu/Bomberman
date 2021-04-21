using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour {
    public int range;
    public int speed = 10;
    public int lifes = 3;
    public bool isImmune = false;
    public int immunityTimer = 800;
    public int bombs = 3;
    int bombCooldown = 800;
    public int points = 0;

    void Start() {

    }

    void Update() {

    }

    public void checkBomb() {
        if(this.bombCooldown == 0) {
            this.bombs++;
            this.bombCooldown = 800;
        }
        this.bombCooldown--;
    }

    public void isAlive() {
        if(this.lifes < 1) {
            Destroy(this.gameObject);
        }
    }

    public void checkImmunity() {
        if(this.isImmune) {
            if(this.immunityTimer == 0) {
                this.isImmune = false;
                this.immunityTimer = 800;
            }
            this.immunityTimer--;
        }
    }
}