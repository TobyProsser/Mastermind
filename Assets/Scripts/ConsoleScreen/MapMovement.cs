using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMovement : MonoBehaviour
{
    public GameObject player;
    public float mapMoveSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(player.transform.localPosition.x > 350)
        {
            print("player X difference: " + (player.transform.localPosition.x - 350));
            print(this.transform.localPosition);
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - mapMoveSpeed, this.transform.localPosition.y ,this.transform.localPosition.z);
        }
        else if (player.transform.localPosition.x < -350){
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + mapMoveSpeed, this.transform.localPosition.y ,this.transform.localPosition.z);
        }

        if(player.transform.localPosition.y > 850)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y-mapMoveSpeed ,this.transform.localPosition.z);
        }
        else if (player.transform.localPosition.y < -330){
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y + mapMoveSpeed ,this.transform.localPosition.z);
        }
    }
}
