using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour
{
    public static PlayerUIUpdater instance;

    [SerializeField] private Text lifesText;
    [SerializeField] private Image lifesImageObject;

    [SerializeField] private Text bombAmountText;

    [SerializeField] private Sprite lifesSprite;
    [SerializeField] private Sprite immuneLifesSprite;
    private Character player;

    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerIt in players)
        {
            if (playerIt.GetComponent<FPS_Player>())
            {
                if (PlayerPrefs.GetString("playerMode") == "FPP")
                {
                    player = playerIt.GetComponent<Character>();
                }
            }
            else if (playerIt.GetComponent<TPS_Player>())
            {
                if (PlayerPrefs.GetString("playerMode") == "TPP")
                {
                    player = playerIt.GetComponent<Character>();
                }
            }
        }
        PlayerUIUpdater.instance.UpdateLifesText();
        PlayerUIUpdater.instance.UpdateBombAmountText();
    }

    private void Update() {
        UpdateLifesText();
        UpdateBombAmountText();
        UpdateHeartsIconOnImmune();
    }

    public void UpdateLifesText() {
        lifesText.text = player.lifes.ToString();
    }

    public void UpdateBombAmountText() {
        bombAmountText.text = player.bombs.ToString();
    }

    public void UpdateHeartsIconOnImmune() {
        if(player.isImmune){
            lifesImageObject.sprite = immuneLifesSprite;
        } else {
            lifesImageObject.sprite = lifesSprite;
        }
    }
}
