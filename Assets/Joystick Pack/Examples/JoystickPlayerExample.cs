using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    public float speed;
    public VariableJoystick variableJoystick;
    public Rigidbody rb;

    public float maxXValues;
    public float maxTop;
    public float maxBottom;

    public Transform player;

    public void FixedUpdate()
    {
        Vector3 direction = Vector3.up * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;
        if(Mathf.Abs(this.transform.localPosition.x) > maxXValues){
            //print("Direction x, y: " + direction.x + ", " + direction.y);
            if(direction.x < 0 && this.transform.localPosition.x < -maxXValues){
                //print("Min X reached: " + this.transform.localPosition.x);
                return;
            }else if(direction.x > 0 && this.transform.localPosition.x > maxXValues){
                //print("Max X reached: " + this.transform.localPosition.x);
                return;
            }
        }

        if(this.transform.localPosition.y > maxTop || this.transform.localPosition.y < maxBottom){
            if(direction.y < 0 && this.transform.localPosition.y < maxBottom){

               // print("Min Y reached: " + this.transform.localPosition.y);                
                return;
            }else if(direction.y > 0 && this.transform.localPosition.y > maxTop){
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
}