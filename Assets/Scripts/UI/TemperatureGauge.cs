using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureGauge : MonoBehaviour {

    public Slider coolingSlider;
    public Slider speedSlider;
    public Slider heatSlider;

    public Text coolingText;
    public Text speedText;
    public Text heatText;
    public PlayerMovement player;

	// Use this for initialization
	void Start () {
        coolingSlider.minValue = 0;
        coolingSlider.maxValue = 100;
        speedSlider.minValue = player.minSpeed;
        speedSlider.maxValue = player.maxSpeed;
        heatSlider.minValue = 0;
        heatSlider.maxValue = 100;
    }
	
	// Update is called once per frame
	void Update () {
        UpdateSliders();
        UpdateText();
	}

    void UpdateSliders()
    {
        coolingSlider.value = player.coolingValue;
        speedSlider.value = player.CalculateEnginePower();
        heatSlider.value = player.heatValue;
    }

    void UpdateText()
    {
        float speedPercentage = (player.CalculateEnginePower() - player.minSpeed) / (player.maxSpeed - player.minSpeed);
        coolingText.text = "" + (int)player.coolingValue + "%";
        speedText.text = (int) (speedPercentage*100) + "%";
        heatText.text = (int)player.heatValue + "%";
    }
}
