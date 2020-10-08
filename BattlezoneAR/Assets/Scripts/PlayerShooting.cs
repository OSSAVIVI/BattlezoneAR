using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    bool isShooting;
    public Rigidbody[] projectile;
    public Transform shotSpawn;
    private float fireRate;
    private float nextFire;
    private float shootForce;

    void Start()
    {
        isShooting = false;
        fireRate = 0.75f;
        nextFire = -1f;
        shootForce = 1500f;
    }

    void FixedUpdate()
    {
        if (isShooting)
        {
            Shoot();
        }
    }

    public void ToggleAttackOn()
    {
        print("FIRE BUTTON DOWN");
        isShooting = true;
    }

    public void ToggleAttackOff()
    {
        print("FIRE BUTTON UP");
        isShooting = false;
    }

    public void Shoot()
    {
        if (Time.time > nextFire)
        {
            print("FIRING SHOT!");
            nextFire = Time.time + fireRate;
            Rigidbody shot = Instantiate(projectile[0], shotSpawn.position, shotSpawn.rotation);
            shot.AddForce(shotSpawn.forward * shootForce);
        }
    }
}
