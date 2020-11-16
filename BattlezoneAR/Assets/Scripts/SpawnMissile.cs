using System.Collections;
using System.Collections.Generic;
using System.Security.Principal;
using System.Threading;
using UnityEngine;

public class SpawnMissile : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject[] enemyMissile;
    bool missileSpanwed = false;

    void Update()
    {
        if (PlayerPrefs.GetInt("PlayerScore") >= 10000 && !missileSpanwed)
        {
            StartCoroutine(PrimaryEnemySpawn());
            missileSpanwed = true;
        }
    }

    IEnumerator PrimaryEnemySpawn()
    {
        for (int i = 0; i < 3; i++)
        {
            Instantiate(enemyMissile[i], spawnPoints[i].position, Quaternion.identity);
            yield return new WaitForSeconds(10);
        }

        StartCoroutine(PrimaryEnemySpawn());
    }
}
