using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public float levelTimer;

    // Start is called before the first frame update
    void Awake()
    {
        isGameOver = false;
        SpawnPlayer();
        driver = GetComponent<Driver>();
        networking = GetComponent<NetworkingManager>();
        driver.steeringEnabled = true;
        levelTimer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameOver)
        {
            UpdateTimer();
            CheckOnPlayer();
        }
        else
        {
            driver.steeringEnabled = false;
        }
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

    void OnVictory()
    {
        if(submitScoresEnabled)
        {
            Debug.Log("sending score");
            networking.SubmitNewScore(player.name, levelTimer);
        }
        Score s = new Score();
        s.inserterID = player.name;
        s.scoreData.time = levelTimer;
        networking.scoreList.Add(s);
        networking.scoreList.Sort((x, y) => x.scoreData.time.CompareTo(y.scoreData.time));
        isGameOver = true;
    }

    void OnDeath()
    {
        if(submitGhostsEnabled)
        {
            Debug.Log("sending ghost");
            networking.SubmitNewGhost(player.name, player.transform.position, player.transform.rotation);
        }
        isGameOver = true;
    }
}
