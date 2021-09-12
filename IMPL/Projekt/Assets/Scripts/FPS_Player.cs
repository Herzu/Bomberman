using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//! Klasa odpowiedzialna za kontrolę gracza pierwszoosobowego
public class FPS_Player : Character
{
    public GameObject bombPrefab;           //!< prefab bomby
    public GameObject thrownBombPrefab;     //!< prefab rzuconej bomby
    int bombSpeed = 10;                     //!< prędkość rzutu bomby
    int cooldown = 0;                       //!< czas odnowienia między rzutami bombami
    // Start is called before the first frame update
    void Start()
    {
        //inicjalizacja postaci
        Init();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //jeżeli gracz nacisnął lewy przycisk myszy i może postawić bombę
        if (Input.GetMouseButtonDown(0) && this.cooldown == 0 && this.bombs > 0)
        {
            //bomba jest inicjalizowana na pozycji gracza i odpowiednie zmienne są przekazywane
            GameObject bomb = Instantiate(thrownBombPrefab, gameObject.transform.position+new Vector3(0,0.3f,0), Quaternion.identity);
            bomb.GetComponent<Rigidbody>().velocity = Camera.main.gameObject.transform.forward * bombSpeed;
            bomb.GetComponent<ThrownBomb>().bombPrefab = bombPrefab;
            bomb.GetComponent<ThrownBomb>().bombLifetime = bombLifetime;
            bomb.GetComponent<ThrownBomb>().range = range;
            bomb.GetComponent<ThrownBomb>().is3D = true;
            //ustawienie czasu odnowienia między rzutami
            this.cooldown = 10;
            //zasygnalizowanie użycia bomby
            this.placeBomb();
        }
        //sprwadzenie czasów odnowienia bomb
        this.checkBomb();
        //sprawdzenie czasu nieśmiertelności
        this.checkImmunity();
        //redukcja czasu odnowienia między rzutami jeżeli nie jest równy zero
        if (this.cooldown != 0)
            this.cooldown--;
    }
}
