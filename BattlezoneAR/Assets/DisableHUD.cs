using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableHUD : MonoBehaviour
{
    public string side;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt($"button{side}") == 0)
            gameObject.SetActive(false);
    }
}
