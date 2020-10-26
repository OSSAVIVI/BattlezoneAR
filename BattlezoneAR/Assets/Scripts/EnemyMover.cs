using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	//public GameObject player;
    public GameObject target;
    // Start is called before the first frame update
    void Start()
    {
        //target = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("MainCamera");

        if (Vector3.Distance(transform.position, target.transform.position) > 1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
            transform.position += transform.forward * 1f * Time.deltaTime;
        }
    }
}
