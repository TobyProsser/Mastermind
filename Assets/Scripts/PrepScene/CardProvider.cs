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

    GameObject spawnedCard;

    public float cardToTopSpeed = .4f;

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
        if(spawnedCard)
        {
            StartCoroutine(MoveBottomToTop());
        }
        if(currentCardNum < deckSingleton.cardObjects.Count){
            SpawnBottomCard();
        }
        
    }

    private IEnumerator MoveBottomToTop()
    {
        GameObject topCard = spawnedCard;
        while (Vector3.Distance( topCard.transform.position, topPos.position) > 0.01f)
        {
             topCard.transform.position = Vector3.MoveTowards( topCard.transform.position, topPos.position, cardToTopSpeed * Time.deltaTime);
            yield return null; // Wait for the next frame
        }
        topCard.transform.position = topPos.position; // Ensure the object reaches the target position
        topCard.GetComponent<BoxCollider2D>().enabled = true;
    }

    void SpawnBottomCard()
    {
        GameObject curCard = Instantiate(baseCard, bottomPos.position, bottomPos.rotation);
            spawnedCard = curCard;
            curCard.GetComponent<SpriteRenderer>().sprite = deckSingleton.cardObjects[currentCardNum].cardIcon;
            curCard.GetComponent<CardController>().thisCardsObject = deckSingleton.cardObjects[currentCardNum];
            curCard.GetComponent<CardController>().time= deckSingleton.cardObjects[currentCardNum].time;
            curCard.GetComponent<BoxCollider2D>().enabled = false;
            TempCardTracker tempCardTracker = curCard.AddComponent<TempCardTracker>();
            tempCardTracker.cardProvider = this;
            currentCardNum++;
    }
}
