using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour {
    public static FadeManager instance;
    [SerializeField] private float fadeinTime = 1f;
    [SerializeField] private float fadeoutTime = 1f;


    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    public void FadeIn(CanvasGroup canvasGroup) {
        canvasGroup.DOFade(1, fadeinTime).SetUpdate(true);
    }

    public void FadeOut(CanvasGroup canvasGroup) {
        canvasGroup.DOFade(0, fadeoutTime).SetUpdate(true);
    }

    public void FadeIn(CanvasGroup canvasGroup, float fadeTime) {
        canvasGroup.DOFade(1, fadeTime).SetUpdate(true);
    }

    public void FadeOut(CanvasGroup canvasGroup, float fadeTime) {
        canvasGroup.DOFade(0, fadeTime).SetUpdate(true);
    }
    public void FadeIn(GameObject gameObject) {
        gameObject.GetComponent<CanvasGroup>().DOFade(1, fadeinTime).SetUpdate(true);
    }

    public void FadeOut(GameObject gameObject) {
        gameObject.GetComponent<CanvasGroup>().DOFade(0, fadeoutTime).SetUpdate(true);
    }

    public void FadeIn(GameObject gameObject, float fadeTime) {
        gameObject.GetComponent<CanvasGroup>().DOFade(1, fadeTime).SetUpdate(true);
    }

    public void FadeOut(GameObject gameObject, float fadeTime) {
        gameObject.GetComponent<CanvasGroup>().DOFade(0, fadeTime).SetUpdate(true);
    }
}