using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARShoot : MonoBehaviour
{
    private bool isShooting;
    private float shootRate;
    private float nextShoot;
    private float shootForce;
    public Rigidbody[] projectile;
    public Transform shotSpawn;

    private void Start()
    {
        isShooting = false;
        shootRate = 1f;
        nextShoot = -1f;
        shootForce = 1500f;
    }

    public void BeginShooting()
    {
        isShooting = true;
    }

    public void StopShooting()
    {
        isShooting = false;
    }

    public void FixedUpdate()
    {
        if (isShooting && Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            Rigidbody shot = Instantiate(projectile[0], shotSpawn.position, shotSpawn.rotation);
            shot.AddForce(shotSpawn.forward * shootForce);
            SoundManagerScript.playShotSound();
        }
    }
}
