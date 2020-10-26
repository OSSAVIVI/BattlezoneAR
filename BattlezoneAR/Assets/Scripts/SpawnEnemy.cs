using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    //public GameObject newTank;
    ////public Vector3 spawnSpot = new Vector3(5,5,5);
    //public Vector3 spawnSpot;

    public Transform[] spawnPoints;
    public GameObject[] enemyTank;

    // Start is called before the first frame update
    void Start()
    {
        //spawnSpot = new Vector3(15,0,15);
        //GameObject player = GameObject.FindWithTag("Player");
        //Instantiate(newTank, spawnSpot, transform.rotation);

        StartCoroutine(PrimaryEnemySpawn());
    }

    IEnumerator PrimaryEnemySpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyTank[i], spawnPoints[i].position, Quaternion.identity);
            yield return new WaitForSeconds(10);
        }

        StartCoroutine(PrimaryEnemySpawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
