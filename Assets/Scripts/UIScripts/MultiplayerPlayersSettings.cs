using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType {
    None = 0,
    Arrows = 1,
    WSAD = 2,
    Controller = 3,
    AI = 4
}

public class MultiplayerPlayersSettings : MonoBehaviour
{
    private ControlType[] playerControls = new ControlType[4];

    public void OnDisable(){
        int index = 0;
        foreach (ControlType type in playerControls) {
            PlayerPrefs.SetInt("P"+ (index + 1) +"Controlls", (int)type);
            index++;
        }
    }

    public void setControlType(int playerNumber, ControlType type) {
        playerControls[playerNumber - 1] = type;
    }

    public void handleP1Toggle(bool value) {
        setControlType(1, value ? ControlType.WSAD : ControlType.None);
    }
    
    public void handleP2Toggle(bool value) {
        setControlType(2, value ? ControlType.Arrows : ControlType.None);
    }
    
    public void handleP3Toggle(bool value) {
        setControlType(3, value ? ControlType.Controller : ControlType.None);
    }
    
    public void handleP4Toggle(bool value) {
        setControlType(4, value ? ControlType.AI : ControlType.None);
    }
}
