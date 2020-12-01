using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
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
        // Check if there is a plane above the enemy
        Vector3 enemySkyPosition = new Vector3(transform.position.x, transform.position.y + 100.0f, transform.position.z);
        RaycastHit[] allSkyHits = Physics.RaycastAll(enemySkyPosition, Vector3.down);
        Vector3 highestARPlanePos = new Vector3(transform.position.x, -999.9f, transform.position.z);

        foreach (var hit in allSkyHits)
        {
            if(hit.collider.name.Substring(0,7) == "ARPlane" && highestARPlanePos.y < hit.point.y)
            {
                highestARPlanePos = new Vector3(transform.position.x, hit.point.y, transform.position.z); 
            }
        }

        if(transform.position.y < highestARPlanePos.y)
        {
            Vector3 climbTo = new Vector3(highestARPlanePos.x, highestARPlanePos.y + 0.05f, highestARPlanePos.z);
            Quaternion climbRotation = Quaternion.LookRotation(climbTo - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, climbRotation, turnSpeed * Time.deltaTime);
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }

        target = GameObject.FindWithTag("MainCamera");
        targetVectorARGround = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);

        //// This makes the enemy tank "look" at the player in regard to the x, z axis
        Quaternion targetRotation = Quaternion.LookRotation(targetVectorARGround - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);

        // This makes the enemy tanks move toward the player
        if (Vector3.Distance(targetVectorARGround, transform.position) > 0.5f && (Physics.Raycast(transform.position, Vector3.down, 0.1f)))
        {
            // NEED TO ADD IN DETECTION FOR WHEN THE ENEMY SPAWNS SLIGHTLY INSIDE PLANE
            // Can't just look up, because it hits itself
            // Need to check what it is hitting slightly both up and down and confirm that it is an AR plane, not itself
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
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
