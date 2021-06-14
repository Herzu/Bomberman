using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za zarz�dzanie postaci�
public class Character: MonoBehaviour {
    public int range;               //!< zasi�g wybuchu
    public int speed;               //!< szybko�� poruszania
    public int jumpSpeed = 10;      //!< szybko�� skoku
    public int lifes;               //!< �ycia
    public bool isImmune = false;   //!< czy gracz jest nie�miertelny
    public int immunityTimer = 800; //!< pozosta�y czas nie�miertelno�ci
    public bool push = false;       //!< czy gracz mo�e popycha� bomb�
    public int bombs;               //!< ilo�� bomb
    public int bombLifetime;        //!< bazowy czas �ycia bomb
    public int mapHeight;           //!< wysoko�� mapy
    public GameObject bombPrefab;   //<! prefab bomby
    List<int> bombCooldowns;        //!< lista czas�w odnowienia poszczeg�lnych bomb
    int bombCooldown = 0;

    void Start() {
        //inicjalizacja list bomb
        bombCooldowns = new List<int>();
    }

    void FixedUpdate() {
        //aktualizacja listy bomb
        checkBomb();
        //aktualizacja nie�miertelno�ci
        checkImmunity();
    }
    /**  funkcja wywo�ywana z klasy rodzicielskiej przy inicjalizacji*/
    protected void Init()
    {
        bombCooldowns = new List<int>();
        jumpSpeed = 10;
    }
    /** funkcja umieszczaj�ca posta� w odpowiednim miejscu mapy*/
    public void moveToPlace()
    {
        gameObject.transform.position = new Vector3(transform.position.x, 2 * mapHeight + 1, transform.position.z);
    }
    /** funkcja zapisuj�ca szybko�� postaci do kontrolera postaci*/
    public void updateSpeed()
    {
        //je�eli posta� jest pierwszosobowa, szybko�� oraz szybko�� skoku s� przekazywane do kontrollera
        if (this.gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>() != null)
        {
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_RunSpeed = speed;
            gameObject.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_JumpSpeed = jumpSpeed;
        }
        //dla ka�dego typu kontrolera szybko�� przypisywna jest w odpowiednie miejsce
        if (this.gameObject.GetComponent<ArrowsController>() != null)
            gameObject.GetComponent<ArrowsController>().speed = speed / 2;
        if (this.gameObject.GetComponent<WASDController>() != null)
            gameObject.GetComponent<WASDController>().speed = speed / 2;
        if (this.gameObject.GetComponent<GamepadController>() != null)
            gameObject.GetComponent<GamepadController>().speed = speed / 2;
        //dla ka�dego typu modelu szybko�� animacji przekazywana jest do animatora
        if (this.gameObject.GetComponent<Animator>() != null)
            gameObject.GetComponent<Animator>().speed = speed / 10;
        if (this.gameObject.transform.GetChild(0).GetComponent<Animator>() != null)
            gameObject.transform.GetChild(0).GetComponent<Animator>().speed = speed / 10.0f;
    }
    /** funkcja sprawdzaj�ca czy kt�ra� bomba si� nie odnowi�a oraz redukuje czas odnowienia ka�dej bomby*/
    public void checkBomb() {
        for(int i=0;i<bombCooldowns.Count;i++)
        {
            //je�eli czas odnowienia osi�gnie zero gracz odzyskuje bomb�
            if(--bombCooldowns[i]==0)
            {
                bombs++;
            }
        }
        //usuni�cie bomb kt�re zosta�y ju� zwr�cone
        bombCooldowns.RemoveAll(item => item == 0);
    }
    /** funkcja wywo�ywana przy postawieniu bomby*/
    public void placeBomb()
    {
        if (this.bombs > 0) {
            //redukcja ilo�ci bomb
            this.bombs -= 1;
            //dodanie bomby do listy czas�w odnowienia
            bombCooldowns.Add(bombLifetime);

            //pobranie zasięgu i czasu życia bomby ze skryptu postaci
            // Vector3Int intVector = new Vector3Int((int)this.transform.position.x, (int)this.transform.position.y, (int)this.transform.position.z);
            Vector3Int intVector = Vector3Int.FloorToInt(this.transform.position);
            //obliczenie wektora pozycji stawiającego bombę na środku pola
            Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 - (intVector.z) % 2);
            //stworzenie bomby
            GameObject bomb = Instantiate(
                bombPrefab,
                bombPlacement,
                Quaternion.identity,
                this.gameObject.transform.parent
            );
            //przekazanie czasu życia
            bomb.GetComponent<Bomb>().maxLifetime = bombLifetime;
            //przekazanie zasięgu i czasów życia do obiektów odpowiadających za zadawanie obrażeń
            bomb.transform.GetChild(1).GetComponent<BoxCollider>().size = new Vector3(4 * range, 1, 1);
            bomb.transform.GetChild(1).GetComponent<BombExplosion>().lifetime = bombLifetime;
            bomb.transform.GetChild(2).GetComponent<BoxCollider>().size = new Vector3(1, 4 * range, 1);
            bomb.transform.GetChild(2).GetComponent<BombExplosion>().lifetime = bombLifetime;
            bomb.transform.GetChild(3).GetComponent<BoxCollider>().size = new Vector3(1, 1, 4 * range);
            bomb.transform.GetChild(3).GetComponent<BombExplosion>().lifetime = bombLifetime;
            //przekazanie wartości trójwymiarowości do bomby
            bomb.GetComponent<Bomb>().is3D = false;
            //przekazanie zasięgu do bomby
            bomb.GetComponent<Bomb>().range = range;  
        } 
    }
    /** funkcja sprawdzaj�ca czy gracz jest martwy (czy nie ma �y� lub wypad� poza map�)
     * @return informacja czy gracz jest martwy
     */
    public bool isDead() {
        return this.lifes < 1 || this.gameObject.transform.position.y < 0;
    }
    /** funckja obs�uguj�ca przegran� postaci*/
    public void handleGameover() {
        if(this.gameObject) {
            Destroy(this.gameObject);
        }
    }
    /** funkcja redukuj�ca czas nie�miertelno�ci*/
    public void checkImmunity() {
        if(this.isImmune) {                 //je�eli gracz jest nie�miertelny
            if(this.immunityTimer == 0) {   //je�eli nie�miertelno�� si� skonczy�a
                //nie�miertelno�� jest wy��czana
                this.isImmune = false;
                //licznik nie�miertelno�ci jest resetowany
                this.immunityTimer = 800;
            }
            else
                //je�eli nie�miertelno�� si� nie sko�czy�a czas nie�miertelno�ci jest redukowany
                this.immunityTimer--;
        }
    }
}