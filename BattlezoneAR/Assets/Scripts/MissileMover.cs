using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMover : MonoBehaviour
{
    public GameObject target;
    bool stopRotation = false;

    void Start()
    {
        target = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 0.2f)
        {
            stopRotation = true;
        }

        if (!stopRotation)
        {
            transform.LookAt(target.transform);
        }

        transform.position += transform.forward * 2f * Time.deltaTime;
    }
}
