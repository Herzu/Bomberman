using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character: MonoBehaviour {
    public int range;
    public int speed;
    public int jumpSpeed = 10;
    public int lifes;
    public bool isImmune = false;
    public int immunityTimer = 800;
    public bool push = false;
    public int bombs;
    public int bombLifetime;
    public int mapHeight;
    List<int> bombCooldowns;

    void Start() {
        bombCooldowns = new List<int>();
    }

    void Update() {

    }
    protected void Init()
    {
        bombCooldowns = new List<int>();
        jumpSpeed = 10;
    }
    public void moveToPlace()
    {
        gameObject.transform.position = new Vector3(transform.position.x, 2 * mapHeight + 1, transform.position.z);
    }
    public void updateSpeed()
    {
        if (this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>() != null)
        {
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = speed;
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = jumpSpeed;
        }
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