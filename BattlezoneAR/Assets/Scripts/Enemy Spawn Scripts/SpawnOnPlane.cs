using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// Adds ARRaycastManager class to any object with this script
//[RequireComponent(typeof(ARRaycastManager))]
[RequireComponent(typeof(ARPlaneManager))]
public class SpawnOnPlane : MonoBehaviour
{
    //private ARRaycastManager raycastManager;
    //private GameObject enemyObject;
    //public Transform arPosition;

    //[SerializeField]
    //private GameObject placeablePrefab;

    //static List<ARRaycastHit> raycastHits = new List<ARRaycastHit>();

    //public Vector2 touchPosition;
    //public Vector2 cameraViewPosition;

    [SerializeField]
    private GameObject placedPrefab;
    private GameObject placedObject;

    [SerializeField]
    private ARPlaneManager arPlaneManager;

    private GameObject target;
    //private Quaternion targetRotation;
    private Vector3 targetVectorGround;


    private void Awake()
    {
        //raycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;

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

    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        if ((args.added != null 
            || args.updated != null)
            && placedObject == null
            )
        {
            target = GameObject.FindWithTag("MainCamera");
            //targetRotation = target.transform.rotation;
            //targetPositionGround.position = target.transform.position;

            //InGameLog.writeToLog(target.transform.position.ToString() + "\n" + targetVectorGround.ToString());

            if (args.added[0] != null)
            {
                ARPlane arPlane = args.added[0];
                targetVectorGround = new Vector3(target.transform.position.x, arPlane.transform.position.y + 0.05f, target.transform.position.z);
                placedObject = Instantiate(placedPrefab, arPlane.transform.position, Quaternion.identity);
                placedObject.transform.LookAt(targetVectorGround);
            } 
            //else if (args.updated != null)
            //{
            //    ARPlane arPlane = args.added[0];
            //    placedObject = Instantiate(placedPrefab, arPlane.transform.position, Quaternion.identity);
            //    placedObject.transform.LookAt(targetVectorGround);
            //}

        }
    }



    //// Gets where the user presses on the screen
    //bool TryGetTouchPosition(out Vector2 touchPosition)
    //{
    //    if (Input.touchCount > 0)
    //    {
    //        touchPosition = Input.GetTouch(0).position;
    //        return true;
    //    }

    //    touchPosition = default;
    //    return false;
    //}

    //private void Update()
    //{
    //    if(TryGetTouchPosition(out Vector2 touchPosition))
    //    {
    //        if (raycastManager.Raycast(touchPosition, raycastHits, UnityEngine.XR.ARSubsystems.TrackableType.PlaneWithinPolygon))
    //        {
    //            var hitPose = raycastHits[0].pose;
    //            enemyObject = Instantiate(placeablePrefab, hitPose.position, hitPose.rotation);
    //        }
    //    }
    //}

    //public Transform[] spawnPoints;
    //public GameObject[] enemyTank;
    //// Start is called before the first frame update
    //void Start()
    //{
    //    //spawnSpot = new Vector3(15,0,15);
    //    //GameObject player = GameObject.FindWithTag("Player");
    //    //Instantiate(newTank, spawnSpot, transform.rotation);

    //    StartCoroutine(PrimaryEnemySpawn());
    //}

    //IEnumerator PrimaryEnemySpawn()
    //{
    //    for (int i = 0; i < 3; i++)
    //    {
    //        Instantiate(enemyTank[i], spawnPoints[i].position, Quaternion.identity);
    //        yield return new WaitForSeconds(10);
    //    }

    //    StartCoroutine(PrimaryEnemySpawn());
    //}


}
