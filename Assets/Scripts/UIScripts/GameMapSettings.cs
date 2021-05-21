using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMapSettings : MonoBehaviour
{
    public GameObject mapVisualization;
    public GameObject playerVisualization;
    private int xSize;
    private int ySize;
    private int zSize;
    private int bombsAmount;
    private int lifesAmount;
    private int powerupChanceAmount;

    public Slider xSlider;
    public Slider ySlider;
    public Slider zSlider;
    public Slider bombsSlider;
    public Slider lifesSlider;
    public Slider powerupsSlider;

    public void OnEnable() {
        xSize = PlayerPrefs.GetInt("xSize");
        ySize = PlayerPrefs.GetInt("ySize");
        zSize = PlayerPrefs.GetInt("zSize");
        bombsAmount = PlayerPrefs.GetInt("bombsAmount");
        lifesAmount = PlayerPrefs.GetInt("lifesAmount");
        powerupChanceAmount = PlayerPrefs.GetInt("powerupChanceAmount");
    }

    public void Start() {
        xSlider.value = xSize;
        ySlider.value = ySize;
        zSlider.value = zSize;
        bombsSlider.value = bombsAmount;
        lifesSlider.value = lifesAmount;
        powerupsSlider.value = powerupChanceAmount;
        UpdateVisualization();
    }

    public void OnDisable() {
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

    public void ChangeXSize(float value) {
        xSize = (int)value;
        UpdateVisualization();
    }

    public void ChangeYSize(float value) {
        ySize = (int)value;
        UpdateVisualization();
    }

    public void ChangeZSize(float value) {
        zSize = (int)value;
        UpdateVisualization();
    }

    public void ChangeBombs(float value) {
        bombsAmount = (int)value;
    }

    public void ChangeLifes(float value) {
        lifesAmount = (int)value;
    }

    public void ChangePowerups(float value) {
        powerupChanceAmount = (int)value;
    }

    public void SetPlayerMode(string mode) {
        PlayerPrefs.SetString("playerMode", mode);
    }

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
