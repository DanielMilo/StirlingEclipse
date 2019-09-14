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
    StirlingEngine playerEngine;

	// Use this for initialization
	void Start () {
        playerEngine = GameObject.FindGameObjectWithTag("Player").GetComponent<StirlingEngine>();
        coolingSlider.minValue = 0;
        coolingSlider.maxValue = 100;
        speedSlider.minValue = playerEngine.minSpeed;
        speedSlider.maxValue = playerEngine.maxSpeed;
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
        coolingSlider.value = playerEngine.coolingValue;
        speedSlider.value = playerEngine.CalculateEnginePower();
        heatSlider.value = playerEngine.heatValue;
    }

    void UpdateText()
    {
        float speedPercentage = (playerEngine.CalculateEnginePower() - playerEngine.minSpeed) / (playerEngine.maxSpeed - playerEngine.minSpeed);
        coolingText.text = "" + (int)playerEngine.coolingValue + "%";
        speedText.text = (int) (speedPercentage*100) + "%";
        heatText.text = (int)playerEngine.heatValue + "%";
    }
}
