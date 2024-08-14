using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class BrawlController : MonoBehaviour
{
    public GameObject playerCard;
    public GameObject enemyCard;
    public List<CardObject> player1Moves;
    public Player player1;
    public List<CardObject> player2Moves;
    public Player player2;

    public TextMeshProUGUI player1Text;
    public TextMeshProUGUI player2Text;

    public TextMeshProUGUI player1HealthText;
    public TextMeshProUGUI player2HealthText;
    int stepNum= 0;
    // Start is called before the first frame update
    void Start()
    {
        player1 = new Player();
        player2 = new Player();

        player1Moves = DeckSingleton.Instance.currentHand;
        player2Moves = DeckSingleton.Instance.currentHand;
        Step();
    }

    void Step(){
        CardObject curCard = player1Moves[stepNum];
        //player1Text.text = curCard.cardName;
        playerCard.GetComponent<SpriteRenderer> ().sprite = curCard.cardIcon;

        string type1 = curCard.cardType.ToString();
        string subType1 = curCard.subType.ToString();
        float amount1 = curCard.typeAmount;
        int time1 = curCard.time;

        string type2 = player2Moves[stepNum].cardType;
        //player2Text.text = curCard.cardName;
        enemyCard.GetComponent<SpriteRenderer> ().sprite = curCard.cardIcon;

        string subType2 = player2Moves[stepNum].subType;
        float amount2 = player2Moves[stepNum].typeAmount;
        int time2 = player2Moves[stepNum].time;

        if(time1 < time2){
            //Player1 moves first
            DetectOutcome(player1, type1,subType1, time1, amount1, player2, type2, subType2, amount2, time2);
        }else if(time1 > time2){
            //Player2 moves first
            DetectOutcome(player2, type2, subType2, time2, amount2, player1, type1,subType1, amount1, time1);
        }
        else{
            DetectOutcome(player1, type1,subType1, time1, amount1, player2, type2, subType2, amount2, time2);
        }

        stepNum++;
        StartCoroutine(StepWait());
    }

    IEnumerator StepWait(){
        yield return new WaitForSeconds(2);
        Step();
    }

    void DetectOutcome(Player attackingPlayer, string attackingType, string attackingSubType, int attackingTime, float attackingAmount, Player defendingPlayer, string defendingType,string defendingSubType, float defendingAmount, int defendingTime)
    {
        switch (attackingType){
            case "Damage": {
                DealDamage(attackingPlayer, defendingPlayer, attackingAmount, attackingTime, defendingType, defendingAmount, defendingTime); 
            break;
            }
            case "Block": {
                HandleBlock(attackingPlayer, defendingPlayer, attackingAmount, attackingTime, defendingType, defendingAmount, defendingTime);
            break;
            }
            case "Dodge": break;    //If player dodges nothing happens (until area attacks are included)
            case "Potion": break;
        }
    }

    void HandleBlock(Player attackingPlayer, Player defendingPlayer, float attackingAmount, int attackingTime, string defendingType, float defendingAmount, int defendingTime)
    {
        switch (defendingType){
            case "Block": break;  //IF defending player also blocked do nothing
            case "Dodge": break;    //IF defending player dodged do nothing
            case "Attack": {
                //If the attacker blocks faster than opponent can block then reduce damage
                if(attackingTime <= defendingTime)   //Time to Block(attacker is blocking) vs Time to attack(defender is trying to deal damage)
                {
                //Subract defenders damage attack from attackers block Amount
                defendingAmount -= attackingAmount;
                //Then Deal that damage, if damage is less then 0 deal 0 damage
                defendingPlayer.health -= attackingAmount >= 0 ? 0 : attackingAmount;
                
                }
                else{
                    //Else they take full damage
                    defendingAmount -= attackingAmount;
                }
                break;
            } 
            default: {
                //If player does not attack back, do nothing
                break;
            }
        }
    }
    void DealDamage(Player attackingPlayer, Player defendingPlayer, float attackingAmount, int attackingTime, string defendingType, float defendingAmount, int defendingTime){
        switch (defendingType){
            case "Block": {
                //if it takes longer to attack then to defend then some of the attacking amount is decreased
                if(attackingTime >= defendingTime)
                {
                //Subract attacking amount from block amount
                attackingAmount -= defendingAmount;
                //Then Deal that damage, if damage is less then 0 deal 0 damage
                defendingPlayer.health -= attackingAmount >= 0 ? 0 : attackingAmount;
                
                }
                else{
                    //Else they take full damage
                    defendingPlayer.health -= attackingAmount;
                }
                break;
                }
            case "Dodge": break;    //IF defending player dodged deal no damage
            case "Attack": {
                //If the defending player is also attacking then the attacker also takes damage as they are not blocking or dodging.
                attackingPlayer.health -= defendingAmount;
                //but they still take all the damage themselves
                defendingPlayer.health -= attackingAmount;
                break;
            } 
            default: {
                //If defending player is not blocking or dodgeing, they take all the damage
                defendingPlayer.health -= attackingAmount;
                break;
            }
        }

        player1HealthText.text = player1.health.ToString();
        player2HealthText.text = player2.health.ToString();
    }
}
