using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //public Joystick joystick;
    private float rotSpeed;
    private float rotX;
    private float rotY;
    private int minRotX;
    private int maxRotX;

    private void Start()
    {
        rotSpeed = 90f;
        rotX = 10f;
        minRotX = -10; //adjust the min/max if you think camera should move more up/down
        maxRotX = 10;
    }

    void FixedUpdate()
    {
        //if (joystick.Horizontal > 0f || joystick.Horizontal < 0f || joystick.Vertical > 0f || joystick.Vertical < 0f)
        //{
        //    rotX -= joystick.Vertical * Time.fixedDeltaTime * rotSpeed;
        //    rotY += joystick.Horizontal * Time.fixedDeltaTime * rotSpeed;
        //}
        ////Prevents camera from moving too far vertically up or down
        //if (rotX < minRotX) rotX = minRotX;
        //else if (rotX > maxRotX) rotX = maxRotX;
        //transform.rotation = Quaternion.Euler(rotX, rotY, 0);
    }
}
