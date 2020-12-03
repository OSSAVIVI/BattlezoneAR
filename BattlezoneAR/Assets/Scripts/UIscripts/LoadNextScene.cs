using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LoadNext()
    {
        if (PlayerPrefs.GetInt("buttonLeft") == 0 && PlayerPrefs.GetInt("buttonRight") == 0)
        {
            PlayerPrefs.SetInt("buttonRight", 1); //by default, just choose right on first play
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
