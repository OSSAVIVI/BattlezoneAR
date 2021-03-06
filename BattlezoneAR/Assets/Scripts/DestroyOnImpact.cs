﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class DestroyOnImpact : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        print(collision.gameObject);
        print(gameObject);
        print("SHOT HAS MADE IMPACT!");
        string message = gameObject.name;
        message = message + transform.position.ToString();
        message = message + ", collided with: " + collision.gameObject;
        message = message + "\nCollision position/transform: " + collision.gameObject.transform.position;
        message = message + "\nBullet Size: " + transform.lossyScale.ToString();
        //InGameLog.writeToLog(message);
        Destroy(gameObject);
    }
}
