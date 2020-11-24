using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    private float shootRate;
    public float shootRateMin;
    public float shootRateMax;
    private float nextShoot;
    public float shootForce;
    public Rigidbody[] projectile;
    public Transform shotSpawn;

    private void Start()
    {
        shootRate = Random.Range(shootRateMin, shootRateMax);
        nextShoot = Time.time + shootRate;
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
