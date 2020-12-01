using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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
    private Vector3 spawnARPosition;

    [SerializeField]
    public Transform[] enemySpawnPoints;

    [SerializeField]
    public GameObject[] enemyPrefabs;

    [SerializeField]
    public int[] spawnScores;

    private GameObject enemySpawnObject;

    private int enemyTier = 0;

    private Vector3 spawnPosition;

    private bool spawnOnARPlanes;

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
        spawnOnARPlanes = true;
        // Start spawning tanks on AR planes
        StartCoroutine(FindARPlanesAlert());
        //StartCoroutine(SpawnEnemiesAR());
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

    // Alert player while no planes are active
    IEnumerator FindARPlanesAlert()
    {
        // Remove enemy if things become disoriented 
        if(enemySpawnObject != null)
        {
            Destroy(enemySpawnObject);
        }

        string alertMessage = "";

        if (arPlanesTracking.Count > 0)
        {
            alertMessage = "";
            AlertLog.write(alertMessage);
            yield return new WaitForSeconds(0);
            StartCoroutine(SpawnEnemiesAR());
        } else
        {
            alertMessage = "LOOK AROUND SLOWLY FOR TANK PORTALS";
            AlertLog.write(alertMessage);
            yield return new WaitForSeconds(0.5f);

            alertMessage = "";
            AlertLog.write(alertMessage);
            yield return new WaitForSeconds(0.5f);
            StartCoroutine(FindARPlanesAlert());
        }
    }

    // Spawn tanks onto planes during the scene
    IEnumerator SpawnEnemiesAR()
    {
        // If there are reliable AR planes
        if(arPlanesTracking.Count > 0)
        {
            // If there is no current enemy spawn
            if (enemySpawnObject == null)
            {
                // Clear alert box
                string alertMessage = "";
                AlertLog.write(alertMessage);

                // Buffer next spawn
                yield return new WaitForSeconds(4);

                // Determine current enemy tier
                int currentScore = PlayerPrefs.GetInt("PlayerScore");
                while(enemyTier < spawnScores.Count() - 1 && currentScore >= spawnScores[enemyTier + 1])
                {
                    enemyTier++;
                }

                // Random seed
                System.Random randomSeed = new System.Random();

                // Get random enemy from available tiers
                int randomEnemyIndex = randomSeed.Next(0, enemyTier + 1);

                // Get spawn position
                // If AR plane spawn
                if((randomEnemyIndex == 0 || randomEnemyIndex == 2) && spawnOnARPlanes)
                {
                    // If we lost our plane, start looking again
                    if (!(arPlanesTracking.Count > 0))
                    {
                        yield return new WaitForSeconds(0.25f);
                        StartCoroutine(FindARPlanesAlert());
                    }

                    // Get a random AR plane
                    int randomPlaneIndex = randomSeed.Next(0, arPlanesTracking.Count);
                    ARPlane arPlane = arPlanesTracking[randomPlaneIndex];

                    // Random x, z offset from center of AR plane
                    // May sometimes not be above actual plane
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

                    spawnARPosition = new Vector3(arPlane.center.x + randX, arPlane.center.y + 0.05f, arPlane.center.z + randZ);
                    target = GameObject.FindWithTag("MainCamera");
                    targetVectorGround = new Vector3(target.transform.position.x, arPlane.center.y + 0.05f, target.transform.position.z);

                    // If the random point was not within the bounds, just put in the center of the plane
                    if (!Physics.Raycast(spawnARPosition, Vector3.down, 0.1f))
                    {
                        spawnARPosition = new Vector3(arPlane.center.x, arPlane.center.y + 0.05f, arPlane.center.z);
                        target = GameObject.FindWithTag("MainCamera");
                        targetVectorGround = new Vector3(target.transform.position.x, arPlane.center.y + 0.05f, target.transform.position.z);
                    }

                    spawnPosition = spawnARPosition;

                    //// Find random valid spawn point above plane, if something
                    //// weird happens spawn in middle of plane
                    //int limit = 0;
                    //do
                    //{
                    //    if (limit == 5)
                    //    {
                    //        spawnARPosition = new Vector3(arPlane.center.x, arPlane.center.y + 0.05f, arPlane.center.z);
                    //        target = GameObject.FindWithTag("MainCamera");
                    //        targetVectorGround = new Vector3(target.transform.position.x, arPlane.center.y + 0.05f, target.transform.position.z);
                    //    }
                    //    else
                    //    {
                    //        // Random x, z offset from center of AR plane
                    //        // May sometimes not be above actual plane
                    //        // Limit attempts to find point above plane to 4
                    //        Vector3 min = arPlane.GetComponent<MeshFilter>().mesh.bounds.min;
                    //        Vector3 max = arPlane.GetComponent<MeshFilter>().mesh.bounds.max;

                    //        double rangeX = (double)max.x - (double)min.x;
                    //        double sampleX = randomSeed.NextDouble();
                    //        double scaledX = (sampleX * rangeX) + min.x;
                    //        float randX = (float)scaledX;

                    //        double rangeZ = (double)max.z - (double)min.z;
                    //        double sampleZ = randomSeed.NextDouble();
                    //        double scaledZ = (sampleZ * rangeZ) + min.z;
                    //        float randZ = (float)scaledZ;

                    //        spawnARPosition = new Vector3(arPlane.center.x + randX, arPlane.center.y + 0.05f, arPlane.center.z + randZ);
                    //        target = GameObject.FindWithTag("MainCamera");
                    //        targetVectorGround = new Vector3(target.transform.position.x, arPlane.center.y + 0.05f, target.transform.position.z);
                    //    }

                    //    limit++;

                    //} while (!(Physics.Raycast(spawnARPosition, Vector3.down, 0.1f)) && (limit < 5));

                    //spawnPosition = spawnARPosition;
                } // If non-AR plane spawn
                else if (randomEnemyIndex == 1 || !spawnOnARPlanes)
                {
                    // Get a random spawn location from given positions
                    int randomSpawnIndex = randomSeed.Next(0, enemySpawnPoints.Count());
                    Transform enemySpawnPoint = enemySpawnPoints[randomSpawnIndex];
                    spawnPosition = new Vector3(enemySpawnPoint.position.x, enemySpawnPoint.position.y, enemySpawnPoint.position.z);
                }

                // Place enemy at spawn point
                enemySpawnObject = Instantiate(enemyPrefabs[randomEnemyIndex], spawnPosition, Quaternion.identity);

                // If AR spawn, rotate to face player
                if (randomEnemyIndex == 0)
                {
                    // Spawn with random direction
                    enemySpawnObject.transform.rotation = Quaternion.Euler(0, randomSeed.Next(0, 360), 0);
                    
                } 
                else if (randomEnemyIndex == 1)
                {
                    // Let the missile go for 8 seconds
                    int missileTimer = 8;
                    while (enemySpawnObject != null && missileTimer > 0)
                    {
                        yield return new WaitForSeconds(1f);
                        missileTimer--;
                    }

                    // If the missile was not destoryed after given time,
                    // destory the missile
                    if(enemySpawnObject != null)
                    {
                        Destroy(enemySpawnObject);
                    }
                }
                else if (randomEnemyIndex == 2)
                {
                    // Spawn facing player
                    //enemySpawnObject.transform.LookAt(targetVectorGround);
                    // Spawn with random direction
                    enemySpawnObject.transform.rotation = Quaternion.Euler(0, randomSeed.Next(0, 360), 0);
                }
            }

            // Wait some time then start again
            if (arPlanesTracking.Count > 0)
            {
                yield return new WaitForSeconds(0.5f);
                StartCoroutine(SpawnEnemiesAR());
            } else
            {
                yield return new WaitForSeconds(0);
                StartCoroutine(FindARPlanesAlert());
            }
        } // If there are no reliable AR planes
        else
        {
            yield return new WaitForSeconds(0);
            StartCoroutine(FindARPlanesAlert());
        }
    }
}
