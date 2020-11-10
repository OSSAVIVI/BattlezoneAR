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

    static List<ARPlane> arPlanesTracking = new List<ARPlane>();
    //static List<ARPlane> arPlanesRemoved = new List<ARPlane>();

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
        //string message = "1. Here\n";
        //message += (arPlaneManager.trackables).ToString();
        //foreach (ARPlane plane in arPlaneManager.trackables)
        //{
        //    message += plane.transform.position.ToString() + '\n';
        //}
        //InGameLog.writeToLog(message);

        string message = "1. Here\n";
        //arPlanes.Add(args.added[0]);
        //message += (arPlanes).ToString();
        //message += (arPlaneManager.planeCount).ToString;
        //message += "ADDED: " + args.added.Count.ToString() + "\n";
        //message += "UPDATED: " + args.updated.Count.ToString() + "\n";
        //message += "DELETED: " + args.removed.Count.ToString() + "\n";

        //foreach (ARPlane plane in arPlaneManager.trackables)
        //{
        //    message += plane.transform.position.ToString() + '\n';
        //}

        for (int i = 0; i < args.added.Count; i++)
        {
            if (!arPlanesTracking.Contains(args.added[0]))
            {
                arPlanesTracking.Add(args.added[0]);
            }
        }

        for (int j = arPlanesTracking.Count - 1; j >= 0 ; j--)
        {
            if (arPlanesTracking[j].trackingState.ToString() != "Tracking")
            {
                //arPlanesAdded[j].SetActive(false);
                arPlanesTracking.Remove(arPlanesTracking[j]);
            } 
        }


        for (int j = 0; j < arPlanesTracking.Count; j++)
        {
            message += arPlanesTracking[j].transform.position.ToString() + ' ' + arPlanesTracking[j].trackingState.ToString() + '\n';
        }



        //for (int j = 0; j < arPlanesRemoved.Count; j++)
        //{
        //    message += "X" + arPlanesRemoved[j].transform.position.ToString() + ' ' + arPlanesRemoved[j].trackingState.ToString() + '\n';
        //}


        //for (int i = 0; i < args.removed.Count; i++)
        //{
        //    arPlanes.Add(args.removed[0]);
        //}

        //foreach (ARPlane plane in arPlaneManager.trackables)
        //{
        //    message += plane.transform.position.ToString() + '\n';
        //}
        //InGameLog.writeToLog(message);

        if (arPlanesTracking.Count > 0 && placedObject == null)
        {
            target = GameObject.FindWithTag("MainCamera");
            //targetRotation = target.transform.rotation;
            //targetPositionGround.position = target.transform.position;

            //InGameLog.writeToLog(target.transform.position.ToString() + "\n" + targetVectorGround.ToString());

            if (arPlanesTracking[0] != null)
            {
                System.Random randomSeed = new System.Random();
                int randomIndex = randomSeed.Next(0, arPlanesTracking.Count);
                ARPlane arPlane = arPlanesTracking[randomIndex];

                //List<Vector3> boundaryOut = new List<Vector3>();
                //arPlane.TryGetBoundary(boundaryOut);

                //message = arPlane.ToString() + "\n";
                //InGameLog.writeToLog(message);

                targetVectorGround = new Vector3(target.transform.position.x, arPlane.transform.position.y + 0.05f, target.transform.position.z);
                placedObject = Instantiate(placedPrefab, arPlane.transform.position, Quaternion.identity);
                placedObject.transform.LookAt(targetVectorGround);

                //ARPlane arPlane = args.added[0];
                //targetVectorGround = new Vector3(target.transform.position.x, arPlane.transform.position.y + 0.05f, target.transform.position.z);
                //placedObject = Instantiate(placedPrefab, arPlane.transform.position, Quaternion.identity);
                //placedObject.transform.LookAt(targetVectorGround);

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
