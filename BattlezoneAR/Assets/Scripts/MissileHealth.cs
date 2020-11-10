using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHealth : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("EnemyShot"))
        {
            ExplosionSoundScript.playShotSound();
            Destroy(gameObject);
        }
    }
}
