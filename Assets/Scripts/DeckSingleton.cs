using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckSingleton : MonoBehaviour
{
    public static DeckSingleton Instance { get; set; }
    public List<CardObject> cardObjects;
    public List<CardObject> currentHand;
    public List<CardObject> enemyHand = new List<CardObject>();

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
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        NewEnemyHand();
    }
    
    private void NewEnemyHand()
    {
        for(int i = 0; i <= 4; i++) {
            int random = Random.Range(0, CardManager.Instance.cardObjects.Count);
            enemyHand.Add(CardManager.Instance.cardObjects[random]);
        }
    }

    public void AddCard(CardObject card)
    {
        List<CardObject> cardList = new List<CardObject>(cardObjects);
        cardList.Add(card);
        cardObjects = cardList;
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
