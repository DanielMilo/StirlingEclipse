using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    startup, running, menuActive, victory, death, tutorial
}

public class GameController:MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] bool submitGhostsEnabled;
    [SerializeField] bool submitScoresEnabled;

    Transform spawn;
    Driver driver;

    GameObject playerObject;
    Craft player;

    [HideInInspector] public NetworkingManager networking;
    [HideInInspector] public GameState gameState;
    [HideInInspector] public float levelTimer;
    DataCarrier data;
    GameState stateBeforeMenu;

    // Start is called before the first frame update
    void Awake()
    {
        Time.timeScale = 1;
        gameState = GameState.startup;

        SpawnPlayer();
        driver = GetComponent<Driver>();
        networking = GetComponent<NetworkingManager>();
        driver.steeringEnabled = true;
        levelTimer = 0f;

        GameObject dataObj = GameObject.FindGameObjectWithTag("data");

        if(dataObj != null)
        {
            data = dataObj.GetComponent<DataCarrier>();
            player.name = data.playerName;
            gameState = GameState.running;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(gameState != GameState.menuActive &&  Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("opening menu");
            OpenMenu();
        }

        switch(gameState)
        {
            case GameState.startup:
                driver.steeringEnabled = false;
                player.engine.enableFuelDecay = false;
                break;

            case GameState.running:
                UpdateTimer();
                CheckOnPlayer();
                driver.steeringEnabled = true;
                player.engine.enableFuelDecay = true;
                break;

            case GameState.victory:
                driver.steeringEnabled = false;
                player.engine.enableFuelDecay = false;
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    ReloadLevel();
                }
                break;

            case GameState.death:
                driver.steeringEnabled = false;
                player.engine.enableFuelDecay = false;
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    ReloadLevel();
                }
                break;

            case GameState.tutorial:
                UpdateTimer();
                CheckOnPlayer();
                driver.steeringEnabled = true;
                player.engine.enableFuelDecay = false;
                break;
        }
    }

    public void OpenMenu()
    {
        stateBeforeMenu = gameState;
        gameState = GameState.menuActive;
        Time.timeScale = 0;
    }

    public void CloseMenu()
    {
        gameState = stateBeforeMenu;
        Time.timeScale = 1;
    }


    void SpawnPlayer()
    {
        spawn = GameObject.FindGameObjectWithTag("Respawn").transform;
        playerObject = GameObject.Instantiate(playerPrefab, spawn.position, spawn.rotation);
        player = playerObject.GetComponent<Craft>();
        player.PutOnHoverHeight();
    }

    void UpdateTimer()
    {
        levelTimer += Time.deltaTime;
    }

    void CheckOnPlayer()
    {
        if(!player.isAlive)
        {
            OnDeath();
        }
        else if(player.hasWon)
        {
            OnVictory();
        }
    }

    public void FinishStartup()
    {
        if(gameState == GameState.startup)
            gameState = GameState.running;
    }

    void OnVictory()
    {
        networking.AddPlayerScore(levelTimer, submitScoresEnabled);

        gameState = GameState.victory;
    }

    void OnDeath()
    {
        if(submitGhostsEnabled)
        {
            networking.SubmitNewGhost(player.name, player.transform.position, player.transform.rotation);
        }
        gameState = GameState.death;
    }

    public void ReloadLevel()
    {
        DontDestroyOnLoad(data.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMainMenu()
    {
        DontDestroyOnLoad(data.gameObject);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadNextLevel()
    {
        DontDestroyOnLoad(data.gameObject);
        string sceneName = SceneManager.GetActiveScene().name;
        int nextBuildIndex = SceneManager.GetSceneByName(sceneName).buildIndex + 1;
        if(nextBuildIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetSceneByBuildIndex(nextBuildIndex).name);
        }
    }
}


