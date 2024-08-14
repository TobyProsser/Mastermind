using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public float intensity;
    public bool locked;
    GameObject holster;
    bool slowing;
    private float initialValue = 1f; // Your initial value
    private float elapsedTime = 0f; // Elapsed time in seconds
    private float duration = 2f; // Total duration for interpolation (in seconds)


    public float dampening = 0.1f;
    private Vector3 offset;
    Rigidbody2D rb;

    public bool mouseDown;
    bool inMover;

    void Awake()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void OnMouseDown()
    {
        mouseDown = true;
        if (locked)
        {
            locked = !locked;
        }
        else
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            offset = transform.position - worldPosition;
        }
    }

    void OnMouseDrag()
    {
        if (!locked)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition) + offset;
            worldPosition.z = transform.position.z; // Keep the z position constant

            transform.position = Vector3.Lerp(transform.position, worldPosition, dampening);
        }
    }
    void OnMouseUp(){
        mouseDown = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Holster" && holster == null && !mouseDown)
        {
            holster = other.gameObject;
            
        } else if(other.gameObject.tag == "Mover"){
            
            inMover = true;
            StartCoroutine(MoveCoroutine());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (holster && other.gameObject == holster)
            {
                HolsterScript holsterScript = holster.GetComponent<HolsterScript>();
                if(holsterScript != null)
                {
                    GameObject holstersCard = holster.GetComponent<HolsterScript>().currentCard;
                if(holstersCard && holstersCard == this.gameObject)
                {
                    
                transform.gameObject.GetComponent<SpriteRenderer>().color = Color.yellow;
                holster.GetComponent<HolsterScript>().unlockCard();
                holster = null;
                }
                }
                
            }
            else if(other.gameObject.tag == "Mover"){
            rb.simulated = true;
            inMover = false;
        }

    }

    private IEnumerator MoveCoroutine()
    {
        bool moveRight = false; // Direction of movement
        float forceAmount = 2.0f; // Speed of movement
        rb.velocity = Vector2.zero;
        while (inMover)
        {
            if (moveRight)
            {
                rb.AddForce(Vector2.right * forceAmount);
            }
            else
            {
                rb.AddForce(Vector2.left * forceAmount);
            }
            yield return null; // Wait for the next frame
        }
    }
}


/*void OnMouseDown()
    {
        if (locked)
        {
            locked = !locked;
        }

    }
    private void OnMouseDrag()
    {
        if (!locked)
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
            float disToPlayer = Vector2.Distance(transform.position, worldPosition);
            Vector2 pullForce = (worldPosition - transform.position).normalized / disToPlayer * intensity;
            rb.AddForce(pullForce, ForceMode2D.Force);
        }
    }*/