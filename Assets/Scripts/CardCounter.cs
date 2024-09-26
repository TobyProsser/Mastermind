using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardCounter : MonoBehaviour
{

    public TextMeshProUGUI attackText;
    public TextMeshProUGUI blockText;
    public TextMeshProUGUI potionText;

    public TextMeshProUGUI healthText;
    void Start()
    { 
        UpdateText();
    }

    public void UpdateText(){
        DeckSingleton singleton = DeckSingleton.Instance;
        singleton.FindTypes();
        attackText.text = singleton.attackCardsAMT.ToString();
        blockText.text = singleton.blockCardsaMT.ToString();
        potionText.text = singleton.potionCardsaMT.ToString();
        if(healthText){
            healthText.text = singleton.player.health.ToString();
        }
    }
}
