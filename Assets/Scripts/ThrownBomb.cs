using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za możliwość rzucenia bombą w trybie pierwszoosobowym
public class ThrownBomb : MonoBehaviour
{
    public bool is3D;               //!< czy bomba ma wybuchać także w osi pionowej
    public GameObject bombPrefab;   //!< prefab bomby
    public int bombLifetime;        //!< maksymalny czas życia bomby
    public int range;               //!< zasięg bomby
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
        //przy kolizji z terenem postaw bombę i zniszcz ten obiekt
        PutBomb();
        Destroy(this.gameObject);
    }
    /** Funkcja tworząca bombę*/
    private void PutBomb()
    {
        //obliczenie bazowego wektora (z float na int)
        Vector3Int intVector = new Vector3Int((int)transform.position.x, (int)transform.position.y, (int)transform.position.z);
        //obliczenie wektora pozycji stawiającego bombę na środku pola
        Vector3 bombPlacement = intVector + new Vector3(1 - (intVector.x) % 2, 1, 1 - (intVector.z) % 2);
        //stworzenie bomby
        GameObject bomb = Instantiate(bombPrefab, bombPlacement, Quaternion.identity);
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
        bomb.GetComponent<Bomb>().is3D = is3D;
        //przekazanie zasięgu do bomby
        bomb.GetComponent<Bomb>().range = range;
    }
}
