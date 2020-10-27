using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int health;
    public GameObject healthDisplay;
    private float hitRate;
    private float nextHit;
    public GameObject hitFlash;
    private bool flashOn;
    private bool killSwitch;
    private int flashTime;
    private float nextFlash;

    void Start()
    {
        health = 10;
        healthDisplay.GetComponent<Text>().text = health.ToString() + " HP";
        hitRate = 0.5f;
        nextHit = -1f;
        hitFlash.SetActive(false);
        killSwitch = false;
        flashTime = 1;
        nextFlash = -1f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Time.time > nextHit && collision.gameObject.CompareTag("EnemyShot"))
        {
            nextHit = Time.time + hitRate;
            flashOn = true;
            health--;
            healthDisplay.GetComponent<Text>().text = health.ToString() + " HP";
            if (health <= 0)
            {
                ExplosionSoundScript.playShotSound();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //currently reloads scene instead of killing player
                //Destroy(gameObject);
            }
        }
    }

    private void FixedUpdate()
    { //Flash screen with cracked screen and red effect when damage is taken
        if (flashOn && killSwitch == false)
        {
            killSwitch = true;
            hitFlash.SetActive(true);
            nextFlash = Time.time + flashTime;
            print("DOOOH");
        }
        if (flashOn && killSwitch == true && Time.time > nextFlash)
        {
            flashOn = false;
            killSwitch = false;
            hitFlash.SetActive(false);
            print("sldjkahfksjdfhalskjd");
        }
    }
}
