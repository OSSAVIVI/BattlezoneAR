using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMover : MonoBehaviour
{
    public GameObject target;
    bool stopRotation = false;

    void Start()
    {
        //target = GameObject.Find("HitPlane");
        //target = GameObject.FindWithTag("MainCamera");
        target = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        //target = GameObject.Find("MainCamera");

        // This makes the missiles "look" at the player and move toward the player
        //if (Vector3.Distance(transform.position, target.transform.position) > 0.2f)
        //{
        //Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 4f * Time.deltaTime);
        //}

        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            stopRotation = true;
            string message = "Within distance";
            InGameLog.writeToLog(message);
        }

        if (!stopRotation)
        {
            transform.LookAt(target.transform);
        }

        transform.position += transform.forward * 2f * Time.deltaTime;
    }
}
