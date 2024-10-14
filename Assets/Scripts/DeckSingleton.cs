using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckSingleton : MonoBehaviour
{
    public static DeckSingleton Instance { get; set; }

    public Player player;
    public GameObject playersGameObject;
    public GameObject screenSceneToHide;
    public GameObject Homeland;
    public GameObject consoleCamera;
    public Vector3 homelandSpawn;
    public float enemyHealth;
    public string enemyName;
    public GameObject enemy;

    public List<CardObject> cardObjects;
    public List<CardObject> currentHand;
    public List<CardObject> enemyHand = new List<CardObject>();

    public int round;
    public bool playerWon;

    public int attackCardsAMT;
    public int blockCardsaMT;
    public int potionCardsaMT;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        //Change this to store and save player data, not creating new one every login
    
        player = new Player();
        NewEnemyHand();


    }
    
    private void NewEnemyHand()
    {
        for(int i = 0; i <= 4; i++) {
            int random = Random.Range(0, CardManager.Instance.cardObjects.Count);
            enemyHand.Add(CardManager.Instance.cardObjects[random]);
        }
    }
    public void FindTypes()
    {
        blockCardsaMT = 0;
        attackCardsAMT = 0;
        potionCardsaMT = 0;

        foreach(var card in cardObjects) {
            if(card.cardType == "Block") {blockCardsaMT += 1;}
            else if(card.cardType == "Attack") {attackCardsAMT += 1;}
            else if(card.cardType == "Potion") {potionCardsaMT += 1;}

        }
    }
}
