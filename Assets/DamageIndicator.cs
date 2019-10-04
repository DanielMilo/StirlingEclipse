using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageIndicator : MonoBehaviour
{

    [SerializeField] Image image;
    [SerializeField] float minFlashtime;
    [SerializeField] float maxFlashtime;
    [SerializeField] Color heat;
    [SerializeField] Color cold;

    Craft player;
    GameController controller;

    float timer;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        image.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.gameState == GameState.death)
        {
            image.gameObject.SetActive(true);
            return;
        }

        SetIndicatorColor();
        SetIndicatorState();
    }

    void SetIndicatorColor()
    {
        switch(player.criticalResource)
        {
            case Resource.heat:
                image.color = heat;
                break;
            case Resource.cold:
                image.color = cold;
                break;
        }
    }

    void SetIndicatorState()
    {
        Debug.Log(player.heatdeathTimerPercentage);
        if(player.heatdeathTimerPercentage > 0f)
        {
            timer += Time.deltaTime;

            Debug.Log(timer + "/" + GetTimeBetweenFlash());
            if(timer >= GetTimeBetweenFlash())
            {
                image.gameObject.SetActive(!image.gameObject.activeSelf);
                timer = 0;
            }
        }
        else
        {
            timer = 0;
            image.gameObject.SetActive(false);
        }
    }

    float GetTimeBetweenFlash()
    {
        float deltaFlashtime = Mathf.Abs(maxFlashtime - minFlashtime);
        return maxFlashtime - (deltaFlashtime * player.heatdeathTimerPercentage);
    }
}
