using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject helpMenu;


    public void help() {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void back(){
        mainMenu.SetActive(true);
        helpMenu.SetActive(false);
    }


}

