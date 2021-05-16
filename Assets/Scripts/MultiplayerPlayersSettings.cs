using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ControlType {
    None = 0,
    WSAD = 1,
    Arrows = 2,
    Controller = 3,
    AI = 4
}

public class MultiplayerPlayersSettings : MonoBehaviour
{
    private ControlType[] playerControls;

    public void setControlType(int playerNumber, ControlType type) {
        playerControls[playerNumber - 1] = type;
    }
}
