﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
	//public GameObject player;
    public GameObject target;
    // Start is called before the first frame update
    public Rigidbody[] projectile;
    //public Transform shotSpawn;
    bool shotFired = false;
    void Start()
    {
        //target = GameObject.FindWithTag("Player");
    }

    public void Shoot()
    {
        Rigidbody shot = Instantiate(projectile[0], transform.position, transform.rotation);
        shot.AddForce(transform.forward * 1500f);
        SoundManagerScript.playShotSound();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("MainCamera");

        // This makes the enemy tanks "look" at the player
        Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);

        // This makes the enemy tanks move toward the player
        if (Vector3.Distance(transform.position, target.transform.position) > 1.2f)
        {
            transform.position += transform.forward * 0.5f * Time.deltaTime;
        }
        //else if (Vector3.Distance(transform.position, target.transform.position) > 0f)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);
        //    //transform.position += transform.forward * 0.5f * Time.deltaTime;
        //}

        // This tells us if the two objects are facing one another

        Vector3 dirFromAtoB = (target.transform.position - transform.position).normalized;
        float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
 
        if(dotProd > 0.95 && !shotFired){
            shotFired = true;
            Debug.Log("looking at");
            // ObjA is looking mostly towards ObjB
            //Shoot();
        }
    }
}
