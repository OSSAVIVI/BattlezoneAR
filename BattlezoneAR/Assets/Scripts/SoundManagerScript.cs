using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip GunShot;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        GunShot = Resources.Load<AudioClip>("GunShot");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void playsound()
    {
        audioSrc.PlayOneShot(GunShot);
    }
}
