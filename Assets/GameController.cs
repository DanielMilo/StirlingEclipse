using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;

    Transform spawn;
    Driver driver;

    GameObject playerObject;
    Craft player;
    
    [HideInInspector] public bool isGameOver;
    [HideInInspector] public float levelTimer;

    // Start is called before the first frame update
    void Awake()
    {
        isGameOver = false;
        SpawnPlayer();
        driver = GetComponent<Driver>();
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
        isGameOver = true;
    }

    void OnDeath()
    {
        isGameOver = true;
    }
}
