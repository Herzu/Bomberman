using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//! Klasa odpowiedzialna za efekty rozjasniania/zanikania
public class FadeManager : MonoBehaviour {
    public static FadeManager instance;                 //!< obiekt wzorca singleton
    [SerializeField] private float fadeinTime = 1f;     //!< czas wykonywania efektu rozjasniania
    [SerializeField] private float fadeoutTime = 1f;    //!< czas wykonywania efektu zanikania


    private void Awake() {
        // ustawienie singletonu
        if(!instance) {
            instance = this;
        }
    }

    /** Funkcja obsługująca efekt rozjaśniania
	 * @param canvasGroup canvas grupa na której zostanie wykonany efekt
	 */
    public void FadeIn(CanvasGroup canvasGroup) {
        canvasGroup.DOFade(1, fadeinTime).SetUpdate(true);
    }

    /** Funkcja obsługująca efekt zanikania
	 * @param canvasGroup canvas grupa na której zostanie wykonany efekt
	 */
    public void FadeOut(CanvasGroup canvasGroup) {
        canvasGroup.DOFade(0, fadeoutTime).SetUpdate(true);
    }

    /** Funkcja obsługująca efekt rozjasniania
	 * @param canvasGroup canvas grupa na której zostanie wykonany efekt
	 * @param fadeTime czas wykonywania efektu
	 */
    public void FadeIn(CanvasGroup canvasGroup, float fadeTime) {
        canvasGroup.DOFade(1, fadeTime).SetUpdate(true);
    }

    /** Funkcja obsługująca efekt zanikania
	 * @param canvasGroup canvas grupa na której zostanie wykonany efekt
	 * @param fadeTime czas wykonywania efektu
	 */
    public void FadeOut(CanvasGroup canvasGroup, float fadeTime) {
        canvasGroup.DOFade(0, fadeTime).SetUpdate(true);
    }

    /** Funkcja obsługująca efekt rozjasniania
	 * @param gameObject obiekt na którego canvas grupie zostanie wykonany efekt
	 */
    public void FadeIn(GameObject gameObject) {
        gameObject.GetComponent<CanvasGroup>().DOFade(1, fadeinTime).SetUpdate(true);
    }

    /** Funkcja obsługująca efekt zanikania
	 * @param gameObject obiekt na którego canvas grupie zostanie wykonany efekt
	 */
    public void FadeOut(GameObject gameObject) {
        gameObject.GetComponent<CanvasGroup>().DOFade(0, fadeoutTime).SetUpdate(true);
    }

    /** Funkcja obsługująca efekt rozjasniania
	 * @param gameObject obiekt na którego canvas grupie zostanie wykonany efekt
	 * @param fadeTime czas wykonywania efektu
	 */
    public void FadeIn(GameObject gameObject, float fadeTime) {
        gameObject.GetComponent<CanvasGroup>().DOFade(1, fadeTime).SetUpdate(true);
    }

    /** Funkcja obsługująca efekt zanikania
	 * @param gameObject obiekt na którego canvas grupie zostanie wykonany efekt
	 * @param fadeTime czas wykonywania efektu
	 */
    public void FadeOut(GameObject gameObject, float fadeTime) {
        gameObject.GetComponent<CanvasGroup>().DOFade(0, fadeTime).SetUpdate(true);
    }
}