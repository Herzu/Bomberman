using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Klasa odpowiedzialna za komponent wyboru rodzaju sterowania gracza
public class PlayerSelector : MonoBehaviour
{
    public Color color;         //!< kolor zaznaczenia
    public Color disabledColor; //!< kolor wylaczony
    public GameObject frame;    //!< obiekt przechowujacy ramke

	/** Funkcja obslugujaca toggle
	 * @param value nowa wartosc toggle'a
	 */
    public void handleToggleUI(bool value) {
        Image backgroundFrame = frame.GetComponent<Image>();
        if(value) {
            backgroundFrame.color = color;
        } else {
            backgroundFrame.color = disabledColor;
        }
    }
}
