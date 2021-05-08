using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour
{
    public static PlayerUIUpdater instance;

    [SerializeField] private Text lifesText;
    [SerializeField] private Text bombAmountText;

    private Character player;

    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
        PlayerUIUpdater.instance.UpdateLifesText();
        PlayerUIUpdater.instance.UpdateBombAmountText();
    }

    private void Update() {
        UpdateLifesText();
        UpdateBombAmountText();
    }

    public void UpdateLifesText() {
        lifesText.text = player.lifes.ToString();
    }

    public void UpdateBombAmountText() {
        bombAmountText.text = player.bombs.ToString();
    }
}
