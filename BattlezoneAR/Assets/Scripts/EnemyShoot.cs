using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private float shootRate;
    private float nextShoot;
    private float shootForce;
    public Rigidbody[] projectile;
    public Transform shotSpawn;

    private void Start()
    {
        shootRate = Random.Range(5, 10); //Enemies can have shoot rate between 5 and 10 shots pers second
        nextShoot = -1f;
        shootForce = 1000f;
    }

    public void FixedUpdate()
    {
        if (Time.time > nextShoot)
        {
            nextShoot = Time.time + shootRate;
            Rigidbody shot = Instantiate(projectile[0], shotSpawn.position, shotSpawn.rotation);
            shot.AddForce(shotSpawn.forward * shootForce);
            SoundManagerScript.playShotSound();
        }
    }
}
