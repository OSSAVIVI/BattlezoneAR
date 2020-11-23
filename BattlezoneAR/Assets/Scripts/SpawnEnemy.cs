using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject enemyPrefab;
    private GameObject spawnedObject;
    public int spawnScore;
    public int waitTime;
    private bool routineStarted = false;

    // Start is called before the first frame update
    void Update()
    {
        if (PlayerPrefs.GetInt("PlayerScore") >= spawnScore && routineStarted == false)
        {
            // Spawn one the moment the spawn score is passed

            //Random seed 
            System.Random randomSeed = new System.Random();

            // Get random UFO spawn location
            int randomSpawnIndex = randomSeed.Next(0, spawnPoints.Count());
            spawnedObject = Instantiate(enemyPrefab, spawnPoints[randomSpawnIndex].transform.position, Quaternion.identity);

            // Start routine 
            routineStarted = true;
            StartCoroutine(UFOSpawn());
        }
    }

    IEnumerator UFOSpawn()
    {
        // If no UFO spawned 
        if(spawnedObject == null)
        {
            // Buffer next spawn
            yield return new WaitForSeconds(waitTime);

            //Random seed 
            System.Random randomSeed = new System.Random();

            // Get random UFO spawn location
            int randomSpawnIndex = randomSeed.Next(0, spawnPoints.Count());
            spawnedObject = Instantiate(enemyPrefab, spawnPoints[randomSpawnIndex].transform.position, Quaternion.identity);
        } // Currently spawned UFO
        else
        {
            // Wait a bit before checking again
            yield return new WaitForSeconds(5);
        }

        StartCoroutine(UFOSpawn());
    }
}
