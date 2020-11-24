using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOMover : MonoBehaviour
{
    public GameObject target;

    void Start()
    {
        target = GameObject.Find("Target");
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 60f);
        //if (Vector3.Distance(transform.position, target.transform.position) > 3f)
        //{
            transform.position += transform.forward * 0.25f * Time.deltaTime;
        //}
    }
}
