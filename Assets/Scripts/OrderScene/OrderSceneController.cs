using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrderSceneController : MonoBehaviour
{
    CardHandler cardHandler;

    void Awake()
    {
        cardHandler = transform.GetComponent<CardHandler>();
    }
    public void BrawlButton()
    {
        StaticPlayerData.playerCards = cardHandler.FindCardOrder();
        
        SceneManager.LoadScene("BrawlScene");
    }
}
