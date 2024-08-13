using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardProvider : MonoBehaviour
{
    public Transform topPos;
    public Transform bottomPos;

    public GameObject baseCard;

    DeckSingleton deckSingleton;

    int currentCardNum;
    void Start()
    {
        currentCardNum = 0;
        deckSingleton = DeckSingleton.Instance;

        SpawnInitCards();
        
    }

    void SpawnInitCards(){
        SpawnCard();
        SpawnCard();
    }

    public void SpawnCard(){
        GameObject curCard = Instantiate(baseCard, topPos.position, topPos.rotation);
        curCard.GetComponent<SpriteRenderer>().sprite = deckSingleton.cardObjects[currentCardNum].cardIcon;
        TempCardTracker tempCardTracker = curCard.AddComponent<TempCardTracker>();
        tempCardTracker.cardProvider = this;
        currentCardNum++;
    }
}
