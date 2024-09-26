using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectorController : MonoBehaviour
{
    public GameObject card;
    public GameObject[] screenCards;

    public Transform[] spawnPoints;

    public CardCounter counter;

    int curIcons = 0;
    CardManager cardManager;

    void Start(){
        cardManager = CardManager.Instance;
    }

    public void NextButton(){
        SceneManager.LoadScene("ConsoleScene");
    }
    public void SpawnCards(int iconButtonNum)
    {
        curIcons = iconButtonNum;
        int i = 0;
        foreach (var card in screenCards){
            card.GetComponent<Image>().sprite = cardManager.cardObjects[iconButtonNum + i].cardIcon;
            
            i++;
        }
    }

    public void ScreenCardSelect(int iconButtonNum){
        GameObject curCard = Instantiate(card, spawnPoints[iconButtonNum].transform.position, screenCards[0].transform.rotation);
        curCard.GetComponent<SpriteRenderer>().sprite = cardManager.cardObjects[curIcons + iconButtonNum].cardIcon;
        curCard.transform.localScale = new Vector3(.075f,.075f,.075f);
        curCard.GetComponent<Rigidbody2D>().gravityScale = .83f;
        curCard.GetComponent<CardController>().time = iconButtonNum;
        //print("Time set as: " + iconButtonNum);
        CardObject cardObject = cardManager.cardObjects[curIcons + iconButtonNum];
        DeckSingleton.Instance.cardObjects.Add(cardObject);
        counter.UpdateText();
    }

    public void BackButton(){
        SceneManager.LoadScene("PrepScene");
    }
}
