using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureGauge : MonoBehaviour {

    public Slider heatSlider;
    public Slider coolingSlider;
    public Slider speedSlider;

    public PlayerMovement player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        heatSlider.minValue = 0;
        heatSlider.maxValue = 100;
        coolingSlider.minValue = 0;
        coolingSlider.maxValue = 100;
        speedSlider.minValue = player.minSpeed;
        speedSlider.maxValue = player.maxSpeed;

        heatSlider.value = player.heatValue;
        coolingSlider.value = player.coolingValue;
        speedSlider.value = player.CalculateSpeed();
	}
}
