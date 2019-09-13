using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempertureAbsorbtion : MonoBehaviour
{

    public GameObject vehicle;
    
   // public GameObject leftTempSensor;
   // public GameObject rightTempSensor;

    public float AbsorbtionRate = 1;
    public float DispersionRate = 1;
    public float FreezingPoint = 50;


    private float currentHeatValue;
    private float currentCoolingValue;
    private float currentInternalTemp;

    private float currentLeftTemp;
    private float currentRightTemp;

    // Update is called once per frame
    void Update()
    {

        currentHeatValue = vehicle.GetComponent<PlayerMovement>().heatValue;
        currentCoolingValue = vehicle.GetComponent<PlayerMovement>().coolingValue;

        currentInternalTemp = vehicle.GetComponent<Thermometer>().currentTemp;
       //currentLeftTemp = leftTempSensor.GetComponent<Thermometer>().currentTemp;
       // currentRightTemp = rightTempSensor.GetComponent<Thermometer>().currentTemp;

        if (FreezingPoint < currentInternalTemp)
        {
            currentHeatValue = currentHeatValue + AbsorbtionRate * currentInternalTemp;
        }
        else
        {
            currentHeatValue = currentHeatValue - DispersionRate;
        }

        vehicle.GetComponent<PlayerMovement>().heatValue = currentHeatValue;

        if (FreezingPoint > currentInternalTemp)
        {
            currentCoolingValue = currentCoolingValue + AbsorbtionRate * currentInternalTemp;
        }
        else
        {
            currentCoolingValue = currentCoolingValue - DispersionRate;
        }

        vehicle.GetComponent<PlayerMovement>().heatValue = Mathf.Clamp(currentHeatValue, 0, 100);
        vehicle.GetComponent<PlayerMovement>().coolingValue = Mathf.Clamp(currentCoolingValue, 0 , 100);
        
        

    }
}
