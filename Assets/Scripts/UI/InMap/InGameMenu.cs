using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{

    [SerializeField] GameObject window;

    [HideInInspector] public bool showLeaderboard;
    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        showLeaderboard = false;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(controller.gameState == GameState.menuActive)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(showLeaderboard)
                {
                    showLeaderboard = false;
                }
                else
                {
                    if(window.activeSelf)
                    {
                        controller.CloseMenu();
                        window.SetActive(false);
                    }
                }
            }

            window.SetActive(!showLeaderboard);
        }
        else
        {
            window.SetActive(false);
        }
    }

    public void OnResumeButton()
    {
        controller.CloseMenu();
    }

    public void OnLeaderboardButton()
    {
        showLeaderboard = true;
    }

    public void OnRestartButton()
    {
        controller.ReloadLevel();
    }

    public void OnMainMenuButton()
    {
        controller.LoadMainMenu();
    }

    public void OnQuitButton()
    {
        Application.Quit();
    }

    
}
