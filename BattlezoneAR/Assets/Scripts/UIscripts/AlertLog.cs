using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertLog : MonoBehaviour
{
    static Text textUI;
    // Start is called before the first frame update
    void Start()
    {
        textUI = GetComponent<Text>();
    }

    public static void write(string text)
    {
        textUI.text = text;
    }
}
