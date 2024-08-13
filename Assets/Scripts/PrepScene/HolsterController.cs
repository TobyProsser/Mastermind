using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolsterController : MonoBehaviour
{
    GameObject curCard;
    public Transform centerLocation;
    public float speed;
    float finalSize = .056f;
    float startSize = 0.155f;
    bool holdingCard;

    public bool acceptMultipleCards;

    Coroutine mouseChecker;

    public GameObject[] lights;
    public bool keepSize;
    void CardEntered(GameObject other){
        if(!holdingCard || acceptMultipleCards)
            {
                curCard = other.gameObject;
                DragAndDrop dragAndDrop = curCard.GetComponent<DragAndDrop>();
                if(!dragAndDrop.locked && !dragAndDrop.mouseDown)
                {
                    Rigidbody2D rb = curCard.GetComponent<Rigidbody2D>();
                    if(rb)
                    {
                        rb.simulated = false;
                        rb.velocity = Vector2.zero;
                    }
                    curCard.GetComponent<DragAndDrop>().locked = true;
                    holdingCard = true;
                    
                    StartCoroutine(MoveAndResizeObject(centerLocation.position, startSize, finalSize));
                    
                    SpawnNewCard();

                    HandleLights();
                }
                else if(dragAndDrop.mouseDown)
                {
                    mouseChecker = StartCoroutine(CheckForMouseUp(dragAndDrop));
                }
            }
            //If not being held by mouse and card isnt already in
            //Move card to child point centerLocation
    
    }

    private void HandleLights()
    {
        int time = curCard.GetComponent<CardController>().time;
        print("Time on card: " + time);
        for(int i = 0; i < 3; i++) {
            lights[i].SetActive(time == i);
        }
    }

    void SpawnNewCard(){
        TempCardTracker tracker = curCard.GetComponent<TempCardTracker>();
            if(tracker)
            {
                tracker.CardLocked();
            }
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Card")
        {
            CardEntered(other.gameObject);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject == curCard){
            if(mouseChecker != null){
                StopCoroutine(mouseChecker);
                mouseChecker = null;
            }
        }
    }

    IEnumerator CheckForMouseUp(DragAndDrop dragAndDrop)
    {
        while(true){
            
            yield return null;
            if(!dragAndDrop.mouseDown) {
                CardEntered(curCard);
                break;}
        }
    }

    IEnumerator MoveAndResizeObject(Vector3 target, float initialSize, float finalSize)
{
    Vector3 startPosition = curCard.transform.position;
    initialSize = curCard.transform.localScale.x;
    Vector3 targetYPosition = new Vector3(startPosition.x, target.y, startPosition.z);
    float distanceY = Mathf.Abs(target.y - startPosition.y);
    float initialDistanceY = distanceY;

    // Move on the Y axis and resize
    while (distanceY > 0.01f)
    {
        curCard.transform.position = Vector3.MoveTowards(curCard.transform.position, targetYPosition, speed * Time.deltaTime);
        distanceY = Mathf.Abs(target.y - curCard.transform.position.y);

        float sizeFactor = 1 - (distanceY / initialDistanceY);
        float newSize = Mathf.Lerp(initialSize, finalSize, sizeFactor);
        if(!keepSize) { curCard.transform.localScale = new Vector3(newSize, newSize, newSize); }

        yield return null;
    }

    curCard.transform.position = targetYPosition; // Ensure the object reaches the exact target Y position
    if(!keepSize) { curCard.transform.localScale = new Vector3(finalSize, finalSize, finalSize); }// Ensure the object reaches the exact final size

    // Move on the X axis
    Vector3 targetXPosition = new Vector3(target.x, target.y, target.z);
    float distanceX = Mathf.Abs(target.x - curCard.transform.position.x);

    while (distanceX > 0.01f)
    {
        curCard.transform.position = Vector3.MoveTowards(curCard.transform.position, targetXPosition, speed * Time.deltaTime);
        distanceX = Mathf.Abs(target.x - curCard.transform.position.x);

        yield return null;
    }

    curCard.transform.position = targetXPosition; // Ensure the object reaches the exact target X position
}

}
