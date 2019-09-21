using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    GameController controller;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = ParseTimeToText(controller.levelTimer);
    }

    string ParseTimeToText(float time)
    {
        //hours
        int hours = (int) (time / (60 * 60));
        time = time % (60 * 60);

        //minutes
        int minutes = (int)(time / (60));
        time = time % (60);

        //seconds
        int seconds = (int)time;
        int milliseconds = (int)((time % 1) * 100);

        //output
        string output = AjustNumberLength(hours) + ":" + AjustNumberLength(minutes) + ":" + AjustNumberLength(seconds) + "." +  AjustNumberLength(milliseconds);
        return output;
    }

    string AjustNumberLength(int x)
    {
        if(x < 10)
        {
            return "0" + x;
        }
        else
        {
            return "" + x;
        }
    }
}
