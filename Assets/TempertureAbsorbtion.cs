using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempertureAbsorbtion : MonoBehaviour
{

    public GameObject vehicle;
    
    public GameObject leftTempSensor;
    public GameObject rightTempSensor;

    public float AbsorbtionRate = 1;
    public float DispersionRate = 1;
    private float currentHeatValue;
    private float currentCoolingValue;
    private float currentInternalTemp;

    private float currentLeftTemp;
    private float currentRightTemp;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        currentHeatValue = vehicle.GetComponent<PlayerMovement>().heatValue;
        currentCoolingValue = vehicle.GetComponent<PlayerMovement>().coolingValue;

        currentInternalTemp = vehicle.GetComponent<Thermometer>().currentTemp;
       //currentLeftTemp = leftTempSensor.GetComponent<Thermometer>().currentTemp;
       // currentRightTemp = rightTempSensor.GetComponent<Thermometer>().currentTemp;

        if (currentHeatValue < currentInternalTemp)
        {
            currentHeatValue = currentHeatValue + AbsorbtionRate * currentInternalTemp / currentHeatValue;
        }
        else
        {
            currentHeatValue = currentHeatValue - DispersionRate;
        }

        vehicle.GetComponent<PlayerMovement>().heatValue = currentHeatValue;

        if (currentCoolingValue > currentInternalTemp)
        {
            currentCoolingValue = currentCoolingValue + AbsorbtionRate * currentInternalTemp / currentCoolingValue;
        }
        else
        {
            currentCoolingValue = currentCoolingValue - DispersionRate;
        }

        vehicle.GetComponent<PlayerMovement>().heatValue = Mathf.Clamp(currentHeatValue, 0, 100);
        vehicle.GetComponent<PlayerMovement>().coolingValue = Mathf.Clamp(currentCoolingValue, 0 , 100);
        
        

    }
}
