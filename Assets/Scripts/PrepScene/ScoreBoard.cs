using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    int round;
    bool playerWon;

    public TextMeshProUGUI[] round1;
    public TextMeshProUGUI[] round2;
    public TextMeshProUGUI[] round3;

    private void  OnEnable()
    {
        round =  DeckSingleton.Instance.round;
        playerWon = DeckSingleton.Instance.playerWon;
    }

    private void LateUpdate() {
        SetRound();
    }

    public void SetRound(){
        switch (round){
            case 0: break;
            case 1: {
                round1[0].text = playerWon ? "W" : "L";
                round1[1].text = playerWon ? "L" : "W";
                break;
            }
            case 2: {
                round2[0].text = playerWon ? "W" : "L";
                round2[1].text = playerWon ? "L" : "W";
                break;
            }
            case 3: {
                round3[0].text = playerWon ? "W" : "L";
                round3[1].text = playerWon ? "L" : "W";
                break;
            }
        }
    }
    
}
