using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsMenuGUI : MonoBehaviour
{
    MainMenuManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBackButton()
    {
        manager.state = MenuState.main;
    }
}
