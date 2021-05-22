using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Klasa sluzaca do wyswietalania wartosci slidera za pomoca tesktu na ekranie
public class SliderTextUpdater : MonoBehaviour
{
    public Text sliderText;     //!< obiekt tekstu wyswietlajacy wartosc slidera
    public Slider sliderObject; //!< obiekt slidera ktorego wartosc bedzie wyswietlana

    public void Start() {
        sliderText.text = sliderObject.value.ToString();
    }

	/** Funkcja ustawiajaca wartosc slidera w wyswietlanym tekscie
	 * @param value wartosc slidera 
	 */
    public void UpdateText(float value) {
        sliderText.text = value.ToString();
    } 
}
