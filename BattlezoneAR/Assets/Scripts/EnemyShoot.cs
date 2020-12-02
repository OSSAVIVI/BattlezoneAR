using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class EnemyShoot : MonoBehaviour
{
    private float shootRate;
    public float shootRateMin;
    public float shootRateMax;
    private float nextShoot;
    public float shootForce;
    public Rigidbody[] projectile;
    public Transform shotSpawn;

    private int playerScore;

    private void Start()
    {
        shootRate = UnityEngine.Random.Range(shootRateMin, shootRateMax);
        nextShoot = Time.time + shootRate;

        playerScore = PlayerPrefs.GetInt("PlayerScore");
        if (playerScore > 100000)
        {
            shootRateMin = shootRateMin * 0.5f;

            if (shootRateMin < 1.5f)
            {
                shootRateMin = 1.5f;
            }
        }
    }

    public void FixedUpdate()
    {
        if (Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;

            // Check if there is a plane above the enemy
            Vector3 enemySkyPosition = new Vector3(transform.position.x, transform.position.y + 100.0f, transform.position.z);
            RaycastHit[] allSkyHits = Physics.RaycastAll(enemySkyPosition, Vector3.down);
            Vector3 highestARPlanePos = new Vector3(transform.position.x, -999.9f, transform.position.z);
            Vector3 highestARPlaneCenter = new Vector3(transform.position.x, -999.9f, transform.position.z);
            Vector3 highestARPlaneCenterGround = new Vector3(transform.position.x, transform.position.y, transform.position.z);

            foreach (var hit in allSkyHits)
            {
                print(hit.collider.name);
                if (hit.collider.name.Length > 7 && hit.collider.name.Substring(0, 7) == "ARPlane" && highestARPlanePos.y < hit.point.y)
                {
                    highestARPlanePos = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                    highestARPlaneCenter = new Vector3(
                        hit.collider.transform.position.x,
                        hit.collider.transform.position.y,
                        hit.collider.transform.position.z
                    );
                    highestARPlaneCenterGround = new Vector3(highestARPlaneCenter.x, transform.position.y, highestARPlaneCenter.z);
                }
            }

            // There is no plane above the enemy
            if (!(allSkyHits.Count() > 0 && transform.position.y < highestARPlanePos.y))
            {
                Rigidbody shot = Instantiate(projectile[0], shotSpawn.position, shotSpawn.rotation);
                shot.AddForce(shotSpawn.forward * shootForce);
                SoundManagerScript.playShotSound();
            }
        }
    }
}
