using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int health = 20;

    public float radius;
    public GameObject card;
    public int lootAmount;
    public List<int> itemsToDrop;

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
            Destroy(curCard.GetComponent<BoxCollider2D>());
            Destroy(curCard.GetComponent<Rigidbody2D>());

            curCard.AddComponent<BoxCollider>();
            curCard.GetComponent<BoxCollider>().size = new Vector3(10, 10, 10);
            curCard.transform.parent = this.transform.parent;
            curCard.transform.localPosition = spawnPosition;
            curCard.GetComponent<CardController>().thisCardsObject = CardManager.Instance.cardObjects[itemsToDrop[i]];
            curCard.transform.localScale = new Vector3(0.011f,0.011f,0.011f);
        }
    }
}
