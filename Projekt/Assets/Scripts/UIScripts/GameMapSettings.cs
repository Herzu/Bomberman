using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Klasa obslugujaca ekran dostosowywania mapy w Menu
public class GameMapSettings : MonoBehaviour
{
    public GameObject mapVisualization;     //!< obiekt z wizualizacja mapy
    public GameObject playerVisualization;  //!< obiekt z wizualizacja gracza
    private int xSize;                      //!< rozmiar X mapy
    private int ySize;                      //!< rozmiar Y mapy
    private int zSize;                      //!< rozmiar Z mapy
    private int bombsAmount;                //!< ilosc bomb jaka gracz bedzie posiadal w momencie rozpoczecia gry
    private int lifesAmount;                //!< ilosc zyc jaka gracz bedzie posiadal w momencie rozpoczecia gry
    private int powerupChanceAmount;        //!< szansa na wypadniecie powerupu podczas wybuchu skrzynki

    public Slider xSlider;                  //!< obiekt slidera obslugujacego rozmiar X mapy
    public Slider ySlider;                  //!< obiekt slidera obslugujacego rozmiar Y mapy
    public Slider zSlider;                  //!< obiekt slidera obslugujacego rozmiar Z mapy
    public Slider bombsSlider;              //!< obiekt slidera obslugujacego startowa ilosc bomb gracza
    public Slider lifesSlider;              //!< obiekt slidera obslugujacego startowa ilosc zyc gracza
    public Slider powerupsSlider;           //!< obiekt slidera obslugujacego szanse na powerup

    public void OnEnable() {
        // pobranie wartosci zapisanych w PlayerPrefs
        xSize = PlayerPrefs.GetInt("xSize");
        ySize = PlayerPrefs.GetInt("ySize");
        zSize = PlayerPrefs.GetInt("zSize");
        bombsAmount = PlayerPrefs.GetInt("bombsAmount");
        lifesAmount = PlayerPrefs.GetInt("lifesAmount");
        powerupChanceAmount = PlayerPrefs.GetInt("powerupChanceAmount");
    }

    public void Start() {
        // ustawienie startowych wartosci sliderow
        xSlider.value = xSize;
        ySlider.value = ySize;
        zSlider.value = zSize;
        bombsSlider.value = bombsAmount;
        lifesSlider.value = lifesAmount;
        powerupsSlider.value = powerupChanceAmount;
        UpdateVisualization();
    }

    public void OnDisable() {
        // zapiasnie wartosci w PlayerPrefs
        PlayerPrefs.SetInt("xSize", xSize);
        PlayerPrefs.SetInt("ySize", ySize);
        PlayerPrefs.SetInt("zSize", zSize);
        PlayerPrefs.SetInt("bombsAmount", bombsAmount);
        PlayerPrefs.SetInt("lifesAmount", lifesAmount);
        PlayerPrefs.SetInt("powerupChanceAmount", powerupChanceAmount);
    }


    private void UpdateVisualization() {
        int multiplier = 25;
        mapVisualization.transform.localScale = new Vector3(
            (xSize * multiplier),
            (ySize * multiplier),
            (zSize * multiplier)
        );
    }

	/** Funkcja handler dla slidera rozmiaru X mapy
	 * @param value wartosc slidera
	 */
    public void ChangeXSize(float value) {
        xSize = (int)value;
        UpdateVisualization();
    }

	/** Funkcja handler dla slidera rozmiaru Y mapy
	 * @param value wartosc slidera
	 */
    public void ChangeYSize(float value) {
        ySize = (int)value;
        UpdateVisualization();
    }

	/** Funkcja handler dla slidera rozmiaru Z mapy
	 * @param value wartosc slidera
	 */
    public void ChangeZSize(float value) {
        zSize = (int)value;
        UpdateVisualization();
    }

	/** Funkcja handler dla slidera startowej ilosci bomb
	 * @param value wartosc slidera
	 */
    public void ChangeBombs(float value) {
        bombsAmount = (int)value;
    }

	/** Funkcja handler dla slidera startowej ilosci zyc
	 * @param value wartosc slidera
	 */
    public void ChangeLifes(float value) {
        lifesAmount = (int)value;
    }

	/** Funkcja handler dla slidera szansy na powerup
	 * @param value wartosc slidera
	 */
    public void ChangePowerups(float value) {
        powerupChanceAmount = (int)value;
    }

	/** Funkcja pomocnicza ustawiajaca tryb gry w PlayerPrefs
	 * @param mode tryb gry (TPP lub FPP)
	 */
    public void SetPlayerMode(string mode) {
        PlayerPrefs.SetString("playerMode", mode);
    }

	/** Funkcja pomocnicza pozwalająca ustawic wartosci dla gry klasycznej */
    public void PrepareClassicGame() {
        xSize = 15;
        ySize = 15;
        zSize = 1;
        lifesAmount = 3;
        bombsAmount = 1;
        powerupChanceAmount = 25;
        SetPlayerMode("TPP");
        PlayerPrefs.SetInt("P1Controlls", 2);
        PlayerPrefs.SetInt("P2Controlls", 4);
        PlayerPrefs.SetInt("P3Controlls", 0);
        PlayerPrefs.SetInt("P4Controlls", 0);
    }
}
