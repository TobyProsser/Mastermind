using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Mathematics;

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

    public List<GameObject> particles;
    int stepNum= 0;
    // Start is called before the first frame update
    void Start()
    {
        player1 = DeckSingleton.Instance.player;
        player2 = new Player();

        player1Moves = DeckSingleton.Instance.currentHand;
        player2Moves = DeckSingleton.Instance.enemyHand;
        Step();

        UpdateHealthText();
    }

    void Step(){
        CardObject curCard = player1Moves[stepNum];
        //player1Text.text = curCard.cardName;
        playerCard.GetComponent<SpriteRenderer> ().sprite = curCard.cardIcon;
        

        string type1 = curCard.cardType.ToString();
        string subType1 = curCard.subType.ToString();
        float amount1 = curCard.typeAmount;
        int time1 = curCard.time;
        double playerAnimTime = curCard.animTime;
        
        CardObject curCard2 = player2Moves[stepNum];
        string type2 = curCard2.cardType;
        //player2Text.text = curCard.cardName;
        enemyCard.GetComponent<SpriteRenderer> ().sprite = curCard2.cardIcon;
        
        string subType2 = curCard2.subType;
        float amount2 = curCard2.typeAmount;
        int time2 = curCard2.time;
        double enemyAnimTime = curCard2.animTime;
        
        DetectOutcome(player1, type1,subType1, time1, amount1, player2, type2, subType2, amount2, time2);
        
        StartCoroutine(PlayAnimations(time1, type1, playerAnimTime, time2, type2, enemyAnimTime));

    }

    public void UpdateHealthText(){
        player1HealthText.text = player1.health.ToString();
        player2HealthText.text = player2.health.ToString();
    }
    IEnumerator PlayAnimations(float playerTime, string playerType, double playerAnimTime, float EnemyTime, string EnemyType, double enemyAnimTime){
        yield return new WaitForSeconds(.5f);

        if(playerTime < EnemyTime)
        {
            GameObject curParticles = Instantiate(particles[0], playerCard.transform.position, quaternion.identity);
            curParticles.transform.Rotate(new Vector3(90, 0,0));
            curParticles.transform.parent = playerCard.transform;
            curParticles.transform.localPosition = new Vector3(0, 0, 0);
            curParticles.transform.localScale=new Vector3(.5f, .5f,.5f);

            playerCard.GetComponent<Animator>().SetTrigger(playerType);
            yield return new WaitForSeconds((float)playerAnimTime);

            enemyCard.GetComponent<Animator>().SetTrigger(EnemyType);
            yield return new WaitForSeconds((float)enemyAnimTime);
        }
        else if(playerTime > EnemyTime)
        {
            GameObject curParticles = Instantiate(particles[0], playerCard.transform.position, quaternion.identity);
            curParticles.transform.Rotate(new Vector3(90, 0,0));
            curParticles.transform.parent = playerCard.transform;
            curParticles.transform.localPosition = new Vector3(0, 0, 0);
            curParticles.transform.localScale=new Vector3(.5f, .5f,.5f);
            
            enemyCard.GetComponent<Animator>().SetTrigger(EnemyType);
            yield return new WaitForSeconds((float)enemyAnimTime);

            playerCard.GetComponent<Animator>().SetTrigger(playerType);
            yield return new WaitForSeconds((float)playerAnimTime);
        }
        else if(playerTime == EnemyTime){
            GameObject curParticles = Instantiate(particles[0], playerCard.transform.position, quaternion.identity);
            curParticles.transform.Rotate(new Vector3(90, 0,0));
            curParticles.transform.parent = playerCard.transform;
            curParticles.transform.localPosition = new Vector3(0, 0, 0);
            curParticles.transform.localScale=new Vector3(.5f, .5f,.5f);
            
            enemyCard.GetComponent<Animator>().SetTrigger(EnemyType);
            
            playerCard.GetComponent<Animator>().SetTrigger(playerType);
            yield return new WaitForSeconds((float)playerAnimTime + .5f);
        }

        UpdateHealthText();
        stepNum++;
        StartCoroutine(StepWait());
    }
    IEnumerator StepWait(){
        yield return new WaitForSeconds(1f);
        if(stepNum < 5)
        {
            Step();
        }
        else{
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene("PrepScene");
        }
    }

    void DetectOutcome(Player attackingPlayer, string attackingType, string attackingSubType, int attackingTime, float attackingAmount, Player defendingPlayer, string defendingType,string defendingSubType, float defendingAmount, int defendingTime)
    {
        //print("Attacking Type: " + attackingType + "DefendingType " + defendingType);
        switch (attackingType){
            case "Attack": {
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
                defendingPlayer.health -= attackingAmount <= 0 ? 0 : attackingAmount;
                
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
        print("Damage to deal: " + attackingAmount + "Damage to take: " + defendingAmount);
        switch (defendingType){
            case "Block": {
                //if it takes longer to attack then to defend then some of the attacking amount is decreased
                if(attackingTime >= defendingTime)
                {
                //Subract attacking amount from block amount
                attackingAmount -= defendingAmount;
                //Then Deal that damage, if damage is less then 0 deal 0 damage
                defendingPlayer.health -= attackingAmount <= 0 ? 0 : attackingAmount;
                
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
    }
}
