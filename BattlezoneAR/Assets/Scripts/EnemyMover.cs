//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemyMover : MonoBehaviour
{
	//public GameObject player;
    public GameObject target;
    private Vector3 targetVectorARGround;
    private Vector3 enemyVectorARGround;
    // Start is called before the first frame update
    public Rigidbody[] projectile;
    //public Transform shotSpawn;
    bool shotFired = false;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    private void Awake()
    {
        //raycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        //arPlaneManager.planesChanged += PlaneChanged;

        target = GameObject.FindWithTag("MainCamera");
        //targetRotation = target.transform.rotation;

        //Vector3 testPlayerPos = target.transform.position;
        //Vector3 testPlayerDir = target.transform.forward;
        //float spawnDis = 2;

        //Vector3 testSpawnPos = testPlayerPos + testPlayerDir*spawnDis;

        //print(targetRotation);

        //placedObject = Instantiate(placedPrefab, testSpawnPos, targetRotation);

        //ARPlane arPlane = args.added[0];
        //placedObject = Instantiate(placedPrefab, arPlane.transform.position, targetRotation);
    }

    void Start()
    {
        //target = GameObject.FindWithTag("Player");
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

        // This makes the enemy tanks "look" at the player
        //Quaternion targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
        //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);

        // This makes the enemy tank "look" at the player in regard to the x, z axis

        targetVectorARGround = new Vector3(target.transform.position.x, transform.position.y, target.transform.position.z);
        Quaternion targetRotation = Quaternion.LookRotation(targetVectorARGround - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 1 * Time.deltaTime);

        //var inPlane = false;

        //if()
        //{
        //    inPlane = true;
        //}
        //else
        //{
        //    inPlane = false;
        //}

        //InGameLog.writeToLog(inPlane.ToString());

        
        //List<ARRaycastHit> hitInfo = new List<ARRaycastHit>();
        //string message = "1. Here\n";
        //message += (transform.position.x).ToString() + " " + (transform.position.z).ToString() + "\n";
        //message += (target.transform.position.x).ToString() + " " + (target.transform.position.z).ToString() + "\n";
        

        //message += (Physics.Raycast(transform.position, Vector3.down, hitInfo, 0.1f).ToString() + "\n";
        //message += (Physics.Raycast(transform.position, Vector3.up, hitInfo, 0.1f).ToString() + "\n";

        // This makes the enemy tanks move toward the player
        if (
            Math.Abs(transform.position.x - target.transform.position.x) > 0.3f
            && Math.Abs(transform.position.z - target.transform.position.z) > 0.3f
            && (Physics.Raycast(transform.position, Vector3.down, 0.1f))
            )
        {
            // NEED TO ADD IN DETECTION FOR WHEN THE ENEMY SPAWNS SLIGHTLY INSIDE PLANE
            // Can't just look up, because it hits itself
            // Need to check what it is hitting slightly both up and down and confirm that it is an AR plane, not itself
            //message += "2. Here\n";
            //message += (Physics.Raycast(transform.position, Vector3.down, 0.1f)).ToString() + "\n" + (Physics.Raycast(transform.position, Vector3.up, 0.1f)).ToString();
            transform.position += transform.forward * 0.1f * Time.deltaTime;
        }

        //InGameLog.writeToLog(message);
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
