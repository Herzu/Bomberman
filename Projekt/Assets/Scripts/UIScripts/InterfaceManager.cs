using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//! Klasa odpowiedzialna za zarzadzanie wyswietlanym interfejsem
public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;    //!< obiekt wzorca singleton

    private GameObject activePanel;             //!< obiekt przechowujacy aktualnie wyswietlany panel

    private void Awake() {
        // ustawienie singletonu
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        // znalezienie aktualnie wyswietlanego panelu
        activePanel = GameObject.FindGameObjectWithTag("Panel");
    }
    
	/** Funkcja obslugujaca zmiene wyswietlanego panelu
	 * @param panelToOpen panel ktory ma zostac wyswietlony
	 */
    public void ChangePanel(GameObject panelToOpen) {
        activePanel = GameObject.FindGameObjectWithTag("Panel");
        if(activePanel) {
            FadeManager.instance.FadeOut(activePanel);
            activePanel.SetActive(false);
            activePanel = panelToOpen;
            activePanel.SetActive(true);
            activePanel.GetComponent<CanvasGroup>().alpha = 0;
            FadeManager.instance.FadeIn(activePanel);
        }
    }

    /** Funkcja zamykajaca aplikacje */
    public void CloseApp() {
        Application.Quit();
    }
}
