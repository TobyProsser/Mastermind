using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolsterScript : MonoBehaviour
{
    public GameObject currentCard;
    Rigidbody2D rb;
    [SerializeField]
    public float intensity;
    public bool cardLocked;
    public float distToLock;

    public Vector3 startScale = new Vector3(0.44f, 0.44f, 0.44f);
    public Vector3 endScale = new Vector3(0.24f, 0.24f, 0.24f);
    public float duration = 1.5f;
    private float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void LateUpdate()
    {
        if(currentCard && !cardLocked)
        {
            float disToPlayer = Mathf.Abs(Vector2.Distance(currentCard.transform.position, transform.position));
            
            
            if(disToPlayer <= distToLock)
            {
                currentCard.transform.position = transform.position + new Vector3(0, 0, -1);

                
                currentCard.GetComponent<DragAndDrop>().locked = true;
                currentCard.GetComponent<SpriteRenderer>().color = Color.green;
                cardLocked = true;
                rb.velocity = Vector2.zero;
                rb.gravityScale = 0;
                transform.gameObject.GetComponent<SpriteRenderer>().color = Color.blue;
                return;
            }
            Vector2 pullForce = (transform.position - currentCard.transform.position).normalized / disToPlayer * intensity;
            rb.AddForce(pullForce, ForceMode2D.Force);
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Card")
        {
            if(!cardLocked){
            currentCard = other.gameObject;
            rb = currentCard.GetComponent<Rigidbody2D>();
            transform.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(currentCard && other.gameObject == currentCard)
        {
            if(cardLocked)
            {
                currentCard = null;
                cardLocked = false;
                transform.gameObject.GetComponent<SpriteRenderer>().color = Color.magenta;
            }
        }
    }

    public void unlockCard(){
        currentCard = null;
                cardLocked = false;
                transform.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
