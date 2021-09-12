using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za zarz¹dzanie postaci¹
public class Character: MonoBehaviour {
    public int range;               //!< zasiêg wybuchu
    public int speed;               //!< szybkoœæ poruszania
    public int jumpSpeed = 10;      //!< szybkoœæ skoku
    public int lifes;               //!< ¿ycia
    public bool isImmune = false;   //!< czy gracz jest nieœmiertelny
    public int immunityTimer = 800; //!< pozosta³y czas nieœmiertelnoœci
    public bool push = false;       //!< czy gracz mo¿e popychaæ bombê
    public int bombs;               //!< iloœæ bomb
    public int bombLifetime;        //!< bazowy czas ¿ycia bomb
    public int mapHeight;           //!< wysokoœæ mapy
    List<int> bombCooldowns;        //!< lista czasów odnowienia poszczególnych bomb

    void Start() {
        //inicjalizacja list bomb
        bombCooldowns = new List<int>();
    }

    void FixedUpdate() {
        //aktualizacja listy bomb
        checkBomb();
        //aktualizacja nieœmiertelnoœci
        checkImmunity();
    }
    /**  funkcja wywo³ywana z klasy rodzicielskiej przy inicjalizacji*/
    protected void Init()
    {
        bombCooldowns = new List<int>();
        jumpSpeed = 10;
    }
    /** funkcja umieszczaj¹ca postaæ w odpowiednim miejscu mapy*/
    public void moveToPlace()
    {
        gameObject.transform.position = new Vector3(transform.position.x, 2 * mapHeight + 1, transform.position.z);
    }
    /** funkcja zapisuj¹ca szybkoœæ postaci do kontrolera postaci*/
    public void updateSpeed()
    {
        //je¿eli postaæ jest pierwszosobowa, szybkoœæ oraz szybkoœæ skoku s¹ przekazywane do kontrollera
        if (this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>() != null)
        {
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = speed;
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = jumpSpeed;
        }
        //dla ka¿dego typu kontrolera szybkoœæ przypisywna jest w odpowiednie miejsce
        if (this.gameObject.GetComponent<ArrowsController>() != null)
            gameObject.GetComponent<ArrowsController>().speed = speed / 2;
        if (this.gameObject.GetComponent<WASDController>() != null)
            gameObject.GetComponent<WASDController>().speed = speed / 2;
        if (this.gameObject.GetComponent<GamepadController>() != null)
            gameObject.GetComponent<GamepadController>().speed = speed / 2;
        //dla ka¿dego typu modelu szybkoœæ animacji przekazywana jest do animatora
        if (this.gameObject.GetComponent<Animator>() != null)
            gameObject.GetComponent<Animator>().speed = speed / 10;
        if (this.gameObject.transform.GetChild(0).GetComponent<Animator>() != null)
            gameObject.transform.GetChild(0).GetComponent<Animator>().speed = speed / 10.0f;
    }
    /** funkcja sprawdzaj¹ca czy któraœ bomba siê nie odnowi³a oraz redukuje czas odnowienia ka¿dej bomby*/
    public void checkBomb() {
        for(int i=0;i<bombCooldowns.Count;i++)
        {
            //je¿eli czas odnowienia osi¹gnie zero gracz odzyskuje bombê
            if(--bombCooldowns[i]==0)
            {
                bombs++;
            }
        }
        //usuniêcie bomb które zosta³y ju¿ zwrócone
        bombCooldowns.RemoveAll(item => item == 0);
    }
    /** funkcja wywo³ywana przy postawieniu bomby*/
    public void placeBomb()
    {
        //redukcja iloœci bomb
        this.bombs -= 1;
        //dodanie bomby do listy czasów odnowienia
        bombCooldowns.Add(bombLifetime);
    }
    /** funkcja sprawdzaj¹ca czy gracz jest martwy (czy nie ma ¿yæ lub wypad³ poza mapê)
     * @return informacja czy gracz jest martwy
     */
    public bool isDead() {
        return this.lifes < 1 || this.gameObject.transform.position.y < 0;
    }
    /** funckja obs³uguj¹ca przegran¹ postaci*/
    public void handleGameover() {
        if(this.gameObject) {
            Destroy(this.gameObject);
        }
    }
    /** funkcja redukuj¹ca czas nieœmiertelnoœci*/
    public void checkImmunity() {
        if(this.isImmune) {                 //je¿eli gracz jest nieœmiertelny
            if(this.immunityTimer == 0) {   //je¿eli nieœmiertelnoœæ siê skonczy³a
                //nieœmiertelnoœæ jest wy³¹czana
                this.isImmune = false;
                //licznik nieœmiertelnoœci jest resetowany
                this.immunityTimer = 800;
            }
            else
                //je¿eli nieœmiertelnoœæ siê nie skoñczy³a czas nieœmiertelnoœci jest redukowany
                this.immunityTimer--;
        }
    }
}