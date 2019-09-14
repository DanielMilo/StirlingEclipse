﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StirlingEngine : MonoBehaviour
{
    public enum SpeedMode
    {
        min, additive, combiboost
    }

    public float minSpeed;
    public float maxSpeed;

    public SpeedMode speedMode;

    public float overFuelBoost; // percentage how much "unused/leftover" fuel gives in boost mode
    public float heatValue; // heat value between 0 to 100
    public float coolingValue; // cooling value between 0 to 100

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        heatValue = Mathf.Clamp(heatValue, 0f, 100f);
        coolingValue = Mathf.Clamp(coolingValue, 0f, 100f);
    }

    public float CalculateEnginePower()
    {
        float speedPercentage;
        switch(speedMode)
        {
            case SpeedMode.min: // min calculation
                speedPercentage = Mathf.Min(heatValue, coolingValue) / 100;
                break;
            case SpeedMode.additive: // additive
                speedPercentage = (heatValue + coolingValue) / (100 * 2);
                break;
            case SpeedMode.combiboost: // combined values give big boost
                float deltaFuel = Mathf.Abs(heatValue - coolingValue);
                speedPercentage = (Mathf.Min(heatValue, coolingValue) + deltaFuel * overFuelBoost) / 100;
                break;
            default:
                speedPercentage = 0f;
                break;
        }

        speedPercentage = Mathf.Clamp(speedPercentage, 0f, 1f);
        return minSpeed + (maxSpeed - minSpeed) * speedPercentage;
    }
}