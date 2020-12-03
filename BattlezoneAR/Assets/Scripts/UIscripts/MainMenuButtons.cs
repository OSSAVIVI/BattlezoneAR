using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject helpMenu;
    public GameObject deleteText;
    public GameObject noButton;
    public GameObject yesButton;
    public GameObject switchShootSideButton;
    public GameObject switchShootSideButtonText;

    private readonly string confirmationMessage = "are you sure you want to delete all high scores?";
    private readonly string successMessage = "successfully deleted high score data.";
    private readonly string rightButtonText = "shoot button: right side";
    private readonly string leftButtonText = "shoot button: left side";

    private void Start()
    {
        mainMenu.SetActive(true);
        helpMenu.SetActive(false);
        noButton.SetActive(false);
        yesButton.SetActive(false);
        deleteText.SetActive(false);
        deleteText.GetComponent<Text>().text = confirmationMessage;
        if (PlayerPrefs.GetInt("buttonLeft") == 1)
            switchShootSideButtonText.GetComponent<Text>().text = leftButtonText;
        else
            switchShootSideButtonText.GetComponent<Text>().text = rightButtonText;
    }

    public void help() {
        mainMenu.SetActive(false);
        helpMenu.SetActive(true);
    }

    public void back(){
        mainMenu.SetActive(true);
        helpMenu.SetActive(false);
        noButton.SetActive(false);
        yesButton.SetActive(false);
        deleteText.SetActive(false);
        deleteText.GetComponent<Text>().text = confirmationMessage;
        switchShootSideButton.SetActive(true);
    }

    public void DeleteHighScores()
    {
        deleteText.GetComponent<Text>().text = confirmationMessage;
        deleteText.SetActive(true);
        noButton.SetActive(true);
        yesButton.SetActive(true);
        switchShootSideButton.SetActive(false);
    }
    public void NoDelete()
    {
        deleteText.SetActive(false);
        noButton.SetActive(false);
        yesButton.SetActive(false);
        switchShootSideButton.SetActive(true);
    }
    public void YesDelete()
    {
        PlayerPrefs.SetInt("FirstTimeEverPlaying", 0); //this is all you need to do to trigger the high scores to be reset in HighScores.Start()
        noButton.SetActive(false);
        yesButton.SetActive(false);
        deleteText.GetComponent<Text>().text = successMessage;
    }

    public void ChangeButtonSide()
    {
        if (PlayerPrefs.GetInt("buttonRight") == 1)
        {
            PlayerPrefs.SetInt("buttonRight", 0);
            PlayerPrefs.SetInt("buttonLeft", 1);
            switchShootSideButtonText.GetComponent<Text>().text = leftButtonText;
        }
        else if (PlayerPrefs.GetInt("buttonLeft") == 1)
        {
            PlayerPrefs.SetInt("buttonLeft", 0);
            PlayerPrefs.SetInt("buttonRight", 1);
            switchShootSideButtonText.GetComponent<Text>().text = rightButtonText;
        }
        else //if player chooses button before ever playing then neither will be set, so just set it to right
        {
            PlayerPrefs.SetInt("buttonLeft", 0);
            PlayerPrefs.SetInt("buttonRight", 1);
            switchShootSideButtonText.GetComponent<Text>().text = rightButtonText;
        }
    }
}

