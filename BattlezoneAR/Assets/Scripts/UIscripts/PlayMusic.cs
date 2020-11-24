using UnityEngine;

public class PlayMusic : MonoBehaviour
{
    void Start()
    {
        GetComponent<AudioSource>().Play();
    }
}