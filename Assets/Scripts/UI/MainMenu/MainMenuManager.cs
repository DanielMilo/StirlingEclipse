using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MenuState
{
    main, levelSelect, settings, loadingInitiate, loadingInProgress
}

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] GameObject DataCarrierPrefab;

    [SerializeField] GameObject MainMenuGUI;
    [SerializeField] GameObject LevelSelectGUI;
    [SerializeField] GameObject SettingsGUI;
    [SerializeField] GameObject LoadingGUI;


    [HideInInspector] public MenuState state;
    [HideInInspector] public string[] sceneList;
    [HideInInspector] public int sceneIndex;
    [HideInInspector] public DataCarrier data;

    // Start is called before the first frame update
    void Awake()
    {
        SetupDataCarrier();
        state = MenuState.main;
        sceneIndex = 0;
        GetAllScenes();
    }

    // Update is called once per frame
    void Update()
    {
        MainMenuGUI.SetActive(state == MenuState.main);
        LevelSelectGUI.SetActive(state == MenuState.levelSelect);
        SettingsGUI.SetActive(state == MenuState.settings);
        LoadingGUI.SetActive(state == MenuState.loadingInitiate || state == MenuState.loadingInProgress);

        if(state == MenuState.loadingInitiate)
        {
            state = MenuState.loadingInProgress;
            DontDestroyOnLoad(data.gameObject);
            SceneManager.LoadScene(sceneList[sceneIndex]);
        }
    }

    void SetupDataCarrier()
    {
        GameObject dataObj = GameObject.FindGameObjectWithTag("data");

        if(dataObj != null)
        {
            data = dataObj.GetComponent<DataCarrier>();
        }
        else
        {
            dataObj = GameObject.Instantiate(DataCarrierPrefab);
            data = dataObj.GetComponent<DataCarrier>();
        }
    }

    void GetAllScenes()
    {
        sceneList = new string[SceneManager.sceneCountInBuildSettings];
        for(int index = 0; index < SceneManager.sceneCountInBuildSettings; index++)
        {
            string sceneName = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(index));
            sceneList[index] = sceneName;
        }
    }
}
