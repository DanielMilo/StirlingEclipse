using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuGUI : MonoBehaviour
{
    [SerializeField] InputField inputField;
    [SerializeField] GameObject warningText;

    MainMenuManager manager;
    
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuManager>();
        inputField.text = manager.fileManager.data.name;
        inputField.Select();
        warningText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlayButton()
    {
        if(!string.IsNullOrEmpty(inputField.text))
        {
            manager.state = MenuState.levelSelect;
            manager.data.playerName = inputField.text;
            manager.fileManager.data.name = inputField.text;
            manager.fileManager.SaveData();
            warningText.SetActive(false);
        }
        else
        {
            warningText.SetActive(true);
        }
    }

    public void OnSettingsButton()
    {
        manager.state = MenuState.settings;
    }

    public void OnQuitButton()
    {
        Debug.Log("Shutting down application");
        Application.Quit();
    }
}
