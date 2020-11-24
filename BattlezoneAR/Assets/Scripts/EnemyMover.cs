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

    private Camera camera;

    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    void Start()
    {
        target = GameObject.FindWithTag("MainCamera");
        camera = target.GetComponent<Camera>();
    }

    //public void Shoot()
    //{
    //    Rigidbody shot = Instantiate(projectile[0], transform.position, transform.rotation);
    //    shot.AddForce(transform.forward * 1000f);
    //    SoundManagerScript.playShotSound();
    //}

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("MainCamera");
        targetVectorARGround = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        //// This makes the enemy tank "look" at the player in regard to the x, z axis
        Quaternion targetRotation = Quaternion.LookRotation(targetVectorARGround - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.25f * Time.deltaTime);

        // This makes the enemy tanks move toward the player
        if (Vector3.Distance(targetVectorARGround, transform.position) > 0.5f && (Physics.Raycast(transform.position, Vector3.down, 0.1f)))
        {
            // NEED TO ADD IN DETECTION FOR WHEN THE ENEMY SPAWNS SLIGHTLY INSIDE PLANE
            // Can't just look up, because it hits itself
            // Need to check what it is hitting slightly both up and down and confirm that it is an AR plane, not itself
            transform.position += transform.forward * 0.1f * Time.deltaTime;
        }

        string enemyAlert = "";

        // Determine where the target is in reference to the player
        Vector3 enemyPos = Quaternion.Inverse(target.transform.rotation) * (transform.position - target.transform.position);
        bool easilyViewable = false;

        Vector3 screenPoint = camera.WorldToViewportPoint(transform.position);
        if (screenPoint.z > 0 && screenPoint.x > 0.1 && screenPoint.x < 0.9 && screenPoint.y > 0.1 && screenPoint.y < 0.9)
        {
            easilyViewable = true;
        }
        else
        {
            easilyViewable = false;
        }

        if (Vector3.Distance(targetVectorARGround, transform.position) < 2f)
        {
            enemyAlert += "ENEMY IN RANGE";
        }

        if (!easilyViewable)
        {
            if (enemyPos.z < 0)
            {
                enemyAlert += "\n\nENEMY TO REAR";
            }
            else if (enemyPos.x > 0)
            {
                enemyAlert += "\n\nENEMY TO RIGHT";
            }
            else if (enemyPos.x < 0)
            {
                enemyAlert += "\n\nENEMY TO LEFT";
            }
        }

        AlertLog.write(enemyAlert);



        //// This tells us if the two objects are facing one another
        //Vector3 dirFromAtoB = (target.transform.position - transform.position).normalized;
        //float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);

        //if(dotProd > 0.95 && !shotFired){
        //    shotFired = true;
        //    Debug.Log("looking at");
        //    // ObjA is looking mostly towards ObjB
        //    //Shoot();
        //}
    }
}
