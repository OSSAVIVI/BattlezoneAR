using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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

    public float turnSpeed;
    public float moveSpeed;

    private Collider2D enemyCollider;
    private int numColliders;
    private Collider2D[] collisionResults;
    public ContactFilter2D contactFilter;

    private bool facingClimbCenter = false;
    private bool facingClimbHeight = false;
    private bool turning = false;

    private Quaternion originalRotation;


    private void Awake()
    {
        arPlaneManager = GetComponent<ARPlaneManager>();
    }

    void Start()
    {
        target = GameObject.FindWithTag("MainCamera");
        camera = target.GetComponent<Camera>();

        enemyCollider = gameObject.GetComponent<Collider2D>();

        numColliders = 16;
        collisionResults = new Collider2D[numColliders];
        contactFilter = new ContactFilter2D().NoFilter();
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
        string message = "";
        message += "TANK: " + transform.position.ToString();
        // Check if there is a plane above the enemy
        Vector3 enemySkyPosition = new Vector3(transform.position.x, transform.position.y + 100.0f, transform.position.z);
        RaycastHit[] allSkyHits = Physics.RaycastAll(enemySkyPosition, Vector3.down);
        Vector3 highestARPlanePos = new Vector3(transform.position.x, -999.9f, transform.position.z);
        Vector3 highestARPlaneCenter = new Vector3(transform.position.x, -999.9f, transform.position.z);
        Vector3 highestARPlaneCenterGround = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        foreach (var hit in allSkyHits)
        {
            print(hit.collider.name);
            if (hit.collider.name.Length > 7 && hit.collider.name.Substring(0, 7) == "ARPlane" && highestARPlanePos.y < hit.point.y)
            {
                highestARPlanePos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                highestARPlaneCenter = new Vector3(
                    hit.collider.transform.position.x,
                    hit.collider.transform.position.y,
                    hit.collider.transform.position.z
                );
                highestARPlaneCenterGround = new Vector3(highestARPlaneCenter.x, transform.position.y, highestARPlaneCenter.z);
                //message += "\n" + hit.collider.gameObject.lastUpdatedFrame;
                //message += "\n" + hit.collider.gameObject.GetComponent<ARPlane>().center.ToString();
            }
        }

        message += "\nPLANE:" + highestARPlanePos.ToString() 
            + "\nCENTER:" + highestARPlaneCenter.ToString()
            +"\nCENTER GROUND:" + highestARPlaneCenterGround.ToString();

        // There is a plane above the enemy
        if (allSkyHits.Count() > 0 && transform.position.y < highestARPlanePos.y)
        {
            message += "\nPLANE ABOVE";
            message += "\n" + (highestARPlanePos.y - transform.position.y).ToString();
            if ((highestARPlanePos.y - transform.position.y) > .1 || facingClimbHeight == true)
            {

                float angle = 1;
                if (Vector3.Angle(transform.forward, highestARPlaneCenterGround - transform.position) > angle
                    && facingClimbCenter == false)
                {
                    Quaternion groundRotation = Quaternion.LookRotation(highestARPlaneCenterGround - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, groundRotation, turnSpeed * 1.2f * Time.deltaTime);
                }
                else
                {
                    // Save original rotation
                    if (facingClimbCenter == false)
                    {
                        originalRotation = transform.rotation;
                        facingClimbCenter = true;
                    }

                    message += "\nFACING";
                    // Rotate up towards plane
                    //Vector3 dirFromAtoB = (highestARPlaneCenter - transform.position).normalized;
                    //float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
                    if (Vector3.Angle(transform.forward, highestARPlaneCenter - transform.position) > angle
                        && facingClimbHeight == false)
                    {
                        Vector3 climbTo = new Vector3(highestARPlaneCenter.x, highestARPlaneCenter.y + 0.05f, highestARPlaneCenter.z);
                        Quaternion climbRotation = Quaternion.LookRotation(climbTo - transform.position);
                        transform.rotation = Quaternion.RotateTowards(transform.rotation, climbRotation, turnSpeed * 1.2f * Time.deltaTime);
                    }
                    // Looking up to plane, start climbing
                    else
                    {
                        facingClimbHeight = true;
                        transform.position += transform.forward * moveSpeed * 1.2f * Time.deltaTime;
                    }
                }
            } else
            {
                message += "\nTRANSPORTED";
                transform.position = new Vector3(highestARPlanePos.x, highestARPlanePos.y + 0.05f, highestARPlanePos.z);
            }
            // Rotate towards center of plane if not facing
            //Vector3 highestARPlaneCenterGround = new Vector3(highestARPlaneCenter.x, transform.position.y, highestARPlaneCenter.z);
            //Vector3 dirFromAtoB = (highARPlaneCenterGround - transform.position).normalized;
            //float dotProd = Vector3.Dot(dirFromAtoB, transform.forward);
            //if (dotProd > 0.95)
            //{
            print(Vector3.Angle(transform.forward, highestARPlaneCenterGround - transform.position));



            
        }
        // Climbed above plane or was already above
        // Check to make sure enemy is flat again
        else
        {
            message += "\n NO PLANE ABOVE";

            // Level enemy if not leveled from climbing
            float angle = 0.1f;
            if (facingClimbHeight == true && transform.rotation != originalRotation)
            {
                //Vector3 fallTo = new Vector3(highestARPlaneCenterGround.x, highestARPlaneCenterGround.y + 0.05f, highestARPlaneCenterGround.z);
                //Quaternion fallRotation = Quaternion.LookRotation(fallTo - transform.position);

                //transform.rotation = Quaternion.RotateTowards(transform.rotation, fallRotation, turnSpeed * Time.deltaTime);
                message += "\nFALLING";
                //transform.Rotate(45.0f, 0.0f, 0.0f);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, originalRotation, turnSpeed * 1.2f * Time.deltaTime);
                //transform.position += transform.forward * moveSpeed * Time.deltaTime;
            } else
            {
                facingClimbHeight = false;
                facingClimbCenter = false;

                target = GameObject.FindWithTag("MainCamera");
                targetVectorARGround = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

                //// This makes the enemy tank "look" at the player in regard to the x, z axis
                float turnAngle = 10;
                float stopAngle = 1;
                if (Vector3.Angle(transform.forward, targetVectorARGround - transform.position) > turnAngle)
                {
                    turning = true;
                }

                if (Vector3.Angle(transform.forward, targetVectorARGround - transform.position) < stopAngle)
                {
                    turning = false;
                }
                message = "COUNT: " + allSkyHits.Count().ToString() + "\n" + message;
                message = "DISTANCE: " + Vector3.Distance(targetVectorARGround, transform.position).ToString() + "\n" + message;
                message = "RAYCAST: " + (Physics.Raycast(transform.position, Vector3.down, 0.1f)).ToString() + "\n" + message;

                if (turning == true)
                {

                    message = "TURNING\n" + message;
                    Quaternion targetRotation = Quaternion.LookRotation(targetVectorARGround - transform.position);
                    transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                }
                else if (Vector3.Distance(targetVectorARGround, transform.position) > 0.5f && (Physics.Raycast(transform.position, Vector3.down, 0.1f)))
                {
                    message = "MOVING\n" + message;
                    // NEED TO ADD IN DETECTION FOR WHEN THE ENEMY SPAWNS SLIGHTLY INSIDE PLANE
                    // Can't just look up, because it hits itself
                    // Need to check what it is hitting slightly both up and down and confirm that it is an AR plane, not itself
                    transform.position += transform.forward * moveSpeed * Time.deltaTime;
                }

            }

            //// This makes the enemy tanks move toward the player
            //if (Vector3.Distance(targetVectorARGround, transform.position) > 0.5f && (Physics.Raycast(transform.position, Vector3.down, 0.1f)))
            //{
            //    // NEED TO ADD IN DETECTION FOR WHEN THE ENEMY SPAWNS SLIGHTLY INSIDE PLANE
            //    // Can't just look up, because it hits itself
            //    // Need to check what it is hitting slightly both up and down and confirm that it is an AR plane, not itself
            //    transform.position += transform.forward * moveSpeed * Time.deltaTime;
            //}
        }

        InGameLog.writeToLog(message);



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
