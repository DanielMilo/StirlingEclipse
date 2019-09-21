using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectGUI : MonoBehaviour
{

    [SerializeField] Image levelImage;
    [SerializeField] Text levelName;

    MainMenuManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<MainMenuManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(manager.sceneList.Length > 0)
        {
            levelName.text = manager.sceneList[manager.sceneIndex];
        }
    }

    public void OnForwardButton()
    {
        if(manager.sceneList.Length > 0)
        {
            manager.sceneIndex = Mathf.Clamp(manager.sceneIndex + 1, 0, manager.sceneList.Length - 1);
        }
    }

    public void OnBackwardButton()
    {
        if(manager.sceneList.Length > 0)
        {
            manager.sceneIndex = Mathf.Clamp(manager.sceneIndex - 1, 0, manager.sceneList.Length - 1);
        }
    }

    public void OnBackButton()
    {
        manager.state = MenuState.main;
    }

    public void OnPlayButton()
    {
        manager.state = MenuState.loadingInitiate;
    }
}
