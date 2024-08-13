using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSingleton : MonoBehaviour
{
    public static DeckSingleton Instance { get; set; }
    public List<CardObject> cardObjects;

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
            print(card.cardType);
            if(card.cardType == "Block") {blockCardsaMT += 1;}
            else if(card.cardType == "Attack") {attackCardsAMT += 1;}
            else if(card.cardType == "Potion") {potionCardsaMT += 1;}

            print("Blocks: " + blockCardsaMT + "Potions: " + potionCardsaMT + "Attacks: " + attackCardsAMT);
        }
    }
}
