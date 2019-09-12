using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureSource : MonoBehaviour
{

    public int TempIntensity;

    void Start()
    {
        // make life easier such that we dont need to worry about tagging the object correctly
        this.gameObject.tag = "heatsource";
    
    }

    // Update is called once per frame
    void Update()
    {
        //We dont need to do anything here unless some sort of change occurs over time 
    }
}
