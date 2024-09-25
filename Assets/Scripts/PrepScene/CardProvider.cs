using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CardProvider : MonoBehaviour
{
    public Transform topPos;
    public Transform bottomPos;

    public GameObject baseCard;

    public CardCounter counter;

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
        SpawnCard(true);
    }

    public void SpawnCard(bool doubled){
        if(currentCardNum < 5)
        {
            if(spawnedCard)
            {
                StartCoroutine(MoveBottomToTop());
            }
            //print("currentCardNum" + currentCardNum + "deck count" + deckSingleton.cardObjects.Count);
            if(deckSingleton.cardObjects.Count > 1){
                SpawnBottomCard(doubled);
            }else if(deckSingleton.cardObjects.Count == 1)
            { 
                SpawnBottomCard(doubled);
                StartCoroutine(MoveBottomToTop());
            }
        }else if(currentCardNum == 5)
        {
            StartCoroutine(MoveBottomToTop());
        }
        
    }

    private IEnumerator MoveBottomToTop()
    {
        if(!spawnedCard.GetComponent<TempCardTracker>().locked)
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
        else{yield return null;}
    }

    private bool RemoveCardByName(string cardName)
    {
        //print("Name: " + cardName);
        CardObject cardToRemove = DeckSingleton.Instance.cardObjects.FirstOrDefault(card => card.cardName == cardName);
        if (cardToRemove != null)
        {
            //print("card Name: " + cardName + "card to remove: " + cardToRemove.cardName);
            DeckSingleton.Instance.cardObjects.Remove(cardToRemove);

            counter.UpdateText();
            return true;
        }
        return false;
    }

    void SpawnBottomCard(bool spawnDouble)
    {
        GameObject curCard = Instantiate(baseCard, bottomPos.position, bottomPos.rotation);
            spawnedCard = curCard;
            print("Current Card Number: " + currentCardNum + "deck amount: " + deckSingleton.cardObjects.Count);
            curCard.GetComponent<SpriteRenderer>().sprite = deckSingleton.cardObjects[0].cardIcon;
            curCard.GetComponent<CardController>().thisCardsObject = deckSingleton.cardObjects[0];
            curCard.GetComponent<CardController>().time= deckSingleton.cardObjects[0].time;
            curCard.GetComponent<BoxCollider2D>().enabled = false;
            TempCardTracker tempCardTracker = curCard.AddComponent<TempCardTracker>();
            tempCardTracker.cardProvider = this;
            currentCardNum++;

            bool removed = RemoveCardByName(curCard.GetComponent<CardController>().thisCardsObject.cardName);
    
            if(spawnDouble)
            {
                SpawnCard(false);
            }
    }
}
