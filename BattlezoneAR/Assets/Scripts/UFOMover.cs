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
        transform.RotateAround(transform.position, transform.up, Time.deltaTime * 90f);
        if (Vector3.Distance(transform.position, target.transform.position) > 5f)
        {
            transform.position += transform.forward * 1f * Time.deltaTime;
        }
    }
}
