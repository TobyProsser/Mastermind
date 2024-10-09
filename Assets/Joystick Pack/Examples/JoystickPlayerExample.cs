using System.Collections;
using System.Collections.Generic;
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

    public Transform player;
    bool dashing = false;
    float initSpeed;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        if(direction.x != 0){
            //print("Direction x, y: " + direction.x + ", " + direction.y);
            if(direction.x < 0 && this.transform.localPosition.x > maxX){
                //print("Min X reached: " + this.transform.localPosition.x);
                return;
            }else if(direction.x > 0 && this.transform.localPosition.x < minX){
                //print("Max X reached: " + this.transform.localPosition.x);
                return;
            }
        }

        if(this.transform.localPosition.y < maxTop || this.transform.localPosition.y > maxBottom){
            if(direction.y < 0 && this.transform.localPosition.y > maxBottom){

               // print("Min Y reached: " + this.transform.localPosition.y);                
                return;
            }else if(direction.y > 0 && this.transform.localPosition.y < maxTop){
               // print("Max Y reached: " + this.transform.localPosition.y);
                return;
            }
        }

        rb.AddForce(-direction * speed * Time.fixedDeltaTime, ForceMode.VelocityChange);

        // Rotate the player in the direction of movement
        if (direction != Vector3.zero)
        {
            player.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        }
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