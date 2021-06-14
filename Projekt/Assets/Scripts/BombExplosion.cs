using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za zadawanie obrażeń przy eksplozji
public class BombExplosion: MonoBehaviour
{
    public int lifetime;                    //!< pozostały czas życia do wybuchu
    List<GameObject> objectCollisions;      //!< lista kolizji z obiektami
    List<Character> characterCollisions;    //!< lista kolizji z postaciami
    List<GameObject> walls;                 //!< lista kolizji ze ścianami
    public int type; //1=x, 2=y, 3=z
    float lowerWallLimit = float.MinValue, upperWallLimit = float.MaxValue;
    // Start is called before the first frame update
    void Awake()
    {
        //inicjalizacja list
        objectCollisions = new List<GameObject>();
        characterCollisions = new List<Character>();
        walls = new List<GameObject>();
        //pobranie czasu życia
        lifetime = this.gameObject.transform.parent.gameObject.GetComponent<Bomb>().maxLifetime;
    }

    void OnTriggerEnter(Collider col)
    {
        //dodanie elementu do odpowiedniej listy po wykryciu wejścia w zasięg ekslozji
        if (col.gameObject.CompareTag("Block") || col.gameObject.CompareTag("Powerup"))
            objectCollisions.Add(col.gameObject);
        else if (col.gameObject.CompareTag("Wall"))
            walls.Add(col.gameObject);
        else if(col.gameObject.CompareTag("Player")|| col.gameObject.CompareTag("Bot"))
            characterCollisions.Add(col.gameObject.GetComponent(typeof(Character)) as Character);
    }

    void OnTriggerExit(Collider col)
    {
        //usunięcie elementu z odpowiedniej listy po wykryciu wyjścia z zasięgu ekslozji
        if (col.gameObject.CompareTag("Block") || col.gameObject.CompareTag("Powerup"))
            objectCollisions.Remove(col.gameObject);
        else if (col.gameObject.CompareTag("Wall"))
            walls.Remove(col.gameObject);
        else if(col.gameObject.CompareTag("Player")||col.gameObject.CompareTag("Bot"))
            characterCollisions.Remove(col.gameObject.GetComponent(typeof(Character)) as Character);
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        //redukcja czasu żcia
        lifetime--;
        //jeżeli bomba powinna wybuchnąć
        if(lifetime == 0)
        {
            //obliczenie limitu zadawania obrażeń (żeby nie zadawać obrażeń przez niezniszczalne ściany)
            foreach(GameObject wall in walls)
            {
                if(wall != null) {
                    //uzyskanie pozycji relatywnej do bomby
                    float relativePos = GetRelativePos(wall.transform.position);
                    if (relativePos < 0)    //jeżeli ściana znajduje się po lewej
                        lowerWallLimit = Mathf.Max(relativePos, lowerWallLimit);
                    else                    //jeżeli ściana znajduje się po prawej
                        upperWallLimit = Mathf.Min(relativePos, upperWallLimit);
                }
            }
            //destrukcja napotkanych obiektów (zniszczalnych ścian i powerupów)
            foreach (GameObject gObject in objectCollisions)
            {
                if (gObject != null)
                {
                    //uzyskanie pozycji relatywnej do bomby
                    float relativePos = GetRelativePos(gObject.transform.position);
                    //sprawdzenie czy obiekt nie znajduje się za ścianą
                    if (relativePos < upperWallLimit && relativePos > lowerWallLimit)
                        Destroy(gObject.gameObject);
                }
            }
        }
        else if (lifetime < 0)
        {
            //zadawanie obrażeń graczom przez cały czas trwania wybuchu (ze względu na nieśmiertelność po trafieniu nie można zostać zranionym przez jedną bombę dwa razy)
            foreach (Character character in characterCollisions)
            {
                if (character != null)
                {
                    //uzyskanie pozycji relatywnej do bomby
                    float relativePos = GetRelativePos(character.transform.position);
                    //sprawdzenie czy postać nie znajduje się za ścianą
                    if (relativePos < upperWallLimit && relativePos > lowerWallLimit)
                        if (!character.isImmune)            //sprawdzenie czy gracz nie jest odporny na obrażenia
                        {
                            character.lifes--;              //odebranie życia
                            character.isImmune = true;      //włączenie nieśmiertelności po trafieniu
                            character.immunityTimer = 35;   //ustalenie czasu nieśmiertelności
                        }
                }
            }
            if (lifetime == -30)            //zakończenie efektu eksplozji
                Destroy(this.gameObject);   //destrukcja obiektu eksplozji
        }
    }
    /** funkcja obliczająca pozycję relatywną do bomby
     * @param position pozycja sprawdzanego obiektu
     */
    private float GetRelativePos(Vector3 position)
    {
        //w zależności od ustalonego typu relatywna pozycja obliczana jest dla innej osi
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
