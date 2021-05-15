using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public int maxFpsValue = 60;
    void Awake() {
        SetApplicationMaxFps(60);
    }

    void Start() {
        resolutions = Screen.resolutions;
        PrepareResolutionDropdown();
    }

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

    private void SetApplicationMaxFps(int newMaxFps) {
        maxFpsValue = newMaxFps;
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = newMaxFps;
    }

    public void SetMasterVolume(float volume) {
        audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution res = resolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetMaxFps(float value) {
        SetApplicationMaxFps((int)value);
    }

}
