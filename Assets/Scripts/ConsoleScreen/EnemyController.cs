using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float health = 15;

    public float radius;
    public GameObject card;
    public int lootAmount;
    public List<int> itemsToDrop;

void Start()
{
    //DropLoot();
}
    public void DropLoot()
    {
        for (int i = 0; i < lootAmount; i++)
        {
            // Get a random point inside the unit circle
        Vector2 randomPoint = new Vector2(this.transform.localPosition.x, this.transform.localPosition.y) + Random.insideUnitCircle * radius;

        // Convert the 2D point to a 3D point
        Vector3 spawnPosition = new Vector3(randomPoint.x, randomPoint.y, 0);

        // Instantiate the object at the random position
        GameObject curCard = Instantiate(card, this.transform.position, Quaternion.identity);
        curCard.transform.tag = "Collectable";

        // Destroy 2D components
        Destroy(curCard.GetComponent<BoxCollider2D>());
        Destroy(curCard.GetComponent<Rigidbody2D>());

         // Ensure destruction is processed before adding new component
        StartCoroutine(AddBoxColliderAfterFrame(curCard));
        Debug.Log("Coroutine started");

        curCard.transform.parent = this.transform.parent;
        curCard.transform.localPosition = spawnPosition;

        curCard.GetComponent<CardController>().thisCardsObject = CardManager.Instance.cardObjects[itemsToDrop[i]];
        curCard.transform.localScale = new Vector3(0.011f, 0.011f, 0.011f);
        }
    
    }
    public IEnumerator AddBoxColliderAfterFrame(GameObject curCard)
    {
        yield return new WaitForEndOfFrame();
        // Add BoxCollider component
        curCard.AddComponent<BoxCollider>();

        Destroy(this.gameObject);
    }
    
}
