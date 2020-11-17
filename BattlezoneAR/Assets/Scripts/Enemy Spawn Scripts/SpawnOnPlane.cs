using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARPlaneManager))]
public class SpawnOnPlane : MonoBehaviour
{
    static List<ARPlane> arPlanesTracking;
    static List<ARPlane> arPlanesRemoved;

    [SerializeField]
    private GameObject placedPrefab;
    private GameObject placedObject;

    [SerializeField]
    public ARPlaneManager arPlaneManager;

    private GameObject target;
    private Vector3 targetVectorGround;
    private Vector3 spawnOnARPlane;

    private void Awake()
    {
        // Add function to event to track reliable AR planes
        arPlaneManager = GetComponent<ARPlaneManager>();
        arPlaneManager.planesChanged += PlaneChanged;

        // Create new lists for AR planes tracked in this scene
        arPlanesTracking = new List<ARPlane>();
        arPlanesRemoved = new List<ARPlane>();
    }

    private void Start()
    {
        // Start spawning tanks on AR planes
        StartCoroutine(SpawnTanksAR());
    }

    // Add and remove planes based on their tracking status
    private void PlaneChanged(ARPlanesChangedEventArgs args)
    {
        // Add planes to tracking and make sure they are destroyed
        // on removal
        for (int i = 0; i < args.added.Count; i++)
        {
            if (!arPlanesTracking.Contains(args.added[i]))
            {
                arPlanesTracking.Add(args.added[i]);
                args.added[i].destroyOnRemoval = true;
            }
        }

        // Don't track planes that aren't reliable, remove active
        for (int j = arPlanesTracking.Count - 1; j >= 0; j--)
        {
            if (arPlanesTracking[j].trackingState.ToString() != "Tracking")
            {
                arPlanesTracking[j].gameObject.SetActive(false);
                arPlanesRemoved.Add(arPlanesTracking[j]);
                arPlanesTracking.Remove(arPlanesTracking[j]);
            }
        }

        // Track planes that switch back to reliable, set active
        for (int j = arPlanesRemoved.Count - 1; j >= 0; j--)
        {
            if (arPlanesRemoved[j].trackingState.ToString() == "Tracking")
            {

                arPlanesRemoved[j].gameObject.SetActive(true);
                arPlanesTracking.Add(arPlanesRemoved[j]);
                arPlanesRemoved.Remove(arPlanesRemoved[j]);
            }
        }
    }

    // Spawn tanks onto planes during the scene
    IEnumerator SpawnTanksAR()
    {
        // If there are reliable AR planes and there is no tank
        if(arPlanesTracking.Count > 0)
        {
            if (arPlanesTracking[0] != null > 0 && placedObject == null)
            {
                // Get a random AR plane
                System.Random randomSeed = new System.Random();
                int randomIndex = randomSeed.Next(0, arPlanesTracking.Count);
                ARPlane arPlane = arPlanesTracking[randomIndex];

                // Find random valid spawn point above plane, if something
                // weird happens spawn in middle of plane
                int limit = 0;
                do
                {
                    if (limit == 5)
                    {
                        spawnOnARPlane = new Vector3(arPlane.center.x, arPlane.center.y + 0.05f, arPlane.center.z);
                    }
                    else
                    {
                        // Random x, z offset from center of AR plane
                        // May sometimes not be above actual plane
                        // Limit attempts to find point above plane to 4
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
                    }

                    limit++;

                } while (!(Physics.Raycast(spawnOnARPlane, Vector3.down, 0.1f)) && (limit < 5));

                // Place tank at AR spawn point
                placedObject = Instantiate(placedPrefab, spawnOnARPlane, Quaternion.identity);

                // Rotate tank to face player 
                target = GameObject.FindWithTag("MainCamera");
                targetVectorGround = new Vector3(target.transform.position.x, arPlane.center.y + 0.05f, target.transform.position.z);
                placedObject.transform.LookAt(targetVectorGround);
            }
        }

        // Wait two seconds then start again
        yield return new WaitForSeconds(5);
        StartCoroutine(SpawnTanksAR());
    }
}
