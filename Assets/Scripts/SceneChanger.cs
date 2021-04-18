using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger instance;
    private GameObject sceneFader;
    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        LoadSceneFader();
        FadeInCurrentScene();
    }
    
    public void ChangeScene(string SceneName) {
        FadeOutCurrentScene();
        SceneManager.LoadScene(SceneName);
    }

    public void FadeOutCurrentScene() {
        sceneFader.SetActive(true);
        sceneFader.GetComponent<CanvasGroup>().alpha = 0;
        FadeManager.instance.FadeIn(sceneFader);
    }

    public void FadeInCurrentScene() {
        sceneFader.SetActive(true);
        sceneFader.GetComponent<CanvasGroup>().alpha = 1;
        FadeManager.instance.FadeOut(sceneFader);
    }
    
    private void LoadSceneFader() {
        sceneFader = GameObject.FindGameObjectWithTag("SceneFade");
        sceneFader.GetComponent<CanvasGroup>().alpha = 0;
        sceneFader.SetActive(false);
    }
}
