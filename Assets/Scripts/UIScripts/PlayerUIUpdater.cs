using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIUpdater : MonoBehaviour
{
    [SerializeField] private Text lifesText;
    [SerializeField] private Image lifesImageObject;

    [SerializeField] private Text bombAmountText;

    [SerializeField] private Sprite lifesSprite;
    [SerializeField] private Sprite immuneLifesSprite;
    public GameObject player;

    private void Start() {
        if (player != null)
        {
            UpdateLifesText();
            UpdateBombAmountText();
        }
    }

    private void Update() {
        if (player != null)
        {
            UpdateLifesText();
            UpdateBombAmountText();
            UpdateHeartsIconOnImmune();
        }
    }

    public void UpdateLifesText() {
        lifesText.text = player.GetComponent<Character>().lifes.ToString();
    }

    public void UpdateBombAmountText() {
        bombAmountText.text = player.GetComponent<Character>().bombs.ToString();
    }

    public void UpdateHeartsIconOnImmune() {
        if(player.GetComponent<Character>().isImmune){
            lifesImageObject.sprite = immuneLifesSprite;
        } else {
            lifesImageObject.sprite = lifesSprite;
        }
    }
}
