using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.Mathematics;
using UnityEngine.UIElements;

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

    public TextMeshProUGUI player1AnimatedText;
    public TextMeshProUGUI player2AnimatedText;

    public Animator player1Animator;
    public Animator player2Animator;

    public Animator player1CardAnimator;
    public Animator player2CardAnimator;

    public List<GameObject> particles;
    int stepNum= 0;

    int armored = 0;

    Player playerHasSpeed;
    // Start is called before the first frame update
    void Start()
    {
        player1 = DeckSingleton.Instance.player;
        player2 = new Player();

        player2.health = DeckSingleton.Instance.enemy.GetComponent<EnemyController>().health;

        player1Moves = DeckSingleton.Instance.currentHand;
        player2Moves = DeckSingleton.Instance.enemyHand;
        Step();

        UpdateHealthText();
    }

    void Step(){

        player1AnimatedText.text = "";
        player2AnimatedText.text = "";

        if(player2.health <= 0)
        {
            DeckSingleton.Instance.screenSceneToHide.SetActive(true);
            DeckSingleton.Instance.enemy.GetComponent<EnemyController>().DropLoot();
            Destroy(DeckSingleton.Instance.enemy);
            
            SceneManager.UnloadSceneAsync("BrawlScene");

            return;
        }else if(armored > 0)
        {
            armored--;
        }
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
        
        //Whoever moves first is the attacking player
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
            if(player2.health <= 0)
            {
                DeckSingleton.Instance.screenSceneToHide.SetActive(true);
                DeckSingleton.Instance.enemy.GetComponent<EnemyController>().DropLoot();
                            
                SceneManager.UnloadSceneAsync("BrawlScene");
            }
            else{
                DeckSingleton.Instance.enemy.GetComponent<EnemyController>().health = player2.health;
                SceneManager.LoadScene("PrepScene", LoadSceneMode.Additive);
                DeckSingleton.Instance.round++;
                DeckSingleton.Instance.playerWon = player1.health > player2.health;
                SceneManager.UnloadSceneAsync("BrawlScene");
                
            }
            
        }
    }
    void DetectOutcome(Player attackingPlayer, string attackingType, string attackingSubType, int attackingTime, float attackingAmount, Player defendingPlayer, string defendingType,string defendingSubType, float defendingAmount, int defendingTime)
    {
        //print("Attacking Type: " + attackingType + "DefendingType " + defendingType);
        switch (attackingType){
            case "Attack": {
                DealDamage(attackingPlayer, defendingPlayer, attackingSubType, attackingAmount, attackingTime, defendingType, defendingAmount, defendingTime, defendingSubType); 
            break;
            }
            case "Block": {
                HandleBlock(attackingPlayer, defendingPlayer,attackingSubType, attackingAmount, attackingTime, defendingType, defendingAmount, defendingTime, defendingSubType);
            break;
            }
            case "Dodge": break;    //If player dodges nothing happens (until area attacks are included)
            case "Potion": {
                HandlePotion(attackingPlayer, defendingPlayer, attackingSubType, attackingAmount, attackingTime, defendingType, defendingAmount, defendingTime, defendingSubType);
                break;
            }
        }
    }

    void ThrowPotion(Player thrownBy, bool potionBlocked, string potionSubType, float potionAmount)
    {
        if(!potionBlocked)
        {
            //print("potion sub Type: " + potionSubType);
            switch(potionSubType)
        {
            case "Heal": {
                PlayerGiveHealth(thrownBy, potionAmount);
                break;
            }
            case "Harden": {
                armored = 2;
                break;
            }
            case "Speed": {
                playerHasSpeed = thrownBy;
                break;
            }
        }
        }
    }

    IEnumerator WaitForTriggerTime(Animator cardAnimator, Animator textAnimator, string animationName, string triggerName, float triggerTime)
    {
        // Wait for the specified time
        yield return new WaitForSeconds(triggerTime);

        print("TRIGGERING " + triggerName);
            // Trigger the event
            textAnimator.SetTrigger(triggerName);
    }

    void PlayerGiveHealth(Player player, float amount)
    {
        if(player == player1)
            {
                //print("player health: " + player1.health + " + " + amount);
                player1.health += amount;
                player1AnimatedText.text = "+" + amount as string;
                StartCoroutine(WaitForTriggerTime(player1CardAnimator, player1Animator, "AddHealth", "AddHealth", 1));
            }else{
                player2.health += amount;
                player2AnimatedText.text = "+" + amount as string;
                StartCoroutine(WaitForTriggerTime(player1CardAnimator, player2Animator, "AddHealth", "AddHealth", 1));
            }
    }

    void PlayerTakeDamage(Player player, float amount)
    {
        if(armored <= 0)
        {
            if(player == player1)
            {
                player1.health -= amount;
                player1AnimatedText.text = "-" + amount as string;
                StartCoroutine(WaitForTriggerTime(player1CardAnimator, player1Animator, "SubtractHealth", "SubHealth", 1.45f));
                //print("Player Health: " + player1.health + " decreased by: " + amount);
            }else{
                player2.health -= amount;
                player2AnimatedText.text = "-" + amount as string;
                StartCoroutine(WaitForTriggerTime(player1CardAnimator, player2Animator, "SubtractHealth", "SubHealth", 1.45f));
            }
            
        }else{
            print("Player Was Hardened, No damage Taken");
            //Particles that show that armor potion blocked hit
        }
    }
    void HandlePotion(Player attackingPlayer, Player defendingPlayer, string attackingSubtype, float attackingAmount, int attackingTime, string defendingType, float defendingAmount, int defendingTime, string defendingSubtype)
    {
        bool potionBlocked = false;
        
        switch (defendingType){
            //IF block is potion block null potions effects, else do nothing
            case "Block": {
                if(defendingSubtype == "Potion")
                {
                    potionBlocked = true;
                }
                break;
                }
            case "Dodge": break;    //IF defending player dodged do nothing
            case "Attack": {
                PlayerTakeDamage(attackingPlayer, defendingAmount);
                break;
            }
            case "Potion":{
                //Defender Throws potion and then gets damage dealt by attack
                ThrowPotion(defendingPlayer, false, defendingSubtype, defendingAmount);
                break;
            }
            default: {
                //If player does not attack back, do nothing
                break;
            }
        }
        //print("Throw Potion");
        ThrowPotion(attackingPlayer, potionBlocked, attackingSubtype, attackingAmount);
    }
    void HandleBlock(Player attackingPlayer, Player defendingPlayer, string attackingSubtype, float attackingAmount, int attackingTime, string defendingType, float defendingAmount, int defendingTime, string defendingSubtype)
    {
        switch (defendingType){
            case "Block": break;  //IF defending player also blocked do nothing
            case "Dodge": break;    //IF defending player dodged do nothing
            case "Attack": {
                //If the attacker blocks faster than opponent can block then reduce damage
                if(attackingTime <= defendingTime)   //Time to Block(attacker is blocking) vs Time to attack(defender is trying to deal damage)
                {
                //Subract defenders damage attack from attackers block Amount
                float subHealth = attackingAmount - defendingAmount;
                //Then Deal that damage, if damage is less then 0 deal 0 damage
                PlayerTakeDamage(attackingPlayer, subHealth <= 0 ? 0 : subHealth);
                }
                else{
                    //Else they take full damage
                    PlayerTakeDamage(defendingPlayer , attackingAmount);
                }
                break;
            }
            case "Potion":{ 
                ThrowPotion(defendingPlayer, attackingSubtype == "Potion", defendingSubtype, defendingAmount);
            break;
            }
            default: {
                //If player does not attack back, do nothing
                break;
            }
        }
    }
    void DealDamage(Player attackingPlayer, Player defendingPlayer, string attackingSubtype,float attackingAmount, int attackingTime, string defendingType, float defendingAmount, int defendingTime, string defendingSubtype){
        
        //print("Damage to deal: " + attackingAmount + " Damage to take: " + defendingAmount);
        //print("attacking Health: " + attackingPlayer.health + " defending Health: " + defendingPlayer.health);
        switch (defendingType){
            case "Block": {
                //if it takes longer to attack then to defend then some of the attacking amount is decreased
                if(attackingTime >= defendingTime)
                {
                //Subract attacking amount from block amount
                float subHealth = attackingAmount - defendingAmount;
                //Then Deal that damage, if damage is less then 0 deal 0 damage
                PlayerTakeDamage(defendingPlayer , subHealth <= 0 ? 0 : subHealth);
                
                }
                else{
                    //Else they take full damage
                    PlayerTakeDamage(defendingPlayer , attackingAmount);
                }
                break;
                }
            case "Dodge": break;    //IF defending player dodged deal no damage
            case "Attack": {
                //If the defending player is also attacking then the attacker also takes damage as they are not blocking or dodging.
                PlayerTakeDamage(attackingPlayer ,defendingAmount);
                //but they still take all the damage themselves
                PlayerTakeDamage(defendingPlayer , attackingAmount);
                
                break;
            }
            case "Potion":{
                //Defender Throws potion and then gets damage dealt by attack
                ThrowPotion(defendingPlayer, attackingSubtype == "Potion", defendingSubtype, defendingAmount);
                PlayerTakeDamage(defendingPlayer , attackingAmount);
            break;
            }
            default: {
                //If defending player is not blocking or dodgeing, they take all the damage
                PlayerTakeDamage(defendingPlayer , attackingAmount);
                break;
            }
            
        }

        print("Player Health After: " + player1.health + " Enemy Health After: " + defendingPlayer.health);
    }
}
