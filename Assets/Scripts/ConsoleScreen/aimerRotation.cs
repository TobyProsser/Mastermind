using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AimerRotation : MonoBehaviour
{
    public GameObject imageToRotate;
    public GameObject targetImageToRotate;

    public GameObject sceneToHide;
    public Button rotateButton;
    public float rotationSpeed = 10f;
    public float detectionRadius = 10f;
    private float targetRotationZ;
    private bool isRotating = false;
    private float score;
    float angleToEnemy;

    GameObject enemy;

    void Start()
    {
        imageToRotate.gameObject.SetActive(false);
        targetImageToRotate.gameObject.SetActive(false);
    }

    void Update()
    {
        if (isRotating)
        {
            // Slowly rotate the image towards the target rotation on the Z-axis
            float currentZ = Mathf.MoveTowardsAngle(imageToRotate.transform.localEulerAngles.z, targetRotationZ, rotationSpeed * Time.deltaTime);
            imageToRotate.transform.localEulerAngles =  new Vector3(0, 0, currentZ);
        }
    }

    public void OnRotateButtonPressed()
    {
        imageToRotate.gameObject.SetActive(true);
        targetImageToRotate.gameObject.SetActive(true);

        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            isRotating = true;
            // Calculate the angle between the player and the enemy
            Vector3 directionToEnemy = nearestEnemy.transform.position - transform.position;
            angleToEnemy = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;
            // Set the target rotation to be within 90 degrees before and after the angle to the enemy
            targetRotationZ = angleToEnemy - 179f;
            targetImageToRotate.transform.localEulerAngles = new Vector3(0, 0, angleToEnemy);
            imageToRotate.transform.localEulerAngles =  new Vector3(0, 0, angleToEnemy);
            //print("angle to enewmt " + angleToEnemy + " TargetRotation " + targetRotationZ);
            enemy = nearestEnemy.gameObject;
        }
    }

    public void OnRotateButtonReleased()
    {
        int loadOnce = 0;
        imageToRotate.gameObject.SetActive(false);
        targetImageToRotate.gameObject.SetActive(false);
        isRotating = false;
        // Calculate the score based on the distance between the current rotation and the target rotation
        score = Mathf.Abs(Mathf.DeltaAngle(imageToRotate.transform.localEulerAngles.z, angleToEnemy));
        Debug.Log("Score: " + score);

        if(score >= 85 && score <= 95)
        {
            if(enemy){
                DeckSingleton.Instance.enemyHealth = enemy.GetComponent<EnemyController>().health;
                DeckSingleton.Instance.enemyName = enemy.GetComponent<EnemyController>().name;

                DeckSingleton.Instance.enemy = enemy;
                
                if(loadOnce == 0)
                {
                    
                    loadOnce++;
                    sceneToHide.SetActive(false);
                    print("LOAD SCENE");
                    SceneManager.LoadScene("PrepScene", LoadSceneMode.Additive);
                }
                
            }
        }
    }

    private GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float minDistance = detectionRadius;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy;
            }
        }

        return nearestEnemy;
    }
}