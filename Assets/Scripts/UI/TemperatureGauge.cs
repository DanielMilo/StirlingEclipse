using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TemperatureGauge:MonoBehaviour
{

    public Slider coolingSlider;
    public Slider speedSlider;
    public Slider heatSlider;

    public Text coolingText;
    public Text speedText;
    public Text heatText;
    Craft player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        coolingSlider.minValue = 0;
        coolingSlider.maxValue = 100;
        speedSlider.minValue = player.engine.minSpeed;
        speedSlider.maxValue = player.engine.maxSpeed;
        heatSlider.minValue = 0;
        heatSlider.maxValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSliders();
        UpdateText();
    }

    void UpdateSliders()
    {
        speedSlider.minValue = player.engine.minSpeed;
        speedSlider.maxValue = player.engine.maxSpeed;

        coolingSlider.value = player.engine.coolingValue;
        speedSlider.value = player.engine.CalculateEnginePower();
        heatSlider.value = player.engine.heatValue;
    }

    void UpdateText()
    {
        float speedPercentage = (player.engine.CalculateEnginePower() - player.engine.minSpeed) / (player.engine.maxSpeed - player.engine.minSpeed);
        coolingText.text = "" + (int)player.engine.coolingValue + "%";
        speedText.text = (int)(speedPercentage * 100) + "%";
        heatText.text = (int)player.engine.heatValue + "%";
    }
}
