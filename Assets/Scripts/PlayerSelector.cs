using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSelector : MonoBehaviour
{
    public Color color;
    public Color disabledColor;
    public GameObject frame;

    public void handleToggleUI(bool value) {
        Image backgroundFrame = frame.GetComponent<Image>();
        if(value) {
            backgroundFrame.color = color;
        } else {
            backgroundFrame.color = disabledColor;
        }
    }
}
