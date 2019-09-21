using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{

    [SerializeField] GameObject window;

    GameController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        window.SetActive(controller.gameState == GameState.menuActive);
    }

    public void OnResumeButton()
    {
        controller.ToggleMenu();
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
