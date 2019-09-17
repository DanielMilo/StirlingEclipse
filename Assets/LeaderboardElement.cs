using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardElement : MonoBehaviour
{
    [SerializeField] Text placeText;
    [SerializeField] Text nameText;
    [SerializeField] Text timeText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetData(int place, string name, float time)
    {
        placeText.text = place + ".";
        nameText.text = name;
        timeText.text = ParseTimeToText(time);
    }

    string ParseTimeToText(float time)
    {
        //hours
        int hours = (int)(time / (60 * 60));
        time = time % (60 * 60);

        //minutes
        int minutes = (int)(time / (60));
        time = time % (60);

        //seconds
        int seconds = (int)time;
        int milliseconds = (int)((time % 1) * 100);

        //output
        string output = AjustNumberLength(hours) + ":" + AjustNumberLength(minutes) + ":" + AjustNumberLength(seconds) + "." + AjustNumberLength(milliseconds);
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
