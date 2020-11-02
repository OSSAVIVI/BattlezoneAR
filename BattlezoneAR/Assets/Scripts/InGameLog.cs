using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameLog : MonoBehaviour
{
    static Text textUI;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<Text>();
    }

    public static void writeToLog(string text)
    {
        textUI.text = text;
    }
}
