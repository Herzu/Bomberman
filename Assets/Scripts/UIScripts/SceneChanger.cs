using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//! Klasa odpowiedzialna za pomoc w zmienianiu scen
public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;    //!< obiekt wzorca singleton
    private GameObject sceneFader;          //!< obiekt pozwalajacy wykonywac efekt zanikania/rozjasniania przy zmianie scen
    private void Awake() {
        // ustawienie singletonu
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        LoadSceneFader();
        FadeInCurrentScene();
    }
    
    /** Funkcja zmieniajace scene z wykorzystaniem efektu zanikania
	 * @param sceneName nazwa sceny
	 */
    public void ChangeScene(string sceneName) {
        FadeOutCurrentScene();
        SceneManager.LoadScene(sceneName);
    }

    /** Funkcja dokonujaca efektu zanikania na obecnie odpalonej scenie */
    public void FadeOutCurrentScene() {
        sceneFader.SetActive(true);
        sceneFader.GetComponent<CanvasGroup>().alpha = 0;
        FadeManager.instance.FadeIn(sceneFader);
    }

    /** Funkcja dokonujaca efektu rozjasniania na obecnie odpalonej scenie */
    public void FadeInCurrentScene() {
        sceneFader.SetActive(true);
        sceneFader.GetComponent<CanvasGroup>().alpha = 1;
        FadeManager.instance.FadeOut(sceneFader);
    }
    
    /** Funkcja sluzaca do zaladowania obiektu pomocniczego do wykonania efektu zanikania/rozjasniania */
    private void LoadSceneFader() {
        sceneFader = GameObject.FindGameObjectWithTag("SceneFade");
        sceneFader.GetComponent<CanvasGroup>().alpha = 0;
        sceneFader.SetActive(false);
    }
}
