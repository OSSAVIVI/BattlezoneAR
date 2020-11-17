using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemyMover : MonoBehaviour
{
    public GameObject target;
    private Vector3 targetVectorARGround;
    private Vector3 enemyVectorARGround;
    public Rigidbody[] projectile;
    bool shotFired = false;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    void Start()
    {
        target = GameObject.FindWithTag("MainCamera");
    }

    public void Shoot()
    {
        Rigidbody shot = Instantiate(projectile[0], transform.position, transform.rotation);
        shot.AddForce(transform.forward * 1000f);
        SoundManagerScript.playShotSound();
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("MainCamera");

        // This makes the enemy tank "look" at the player in regard to the x, z axis
        targetVectorARGround = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetVectorARGround - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);

        // This makes the enemy tanks move toward the player
        if (Vector3.Distance(targetVectorARGround, transform.position) > 0.5f && (Physics.Raycast(transform.position, Vector3.down, 0.1f)))
        {
            // NEED TO ADD IN DETECTION FOR WHEN THE ENEMY SPAWNS SLIGHTLY INSIDE PLANE
            // Can't just look up, because it hits itself
            // Need to check what it is hitting slightly both up and down and confirm that it is an AR plane, not itself
            transform.position += transform.forward * 0.1f * Time.deltaTime;
        }

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
