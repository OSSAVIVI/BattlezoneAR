using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    public GameObject newTank;
    //public Vector3 spawnSpot = new Vector3(5,5,5);
    public Vector3 spawnSpot;

    // Start is called before the first frame update
    void Start()
    {
        spawnSpot = new Vector3(15,0,15);
        GameObject player = GameObject.FindWithTag("Player");
        Instantiate(newTank, spawnSpot, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
