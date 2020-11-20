﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyObject;
    public int spawnScore;
    public int waitTime;
    bool enemySpanwed = false;

    // Start is called before the first frame update

    void Update()
    {
        if (PlayerPrefs.GetInt("PlayerScore") >= spawnScore && !enemySpanwed)
        {
            StartCoroutine(PrimaryEnemySpawn());
            enemySpanwed = true;
        }
    }
  //  IEnumerator PrimaryEnemySpawn()
   // {
        //for (int i = 0; i < 3; i++)
        //{
        //    Instantiate(enemyTank[i], spawnPoints[i].position, Quaternion.identity);
        //    yield return new WaitForSeconds(10);
        //}

       // StartCoroutine(PrimaryEnemySpawn());
   // }

    IEnumerator PrimaryEnemySpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyObject[i], spawnPoints[i].position, Quaternion.identity);
            yield return new WaitForSeconds(waitTime);
        }

        StartCoroutine(PrimaryEnemySpawn());
    }
}
