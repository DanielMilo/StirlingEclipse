using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGUI : MonoBehaviour
{
    [SerializeField] Text inputText;

    MainMenuManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuManager>();
        inputText.text = manager.data.playerName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButton()
    {
        if(!string.IsNullOrEmpty(inputText.text))
        {
            manager.state = MenuState.levelSelect;
            manager.data.playerName = inputText.text;
        }
    }

    public void OnSettingsButton()
    {
        manager.state = MenuState.settings;
    }

    public void OnQuitButton()
    {

    }
}
