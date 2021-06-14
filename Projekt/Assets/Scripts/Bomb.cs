using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za zachowanie bomby
public class Bomb : MonoBehaviour
{
    public int maxLifetime;				//!< maksymalny czas życia bomby
    private int lifetime;				//!< pozostały czas życia bomby
    private float fuseBurn;				//!< wartość przesunięcia lontu pomby
    public int range;					//!< zasięg bomby
    public GameObject xExplosion;		//!< obiekt odpowiedzialny za sprawdzanie postaci w zasięgu w osi X
    public GameObject yExplosion;		//!< obiekt odpowiedzialny za sprawdzanie postaci w zasięgu w osi Y
    public GameObject zExplosion;		//!< obiekt odpowiedzialny za sprawdzanie postaci w zasięgu w osi Z
    public GameObject fuse;				//!< obiekt reprezentujący lont
    public GameObject explosionEffect;	//!< prefab efektu eksplozji
    public GameObject fireEffect;		//!< prefab efektu	płomienia
    public GameObject playerCollider;	//!< obiekt odczytujący kolizję z graczem
    public bool is3D;					//!< czy bomba ma wybuchać także w osi pionowej
    private GameObject[] exp;			//!< tablica efektów eksplozji
    private ParticleSystem[] par;		//!< tablica systemów cząsteczkowych eksplozji
    private GameObject fire;			//!< obiekt efektu płomienia
    // Start is called before the first frame update
    void Start()
    {
		//zainicjalizowanie tablic efektów i systemów cząsteczkowych eksplozji, w 6 kierunkach w trybie 3D i w 4 w trybie 2D
        if (is3D)
        {
            exp = new GameObject[6];
            par = new ParticleSystem[6];
        }
        else
        {
            exp = new GameObject[4];
            par = new ParticleSystem[4];
        }
		//wyliczenie o ile ma się przesuwać lont co klatkę
        // fuseBurn = 0.2f / maxLifetime;
		//ustawienie czasu życia do maksymalnej wartości
        lifetime = maxLifetime;
		//stworzenie efektu ognia
        // fire = Instantiate(fireEffect, this.transform.position+new Vector3(0,0.5f,0), Quaternion.identity);
    }
	/** funkcja tworzoąca efekt eksplozji z odpowiednim kątem bazując na id
	 * @param id identyfikator kierunku efektu
	 * @param angle kąt o który ma zostać obrócony efekt
	 */
    void CreateParticleEffect(int id, float angle)
    {
        if(id<4)	//wybuchy znajdują się poziomo, więc obrót następuje wokół osi y
            exp[id] = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(0, angle, 0));
        else		//wybuchy znajdują się pionowo więc obrót następuje wokół osi x
            exp[id] = Instantiate(explosionEffect, this.transform.position, Quaternion.Euler(angle, 0, 0));
		//przypisanie efektów do odpowiednich systemów cząsteczkowych
        par[id] = exp[id].gameObject.GetComponent<ParticleSystem>();
		//uzyskanie dostępu do elementu main systemu
        var main = par[id].main;
		//zmiana czasu życia cząsteczek w zależności od zasięgu wybuchu
        main.startLifetime = 0.02f * range;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
		//redukcja pozostałego czasu życia bomby
        lifetime--;
		//obniżenie położenia lontu
        // fuse.gameObject.transform.position -= new Vector3(0,fuseBurn,0);
		//obniżenie położenia efektu ognia
        // fire.gameObject.transform.position = fuse.gameObject.transform.position+new Vector3(0,0.3f,0);
		//stworzenie efektów wybuchu
        if(lifetime==0)
        {
			//zatrzymanie bomby w miejscu
            this.gameObject.transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            //stworzenie odpowiednich systemów cząsteczkowych
			CreateParticleEffect(0, 0);
            CreateParticleEffect(1, 90);
            CreateParticleEffect(2, 180);
            CreateParticleEffect(3, 270);
            if (is3D)
            {
                CreateParticleEffect(4, 90);
                CreateParticleEffect(5, 270);
            }
			//uruchomienie systemów wybuchów
            foreach(ParticleSystem ps in par)
            {
                ps.Play();
            }
        }
		//czekanie aż obiekty wybuchów wykonają swoją pracę 
		else if (lifetime < -30
            && xExplosion == null
            && yExplosion == null
            && zExplosion == null)
        {
			//destrukcja efektów cząsteczkowyc
            for (int i = 0; i < exp.Length; i++)
                Destroy(exp[i].gameObject);
			//destrukcja efektu ognia
            // Destroy(fire.gameObject);
			//destrukcja bomby
            Destroy(this.gameObject);
        }
    }
}
