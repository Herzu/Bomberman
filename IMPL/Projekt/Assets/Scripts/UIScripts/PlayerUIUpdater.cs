using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Klasa obslugujaca wyswietlanie informacji o postaci na ekranie
public class PlayerUIUpdater : MonoBehaviour
{
    [SerializeField] private Text lifesText;            //!< obiekt tekstu wyswietlajacego ilosc zyc postaci
    [SerializeField] private Image lifesImageObject;    //!< obiekt zdjecia zyc gracza 
    [SerializeField] private Text bombAmountText;       //!< obiekt tekstu wywietlajacego ilosc bomb postaci
    [SerializeField] private Sprite lifesSprite;        //!< obraz zyc w normalnym stanie
    [SerializeField] private Sprite immuneLifesSprite;  //!< obraz zyc w stanie niewrazliwosci
    public GameObject player;                           //!< obiekt przechowujacy gracza ktorego dane beda wyswietlane

    private void Start() {
        if (player != null) {
            UpdateLifesText();
            UpdateBombAmountText();
        }
    }

    private void Update() {
        if (player != null) {
            UpdateLifesText();
            UpdateBombAmountText();
            UpdateHeartsIconOnImmune();
        }
    }
    /** Funkcja odswiezajaca tekst z iloscia zyc gracza */
    public void UpdateLifesText() {
        lifesText.text = player.GetComponent<Character>().lifes.ToString();
    }

    /** Funkcja odswiezajaca tekst z iloscia bomb gracza */

    public void UpdateBombAmountText() {
        bombAmountText.text = player.GetComponent<Character>().bombs.ToString();
    }

    /** Funkcja odswiezajaca obraz serc w trakcie niewrazliwosci */
    public void UpdateHeartsIconOnImmune() {
        if(player.GetComponent<Character>().isImmune){
            lifesImageObject.sprite = immuneLifesSprite;
        } else {
            lifesImageObject.sprite = lifesSprite;
        }
    }
}
