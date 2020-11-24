using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int health;
    public GameObject healthDisplay;
    private float hitRate;
    private float nextHit;
    public GameObject[] hitFlash;
    private bool flashOn;
    private bool killSwitch;
    private int flashTime;
    private float nextFlash;
    private int lastFlash;

    void Start()
    {
        health = 3;
        healthDisplay.GetComponent<Text>().text = health.ToString() + " HP";
        hitRate = 0.5f;
        nextHit = -1f;
        StopFlash();
        killSwitch = false;
        flashTime = 1;
        nextFlash = -1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Here");
        print(collision.gameObject);
        if (Time.time > nextHit && (collision.gameObject.CompareTag("EnemyShot") || collision.gameObject.CompareTag("Missile")))
        {
            nextHit = Time.time + hitRate;
            flashOn = true;
            health--;
            healthDisplay.GetComponent<Text>().text = health.ToString() + " HP";
            if (health <= 0)
            {
                ExplosionSoundScript.playShotSound();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
    }

    private void FixedUpdate()
    { //Flash screen with cracked screen and red effect when damage is taken
        if (flashOn && killSwitch == false)
        {
            killSwitch = true;
            ChooseFlash();
            nextFlash = Time.time + flashTime;
        }
        if (flashOn && killSwitch == true && Time.time > nextFlash)
        {
            flashOn = false;
            killSwitch = false;
            StopFlash();
        }
    }

    private void ChooseFlash()
    {
        var rand = Random.Range(0, 30);
        if (rand > 20) lastFlash = 2;
        else if (rand > 10) lastFlash = 1;
        else lastFlash = 0;
        hitFlash[lastFlash].SetActive(true);
    }

    private void StopFlash()
    {
        for (int i = 0; i < 3; i++)
            hitFlash[i].SetActive(false);
    }
}
