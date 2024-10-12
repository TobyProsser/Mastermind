using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandCollection : MonoBehaviour
{
    public GameObject[] holsters;

    public CardProvider provider;

    List<CardObject> GetPlayersHand (){
        List<CardObject> playerHand = new List<CardObject>();
        foreach(GameObject holster in holsters){
            HolsterController controller = holster.GetComponent<HolsterController>();
            if(controller != null && controller.curCard != null){
                CardObject cardObject = controller.curCard.GetComponent<CardController>().thisCardsObject;
                playerHand.Add(cardObject);
            }
        }

        return playerHand;
    }

    void SubmitPlayersHand(){
        DeckSingleton.Instance.currentHand =  GetPlayersHand();
    }

    public void BrawlButton(){

        List<GameObject> cards = provider.spawnedCards;

        foreach(var obj in cards) {
            Destroy(obj);
        }
        
        SubmitPlayersHand();

        SceneManager.LoadScene("BrawlScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("PrepScene");
    }
}
