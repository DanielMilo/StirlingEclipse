using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{

    public float time;
    Text text;

    // Start is called before the first frame update
    void Start()
    {
        time = 0;
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        text.text = parseTimeToText();
    }

    string parseTimeToText()
    {
        float tempTime = time;
        //hours
        int hours = (int) (time / (60 * 60));
        tempTime = time % (60 * 60);

        //minutes
        int minutes = (int)(time / (60));
        tempTime = tempTime % (60);

        //seconds
        int seconds = (int)tempTime;
        int milliseconds = (int)((tempTime % 1) * 100);

        //output
        string output = ajustNumberLength(hours) + ":" + ajustNumberLength(minutes) + ":" + ajustNumberLength(seconds) + "." +  ajustNumberLength(milliseconds);
        return output;
    }

    string ajustNumberLength(int x)
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

    //temporary
    void getTime()
    {

    }
}
