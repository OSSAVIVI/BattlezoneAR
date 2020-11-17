﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyTank;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(PrimaryEnemySpawn());
    }

    IEnumerator PrimaryEnemySpawn()
    {
        //for (int i = 0; i < 3; i++)
        //{
        //    Instantiate(enemyTank[i], spawnPoints[i].position, Quaternion.identity);
            yield return new WaitForSeconds(10);
        //}

        StartCoroutine(PrimaryEnemySpawn());
    }
}
