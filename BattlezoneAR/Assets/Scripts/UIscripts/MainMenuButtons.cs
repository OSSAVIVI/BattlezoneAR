using UnityEngine;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject helpMenu;
    public GameObject deleteText;
    public GameObject noButton;
    public GameObject yesButton;

    private readonly string confirmationMessage = "Are you sure you want to delete all high scores?";
    private readonly string successMessage = "Successfully deleted high score data.";

    private void Start()
    {
        mainMenu.SetActive(true);
        helpMenu.SetActive(false);
        noButton.SetActive(false);
        yesButton.SetActive(false);
        deleteText.SetActive(false);
        deleteText.GetComponent<Text>().text = confirmationMessage;
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
    }

    public void DeleteHighScores()
    {
        deleteText.GetComponent<Text>().text = confirmationMessage;
        deleteText.SetActive(true);
        noButton.SetActive(true);
        yesButton.SetActive(true);
    }
    public void NoDelete()
    {
        deleteText.SetActive(false);
        noButton.SetActive(false);
        yesButton.SetActive(false);
    }
    public void YesDelete()
    {
        PlayerPrefs.SetInt("FirstTimeEverPlaying", 0); //this is all you need to do to trigger the high scores to be reset in HighScores.Start()
        noButton.SetActive(false);
        yesButton.SetActive(false);
        deleteText.GetComponent<Text>().text = successMessage;
    }
}

