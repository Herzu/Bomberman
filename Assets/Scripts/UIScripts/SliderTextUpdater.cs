using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderTextUpdater : MonoBehaviour
{
    public Text sliderText;
    public Slider sliderObject;

    public void Start() {
        sliderText.text = sliderObject.value.ToString();
    }

    public void UpdateText(float value) {
        sliderText.text = value.ToString();
    } 
}
