using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public GameObject player;

    public float startMovingHorizontal;
    public float startMovingTop;
    public float startMovingBottom;
    public float mapMoveSpeed;

    public float maxHorizontal;
    public float maxHeight;
    public float maxBottom;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.localPosition.x > startMovingHorizontal && this.transform.localPosition.x > -maxHorizontal)
        {
            //print("player X difference: " + (player.transform.localPosition.x - startMovingHorizontal));
            //print(this.transform.localPosition);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - mapMoveSpeed, this.transform.localPosition.y ,this.transform.localPosition.z);
        }
        else if (player.transform.localPosition.x < -startMovingHorizontal && this.transform.localPosition.x < maxHorizontal){
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + mapMoveSpeed, this.transform.localPosition.y ,this.transform.localPosition.z);
        }

        if(player.transform.localPosition.y > startMovingTop && this.transform.localPosition.y > maxBottom)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y-mapMoveSpeed ,this.transform.localPosition.z);
        }
        else if (player.transform.localPosition.y < startMovingBottom && this.transform.localPosition.y < maxHeight){
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + mapMoveSpeed ,this.transform.localPosition.z);
        }
    }
}
