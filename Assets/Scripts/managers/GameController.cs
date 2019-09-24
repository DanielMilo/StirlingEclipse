using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    startup, running, menuActive, gameOver, tutorial
}

public class GameController : MonoBehaviour
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
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }

        if(gameState != GameState.menuActive)
        {
            //Time.timeScale = 1;
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

            case GameState.gameOver:
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

    public void ToggleMenu()
    {
        if(gameState != GameState.menuActive)
        {
            stateBeforeMenu = gameState;
            gameState = GameState.menuActive;
            Time.timeScale = 0;
        }
        else
        {
            gameState = stateBeforeMenu;
            Time.timeScale = 1;
        }
    }

    public void LoadMainMenu()
    {
        DontDestroyOnLoad(data.gameObject);
        SceneManager.LoadScene("MainMenu");
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
        if(submitScoresEnabled)
        {
            networking.SubmitNewScore(player.name, levelTimer);
        }
        Score s = new Score();
        s.inserterID = player.name;
        s.scoreData.time = levelTimer;
        networking.scoreList.Add(s);
        networking.scoreList.Sort((x, y) => x.scoreData.time.CompareTo(y.scoreData.time));
        gameState = GameState.gameOver;
    }

    void OnDeath()
    {
        if(submitGhostsEnabled)
        {
            networking.SubmitNewGhost(player.name, player.transform.position, player.transform.rotation);
        }
        gameState = GameState.gameOver;
    }

    public void ReloadLevel()
    {
        DontDestroyOnLoad(data.gameObject);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}


