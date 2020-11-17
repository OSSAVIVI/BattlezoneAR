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

    static List<ARPlane> arPlanesTracking;
    static List<ARPlane> arPlanesRemoved;

    [SerializeField]
    private GameObject placedPrefab;
    private GameObject placedObject;

    [SerializeField]
    public ARPlaneManager arPlaneManager;

    private GameObject target;
    //private Quaternion targetRotation;
    private Vector3 targetVectorGround;
    private Vector3 spawnOnARPlane;

    private string message = "";


    private void Awake()
    {
        //raycastManager = GetComponent<ARRaycastManager>();
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;
        target = GameObject.FindWithTag("MainCamera");

        arPlanesTracking = new List<ARPlane>();
        arPlanesRemoved = new List<ARPlane>();

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
        message = "\n";

        //int count = 0;
        //foreach (var plane in arPlaneManager.trackables)
        //{
        //    count++;
        //}

        //message = "COUNT: " + count + '\n';

        //InGameLog.writeToLog(message);
        //string message = "1. Here\n";
        //message += (arPlaneManager.trackables).ToString();
        //foreach (ARPlane plane in arPlaneManager.trackables)
        //{
        //    message += plane.transform.position.ToString() + '\n';
        //}
        //InGameLog.writeToLog(message);


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
            if (!arPlanesTracking.Contains(args.added[i]))
            {
                arPlanesTracking.Add(args.added[i]);
                args.added[i].destroyOnRemoval = true;
            }
        }

        for (int j = arPlanesTracking.Count - 1; j >= 0; j--)
        {
            if (arPlanesTracking[j].trackingState.ToString() != "Tracking")
            {
                
                arPlanesTracking[j].gameObject.SetActive(false);
                arPlanesRemoved.Add(arPlanesTracking[j]);
                arPlanesTracking.Remove(arPlanesTracking[j]);
            }
        }

        for (int j = arPlanesRemoved.Count - 1; j >= 0; j--)
        {
            if (arPlanesRemoved[j].trackingState.ToString() == "Tracking")
            {

                arPlanesRemoved[j].gameObject.SetActive(true);
                arPlanesTracking.Add(arPlanesRemoved[j]);
                arPlanesRemoved.Remove(arPlanesRemoved[j]);
            }
        }

        //for (int j = 0; j < arPlanesTracking.Count; j++)
        //{
        //    message += "CENT: " + arPlanesTracking[j].center.ToString() + '\n'
        //        + "ALI: " + arPlanesTracking[j].alignment.ToString() + '\n';
        //        foreach (var bound in arPlanesTracking[j].boundary)
        //        {
        //        message += "BOUND: " + bound.ToString() + '\n';
        //        }
        //        //+ "CENTIPS: " + arPlanesTracking[j].centerInPlaneSpace.ToString() + '\n'
        //        message += "EXTENTS: " + arPlanesTracking[j].extents.ToString() + '\n'
        //        //+ "STATE: " + arPlanesTracking[j].trackingState.ToString() 
        //        + '\n';
        //}



        //for (int j = 0; j < arPlanesRemoved.Count; j++)
        //{
        //    message += "X:" + "CENTER: " + arPlanesRemoved[j].center.ToString() + ' ' 
        //        + arPlanesRemoved[j].trackingState.ToString() + '\n';
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

                int limit = 0;
                do
                {
                    Vector3 min = arPlane.GetComponent<MeshFilter>().mesh.bounds.min;
                    Vector3 max = arPlane.GetComponent<MeshFilter>().mesh.bounds.max;

                    double rangeX = (double)max.x - (double)min.x;
                    double sampleX = randomSeed.NextDouble();
                    double scaledX = (sampleX * rangeX) + min.x;
                    float randX = (float)scaledX;

                    double rangeZ = (double)max.z - (double)min.z;
                    double sampleZ = randomSeed.NextDouble();
                    double scaledZ = (sampleZ * rangeZ) + min.z;
                    float randZ = (float)scaledZ;


                    spawnOnARPlane = new Vector3(arPlane.center.x + randX, arPlane.center.y + 0.05f, arPlane.center.z + randZ);

                    message += "1: " + randX.ToString() + '\n'
                    + "2: " + min.y.ToString() + '\n'
                    + "2.1: " + max.y.ToString() + '\n'
                    + "3: " + randZ.ToString() + '\n'
                    + "min: " + min.ToString() + '\n'
                    + "max: " + max.ToString() + '\n'
                    + "center" + arPlane.center + '\n'
                    + "IN PLANE: " + Physics.Raycast(spawnOnARPlane, Vector3.down, 0.1f).ToString();

                    InGameLog.writeToLog(message);


                    limit++;
                } while (!(Physics.Raycast(spawnOnARPlane, Vector3.down, 0.1f)) && (limit < 50));



                placedObject = Instantiate(placedPrefab, spawnOnARPlane, Quaternion.identity);

                targetVectorGround = new Vector3(target.transform.position.x, arPlane.center.y + 0.05f, target.transform.position.z);
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
