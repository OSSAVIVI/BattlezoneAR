using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHealth : MonoBehaviour
{
    public int score;
    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("EnemyShot"))
        {
            ExplosionSoundScript.playShotSound();
            if (collision.gameObject.CompareTag("PlayerShot"))
            {
                PlayerPrefs.SetInt("PlayerScore", PlayerPrefs.GetInt("PlayerScore") + score);
            }
            Destroy(gameObject);
        }
    }
}
