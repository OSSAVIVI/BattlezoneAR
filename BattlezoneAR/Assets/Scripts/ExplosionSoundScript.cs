using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionSoundScript : MonoBehaviour
{

    static AudioSource audioSource;
    static float delay= 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void playShotSound()
    {
        audioSource.PlayDelayed(delay);
    }
}
