using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;

    private GameObject activePanel;

    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        activePanel = GameObject.FindGameObjectWithTag("Panel");
    }
    
    public void ChangePanel(GameObject panelToOpen) {
        activePanel = GameObject.FindGameObjectWithTag("Panel");
        FadeManager.instance.FadeOut(activePanel);
        activePanel.SetActive(false);
        activePanel = panelToOpen;
        activePanel.SetActive(true);
        activePanel.GetComponent<CanvasGroup>().alpha = 0;
        FadeManager.instance.FadeIn(activePanel);
    }

    public void CloseApp() {
        Application.Quit();
    }
}
