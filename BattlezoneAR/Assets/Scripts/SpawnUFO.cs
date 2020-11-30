using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

public class SpawnUFO : MonoBehaviour
{
    public GameObject enemyPrefab;
    private GameObject spawnedObject;
    public int spawnScore;
    public int waitTime;
    private bool routineStarted = false;
    bool firstSpawn = true;
    Vector3 spawnPoint;

    // Start is called before the first frame update
    void Update()
    {
        if (PlayerPrefs.GetInt("PlayerScore") >= spawnScore && routineStarted == false)
        {
            routineStarted = true;
            StartCoroutine(UFOSpawn());
        }
    }

    IEnumerator UFOSpawn()
    {
        // If no UFO spawned 
        if(spawnedObject == null)
        {
            if(!firstSpawn){
                // Buffer next spawn
                yield return new WaitForSeconds(waitTime);
            }
            firstSpawn = false;
            
            //Spawn UFO object in the direction the camera is facing
            spawnPoint = Camera.main.transform.forward*2;
            spawnPoint.y = 0.75f;
            spawnedObject = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
        }
        else
        {
            // Wait a bit before checking again
            yield return new WaitForSeconds(5);
        }

       StartCoroutine(UFOSpawn());
    }
}
