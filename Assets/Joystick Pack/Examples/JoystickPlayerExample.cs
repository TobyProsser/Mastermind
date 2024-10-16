using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public float maxX;
    public float minX;
    public float maxTop;
    public float maxBottom;

    public GameObject playerObject;
    public Transform playerVisual;
    public Vector3 lockedPos;
    public float range;
    public Rigidbody playerRB;
    bool dashing = false;
    float initSpeed;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

        // Rotate the player in the direction of movement
        if (direction != Vector3.zero)
        {
            playerVisual.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }

        // //If moving on X axis
        if(direction.x != 0){
            //If moving Left and map is all the way too the left, move the player instead
            if(direction.x < 0 && this.transform.localPosition.x > maxX){
                MovePlayerX(direction.x);
                
            }//If moving Right and map is all the way too the Right, move the player instead
            else if(direction.x > 0 && this.transform.localPosition.x < minX){
                MovePlayerX(direction.x);
        }
        
        //If player is in right place than move map
        if(math.abs(playerObject.transform.localPosition.x - lockedPos.x) <= range){
            rb.AddForce(new Vector3(-direction.x * speed * Time.fixedDeltaTime, 0, 0), ForceMode.VelocityChange); // X-axis
        }
        //If player is too far left of right place and trying to move right then move player
        else if((playerObject.transform.localPosition.x < lockedPos.x) && direction.x > 0)
        {
            MovePlayerX(direction.x);
        }
        //If player is too far Right of right place and trying to move left then move player
        else if((playerObject.transform.localPosition.x > lockedPos.x) && direction.x < 0){
            MovePlayerX(direction.x);
        }
        }
        //If moving on Y Axis
        if(direction.y != 0){
            //If moving down and at or below maxBottom move player
            print("Current map height: " + this.transform.localPosition.y);
        if(direction.y < 0 && this.transform.localPosition.y >= maxBottom){

                print("Min Y reached: " + this.transform.localPosition.y);
                MovePlayerY(direction.y);
                return;
            }
            //If moving up and at max top move player
            else if(direction.y > 0 && this.transform.localPosition.y <= maxTop){
                print("Max Y reached: " + this.transform.localPosition.y);
                MovePlayerY(direction.y);
                return;
            }

        print("Player y " + playerObject.transform.localPosition.y + " locked pos y " + lockedPos.y);
        print("y Dist: " + math.abs(playerObject.transform.localPosition.y - lockedPos.y));

        //If player is in right place than move map
        if(math.abs(playerObject.transform.localPosition.y - lockedPos.y) <= range){
            rb.AddForce(new Vector3(0, -direction.y * speed * Time.fixedDeltaTime, 0), ForceMode.VelocityChange); //Y axis
            return;
        }
        //If player is too far Below of right place and trying to move up then move player
        else if((playerObject.transform.localPosition.y < lockedPos.y) && direction.y > 0)
        {
            MovePlayerY(direction.y);
            return;
        }
        //If player is too far Above of right place and trying to move down then move player
        else if((playerObject.transform.localPosition.y > lockedPos.y) && direction.y < 0){
            MovePlayerY(direction.y);
            return;
        }
        }

        

        
    }

    void MovePlayer(Vector3 direction){
        playerRB.AddForce(direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
    }

    void MovePlayerX(float xDirection)
{
    Vector3 forceX = new Vector3(xDirection, 0, 0);
    playerRB.AddForce(forceX * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
}

void MovePlayerY(float yDirection)
{
    Vector3 forceY = new Vector3(0, yDirection, 0);
    playerRB.AddForce(forceY * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);
}

    public void OnDashButtonPressed(){
        dashing = true;
        initSpeed = speed;

        speed *= 4f;
    }

    public void OnDashButtonReleased(){
        dashing = false;

        speed = initSpeed;
    }


    void Update()
    {
        // Detect right-click press
        if (Input.GetMouseButtonDown(1))
        {
            OnRightClickPress();
        }

        // Detect right-click release
        if (Input.GetMouseButtonUp(1))
        {
            OnRightClickRelease();
        }
    }

    void OnRightClickPress()
    {
        OnDashButtonPressed();
        // Add your logic for right-click press here
    }

    void OnRightClickRelease()
    {
        OnDashButtonReleased();
        // Add your logic for right-click release here
    }
}