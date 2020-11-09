using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScore : MonoBehaviour
{
    public GameObject scoreUI;
    private float checkRate;
    private float nextCheck;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("PlayerScore", 0); //reset score every round. Can change logic if we end up adding more than one level
        checkRate = 0.25f;
        nextCheck = -1f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > nextCheck) //This is to slow down how often the script has to call PlayerRefs.GetInt("...")
        {
            nextCheck = Time.time + checkRate;
            scoreUI.GetComponent<Text>().text = $"PTS: { PlayerPrefs.GetInt("PlayerScore") }";
        }
    }
}