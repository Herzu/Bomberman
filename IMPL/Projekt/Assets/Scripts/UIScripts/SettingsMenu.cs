using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;


//! Klasa oblusgujaca ekrany menu ustawien aplikacji
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;       //!< obiekt obslugujacy dzwiek
    public Dropdown resolutionDropdown; //!< obiekt dostepu do dropdownu ustawiania rozdzielczosci aplikacji
    private Resolution[] resolutions;   //!< tablica rozdzielczosci ekranu mozliwych do wyboru
    public int maxFpsValue = 60;        //!< wartosc maksymalnej ilosci fps w aplikacji, domyslnie ustawiona na 60
    void Awake() {
        SetApplicationMaxFps(60);
    }

    void Start() {
        // zaladowanie mozliwych rozdzielczosci i wyswietlenie ich w dropdownie
        resolutions = Screen.resolutions;
        PrepareResolutionDropdown();
    }

	/** Funkcja przygotowujaca dropdown z rozdzielczosciami */
    private void PrepareResolutionDropdown() {
        resolutionDropdown.ClearOptions();
        List<string> stringResolutions = new List<string>();
        int currentResolutionIndex = 0;
        int index = 0;

        foreach (Resolution res in resolutions) {
            string option = res.width + "x" + res.height;
            stringResolutions.Add(option);

            if(res.width == Screen.currentResolution.width &&
            res.height == Screen.currentResolution.height) {
                currentResolutionIndex = index;
            }
            index++;
        }

        resolutionDropdown.AddOptions(stringResolutions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

	/** Funkcja ustawiajaca maskymalna ilosc fps
	 * @param newMaxFps nowa maksymalna ilosc fps
	 */
    private void SetApplicationMaxFps(int newMaxFps) {
        maxFpsValue = newMaxFps;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = newMaxFps;
    }

	/** Funkcja ustawiajaca glosnosc
	 * @param volume nowa glosnosc
	 */
    public void SetMasterVolume(float volume) {
        audioMixer.SetFloat("MasterVolume", volume);
    }

	/** Funkcja ustawiajaca tryb pelnoekranowy
	 * @param isFullscreen wartosc bool czy tryb pelnoekranowy powinien zostac wlaczony
	 */
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

	/** Funkcja ustawiajaca rozdzielczosc aplikacji
	 * @param resolutionIndex indeks rozdzielczosci z tablicy rozdzielczosci
	 */
    public void SetResolution(int resolutionIndex) {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

	/** Funkcja pomocnicza obslugujaca slider maksymalnej wartosci fps
	 * @param value wartosc fps
	 */
    public void SetMaxFps(float value) {
        SetApplicationMaxFps((int)value);
    }

}
