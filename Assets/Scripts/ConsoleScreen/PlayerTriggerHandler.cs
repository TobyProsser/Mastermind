using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerTriggerHandler : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "House")
        {
            DeckSingleton.Instance.screenSceneToHide.SetActive(false);
            SceneManager.LoadScene("DeckScene", LoadSceneMode.Additive);
        }
        else if(other.gameObject.tag == "Collectable")
        {
            AddCard(other.gameObject);
        }
    }

    private void AddCard(GameObject gameObject)
    {
        CardObject card = gameObject.GetComponent<CardController>().thisCardsObject;
        DeckSingleton.Instance.cardObjects.Add(card);

        Destroy(gameObject);
    }
}
