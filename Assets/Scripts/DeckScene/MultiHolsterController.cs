using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiHolsterController : MonoBehaviour
{
    [HideInInspector]
    public List<GameObject> cards = new List<GameObject>();
    int curCardInt = 0;
    public Transform thisCenterLocation;
    public float mutliSpeed;
    float multiFinalSize = .056f;
    bool holdingCard;

    public bool acceptMultipleCards;
    public GameObject[] thisLights;
    public bool multiKeepSize;
    void MultiCardEntered(GameObject other){
        cards.Add(other.gameObject);
                DragAndDrop dragAndDrop = cards[curCardInt].GetComponent<DragAndDrop>();
                if(!dragAndDrop.locked && !dragAndDrop.mouseDown)
                {
                    Rigidbody2D rb = cards[curCardInt].GetComponent<Rigidbody2D>();
                    if(rb)
                    {
                        rb.simulated = false;
                        rb.velocity = Vector2.zero;
                    }
                    cards[curCardInt].GetComponent<DragAndDrop>().locked = true;
                    
                    StartCoroutine(MoveAndResizeObject(thisCenterLocation.position, multiFinalSize, curCardInt));

                    MultiHandleLights(curCardInt);
                }

        curCardInt++;

    }

    private void MultiHandleLights(int curInt)
    {
        int time = cards[curInt].GetComponent<CardController>().time;
        //print("Time on card: " + time);
        for(int i = 0; i < 3; i++) {
            thisLights[i].SetActive(time == i);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Card")
        {
            MultiCardEntered(other.gameObject);
        }
    }

    IEnumerator MoveAndResizeObject(Vector3 target, float finalSize, int cardNum)
{
    Vector3 startPosition = cards[cardNum].transform.position;
    float initialSize = cards[cardNum].transform.localScale.x;
    Vector3 targetYPosition = new Vector3(startPosition.x, target.y, startPosition.z);
    float distanceY = Mathf.Abs(target.y - startPosition.y);
    float initialDistanceY = distanceY;

    // Move on the Y axis and resize
    while (distanceY > 0.01f)
    {
        if(!cards[cardNum]){break;}
        cards[cardNum].transform.position = Vector3.MoveTowards(cards[cardNum].transform.position, targetYPosition, mutliSpeed * Time.deltaTime);
        distanceY = Mathf.Abs(target.y - cards[cardNum].transform.position.y);

        float sizeFactor = 1 - (distanceY / initialDistanceY);
        float newSize = Mathf.Lerp(initialSize, finalSize, sizeFactor);
        if(!multiKeepSize) { cards[cardNum].transform.localScale = new Vector3(newSize, newSize, newSize); }

        yield return null;
    }

    if(cards[cardNum]){cards[cardNum].transform.position = targetYPosition; // Ensure the object reaches the exact target Y position
    if(!multiKeepSize) { cards[cardNum].transform.localScale = new Vector3(finalSize, finalSize, finalSize); }// Ensure the object reaches the exact final size}
    }

    // Move on the X axis
    Vector3 targetXPosition = new Vector3(target.x, target.y, target.z);
    float distanceX = Mathf.Abs(target.x - cards[cardNum].transform.position.x);

    while (distanceX > 0.01f)
    {
        if(!cards[cardNum]){break;}
        cards[cardNum].transform.position = Vector3.MoveTowards(cards[cardNum].transform.position, targetXPosition, mutliSpeed * Time.deltaTime);
        distanceX = Mathf.Abs(target.x - cards[cardNum].transform.position.x);

        yield return null;
    }

    cards[cardNum].transform.position = targetXPosition; // Ensure the object reaches the exact target X position
}

}
