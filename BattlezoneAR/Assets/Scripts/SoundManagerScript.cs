using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip GunShot;
    //public static AudioClip Explosion;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        GunShot = Resources.Load<AudioClip>("GunShot");
        //Explosion = Resources.Load<AudioClip>("Explosion");
        audioSrc = GetComponent<AudioSource>();
    }

    public static void playShotSound()
    {
        audioSrc.PlayOneShot(GunShot);
    }
/*     public static void playExplosionSound()
    {
        audioSrc.PlayOneShot(Explosion);
    } */
}
