using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Enum opisujący mozliwe typy sterowania postacia
public enum ControlType {
    None = 0,
    Arrows = 1,
    WSAD = 2,
    Controller = 3,
    AI = 4
}

//! Klasa obslugujaca ekran wyboru graczy w Menu
public class MultiplayerPlayersSettings : MonoBehaviour
{
    private ControlType[] playerControls = new ControlType[4];  //!< tablica przechowujaca informacje dotyczaca typu sterowania dla kazdego z graczy

    public void OnDisable(){
        // ustawienie wybranego typu sterowania dla kazdego z graczy
        int index = 0;
        foreach (ControlType type in playerControls) {
            PlayerPrefs.SetInt("P"+ (index + 1) +"Controlls", (int)type);
            index++;
        }
    }

	/** Funkcja ustawiajaca graczowi odpowiedni typ sterowania
	 * @param playerNumber numer gracza któremu ma zostac ustawiony typ sterowania
	 * @param type typ sterowania ktory ma zosta ustawiony
	 */
    public void setControlType(int playerNumber, ControlType type) {
        playerControls[playerNumber - 1] = type;
    }

	/** Funkcja obslugująca Toggle odpowiadajacy za ustawienie wartosci sterowania gracza numer 1 
	 * @param value wartosc Toggle'a
	 */
    public void handleP1Toggle(bool value) {
        setControlType(1, value ? ControlType.WSAD : ControlType.None);
    }
    
    
	/** Funkcja obslugująca Toggle odpowiadajacy za ustawienie wartosci sterowania gracza numer 2
	 * @param value wartosc Toggle'a
	 */
    public void handleP2Toggle(bool value) {
        setControlType(2, value ? ControlType.Arrows : ControlType.None);
    }
    
    
	/** Funkcja obslugująca Toggle odpowiadajacy za ustawienie wartosci sterowania gracza numer 3 
	 * @param value wartosc Toggle'a
	 */
    public void handleP3Toggle(bool value) {
        setControlType(3, value ? ControlType.Controller : ControlType.None);
    }


	/** Funkcja obslugująca Toggle odpowiadajacy za ustawienie wartosci sterowania gracza numer 4
	 * @param value wartosc Toggle'a
	 */    
    public void handleP4Toggle(bool value) {
        setControlType(4, value ? ControlType.AI : ControlType.None);
    }
}
