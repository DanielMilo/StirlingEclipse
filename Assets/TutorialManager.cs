using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TutorialStep
{
    resources, zones, drawback, finish
}

public class TutorialManager : MonoBehaviour
{
    [SerializeField] TutorialWindow tutorialGUI;
    [SerializeField] GameObject finishZone;

    [HideInInspector] public TutorialStep step;

    Craft player;
    GameController controller;

    float pickupTimer = 0;
    float zoneTimer = 0;
    float drawbackTimer = 0;


    // Start is called before the first frame update
    void Start()
    {
        step = TutorialStep.resources;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.gameState = GameState.tutorial;
        finishZone.SetActive(false);
    }

    void Update()
    {

        if (tutorialGUI)
        {
            CheckObjective();
            if (tutorialGUI.currentInfoIndex < tutorialGUI.getCurrentSize())
            {
                Time.timeScale = 0;
            }
            else
            {
                if (controller.gameState != GameState.menuActive)
                {
                    Time.timeScale = 1;
                }
            }

            if (step == TutorialStep.finish)
            {
                finishZone.SetActive(true);
            }
        }
    }

    void CheckObjective()
    {
        switch(step)
        {
            case TutorialStep.resources:
                if(player.engine.heatValue > 30 && player.engine.coolingValue > 30)
                {
                    pickupTimer += Time.deltaTime;
                    if(pickupTimer > 1f)
                    {
                        step = TutorialStep.zones;
                        tutorialGUI.currentInfoIndex = 0;
                    }
                }
                break;

            case TutorialStep.zones:
                if(zoneTimer > 3f)
                {
                    step = TutorialStep.drawback;
                    tutorialGUI.currentInfoIndex = 0;
                }
                break;

            case TutorialStep.drawback:
                if(player.engine.coolingValue == 0f || player.engine.heatValue == 0f)
                {
                    step = TutorialStep.finish;
                    tutorialGUI.currentInfoIndex = 0;
                    finishZone.SetActive(true);
                }
                break;
        }
    }

    public void OnPlayerTrigger(Collider other)
    {
        if(step == TutorialStep.zones && other.tag == "temperatureZone")
        {
            TempZone zone = other.GetComponentInParent<TempZone>();
            if(other == zone.collectionZone)
            {
                zoneTimer += Time.deltaTime;
            }
        }

        if(step == TutorialStep.drawback && other.tag == "temperatureZone")
        {
            TempZone zone = other.GetComponentInParent<TempZone>();
            if(other == zone.drawbackZone)
            {
                drawbackTimer += Time.deltaTime;
            }
        }
    }
}
