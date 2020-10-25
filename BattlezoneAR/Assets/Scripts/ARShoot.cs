using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARShoot : MonoBehaviour
{
    public Rigidbody[] projectile;
    public Transform shotSpawn;

    public void arShoot()
    {
        Rigidbody shot = Instantiate(projectile[0], shotSpawn.position, shotSpawn.rotation);
        shot.AddForce(shotSpawn.forward * 1500f);
        SoundManagerScript.playShotSound();
    }
}
