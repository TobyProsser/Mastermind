using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    public GameObject cardsHolder;
    public List<GameObject> cards;

    List<GameObject> playerCards;

    public float xOFFSET;
    public float yOFFSET;

    int numberOfObjects = 5;
    public float spacing = 2f;
    void Start()
    {
        float increment = 6 / numberOfObjects;
        playerCards = new List<GameObject>();
        for (int i = 0; i < numberOfObjects; i++)
        {
            Vector3 spawnPosition = new Vector3(0, 0f, 1f) * spacing;

            // Instantiate the object
            GameObject newCard = Instantiate(cards[i], spawnPosition + new Vector3(xOFFSET, yOFFSET, -1), Quaternion.identity);
            newCard.transform.parent = cardsHolder.transform;
            playerCards.Add(newCard);
        }
    }

    public List<CardObject> FindCardOrder()
    {
        playerCards = playerCards.OrderBy(obj => obj.transform.position.x).ToList();
        List<CardObject> cardObjects = new List<CardObject>();
        foreach(var playerCard in playerCards) {
            CardObject newObject = new CardObject();
            CardController curController = playerCard.GetComponent<CardController>();
            newObject.cardName = curController.cardName;
            newObject.cardType = curController.cardType.ToString();
            newObject.subType = curController.subType.ToString();
            newObject.typeAmount = curController.typeAmount;
            newObject.time = curController.time;

            cardObjects.Add(newObject);
        }
        return cardObjects;
    }
}
