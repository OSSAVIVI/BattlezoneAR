﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayerHealth : MonoBehaviour
{
    public int health;
    private float hitRate;
    private float nextHit;  
    void Start()
    {
        hitRate = 0.1f;
        nextHit = -1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time > nextHit && collision.gameObject.CompareTag("PlayerShot"))
        {
            nextHit = Time.time + hitRate;
            health--;
            if (health <= 0)
            {
                ExplosionSoundScript.playShotSound();
                Destroy(gameObject);
            }
        }
    }
}
