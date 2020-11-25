using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HighScores : MonoBehaviour
{
    public GameObject title;
    public static readonly int numberOfHighScores = 8;
    public GameObject[] names;
    public GameObject[] scores;
    private int initialNumber;
    private int initialLetterNumber;
    private readonly char[] initials = new char[3];
    public GameObject upArrow;
    public GameObject downArrow;
    private int isNewHighScore;
    private const int NUM_LETTERS_IN_ALPHABET = 26;

    private int flashRate;
    private float nextFlash;
    private bool flashOn;

    private void Start()
    {
        title.GetComponent<Text>().text = $"HIGH SCORE BOARD";
        flashRate = 1;
        nextFlash = -1f;
        flashOn = false;
        initialLetterNumber = 1; //1 for "A"
        for (int i = 0; i < 3; i++) initials[i] = 'A';
        if (PlayerPrefs.GetInt("FirstTimeEverPlaying") == 0)
        {
            PlayerPrefs.SetInt("FirstTimeEverPlaying", 1);
            for (int idx = 1; idx <= numberOfHighScores; idx++)
            {
                PlayerPrefs.SetInt($"HighScore{idx}", -1);
                PlayerPrefs.SetString($"HighName{idx}", "###");
            }
        }
        SetHighScores();
        isNewHighScore = IsNewHighScore();
        if (isNewHighScore < 0)
        {
            RemoveArrows();
            initialNumber = 3; //makes it so selecting the next select button proceeds to the title screen
        }
        else if (isNewHighScore > 0)
        {
            SetNewHighScores(isNewHighScore);
            initialNumber = 0;
        }
    }

    private void FixedUpdate()
    {
        if (Time.time > nextFlash && !flashOn)
        {
            nextFlash = Time.time + flashRate;
            FlashLetterOn(initialNumber);
        }
        else if (Time.time > nextFlash && flashOn)
        {
            nextFlash = Time.time + flashRate;
            FlashLetterOff();
        }
    }

    private void SetHighScores()
    {
        for (int idx = 1; idx <= numberOfHighScores; idx++)
        {
            scores[idx - 1].GetComponent<Text>().text = PlayerPrefs.GetInt($"HighScore{idx}").ToString();
            names[idx - 1].GetComponent<Text>().text = PlayerPrefs.GetString($"HighName{idx}");
        }
    }

    public int IsNewHighScore()
    {
        int indexOfNewHighScore = -1; //-1 is flag for no new high score
        for (int idx = numberOfHighScores; idx >= 1; idx--)
        { //discover the position of the new high score]
            if (PlayerPrefs.GetInt("PlayerScore") > PlayerPrefs.GetInt($"HighScore{idx}"))
            {
                title.GetComponent<Text>().text = $"NEW #{idx} HIGH SCORE!";
                indexOfNewHighScore = idx;
            }
        }
        return indexOfNewHighScore;
    }

    private void FlashLetterOn(int initialNum)
    {
        flashOn = true;
        if (initialNum == 0)
            names[isNewHighScore - 1].GetComponent<Text>().text = $"_{initials[1]}{initials[2]}";
        else if (initialNum == 1)
            names[isNewHighScore - 1].GetComponent<Text>().text = $"{initials[0]}_{initials[2]}";
        else if (initialNum == 2)
            names[isNewHighScore - 1].GetComponent<Text>().text = $"{initials[0]}{initials[1]}_";
        else
            names[isNewHighScore - 1].GetComponent<Text>().text = GetNewInitials();
    }
    private void FlashLetterOff()
    {
        flashOn = false;
        names[isNewHighScore - 1].GetComponent<Text>().text = GetNewInitials();
    }

    private void RemoveArrows()
    {
        upArrow.SetActive(false);
        downArrow.SetActive(false);
    }

    private void UpdateText()
    {
        initials[initialNumber] = (char)(initialLetterNumber + 64);
        names[isNewHighScore - 1].GetComponent<Text>().text = GetNewInitials();
    }

    public void SelectButton()
    {
        if (initialNumber < 3)
        {
            UpdateText();
            initialNumber++;
            initialLetterNumber = 1;
            if (initialNumber == 3)
            {
                RemoveArrows();
                PlayerPrefs.SetString($"HighName{isNewHighScore}", GetNewInitials());
            }
        }
        else
        {
            SceneManager.LoadScene(0); //load title screen
        }
    }

    public void UpArrowButton()
    {
        initialLetterNumber++;
        if (initialLetterNumber == NUM_LETTERS_IN_ALPHABET + 1)
            initialLetterNumber = 1;
        UpdateText();
    }

    public void DownArrowButton()
    {
        initialLetterNumber--;
        if (initialLetterNumber == 0)
            initialLetterNumber = NUM_LETTERS_IN_ALPHABET;
        UpdateText();
    }

    private void SetNewHighScores(int newIdx)
    {
        int lastScore = PlayerPrefs.GetInt("PlayerScore");
        string lastName = $"{initials[0]}{initials[1]}{initials[2]}"; //this will always just be AAA
        for (int idx = newIdx; idx <= numberOfHighScores; idx++)
        {
            int tempScore = PlayerPrefs.GetInt($"HighScore{idx}");
            PlayerPrefs.SetInt($"HighScore{idx}", lastScore);
            lastScore = tempScore;
            string tempName = PlayerPrefs.GetString($"HighName{idx}");
            PlayerPrefs.SetString($"HighName{idx}", lastName);
            lastName = tempName;
        }
        SetHighScores();
        PlayerPrefs.SetInt("PlayerScore", 0);
    }

    private string GetNewInitials()
    {
        return $"{initials[0]}{initials[1]}{initials[2]}";
    }
}
