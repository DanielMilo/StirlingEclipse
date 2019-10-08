using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth:MonoBehaviour
{
    // Start is called before the first frame update

    public GhostTracer ghostTracer;
    [SerializeField] float heightThreshhold;
    [SerializeField] float mapLowerBounds;
    [SerializeField] float heatdeathTimeout;

    float heatdeathTimer;
    
    Craft player;

    void Start()
    {
        player = GetComponent<Craft>();
        player.lastPositionAboveGround = transform.position;
        player.lastRotationAboveGround = transform.rotation;
        player.isAlive = true;
        player.hasWon = false;

        player.criticalResource = Resource.cold;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isAlive)
        {
            UpdateLastPosition();
            UpdateTimerPercentage();

            CheckHeatDeath();
            CheckFallDeath();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "deathzone")
        {
            OnDeath("Deathzone");
        }
        else if(other.tag == "Finish")
        {
            OnWinning();
        }
    }

    void OnDeath(string reason)
    {
        Debug.Log("YOU DIED! (" + reason + ")");
        player.isAlive = false;
    }

    void OnWinning()
    {
        Debug.Log("YOU WON!");
        player.hasWon = true;
    }

    void UpdateLastPosition()
    {
        if(player.currentHeight <= heightThreshhold)
        {
            player.lastPositionAboveGround = transform.position;
            player.lastRotationAboveGround = transform.rotation;
        } 
    }

    void CheckHeatDeath()
    {
        if(player.engine.coolingValue == 0 && player.engine.heatValue == 0)
        {
            player.criticalResource = Resource.heat;
            heatdeathTimer += Time.deltaTime;
        }
        else if(player.engine.coolingValue == 0 && player.engine.heatValue >= 99)
        {
            player.criticalResource = Resource.heat;
            heatdeathTimer += Time.deltaTime;
        }
        else if(player.engine.coolingValue >= 99 && player.engine.heatValue == 0)
        {
            player.criticalResource = Resource.cold;
            heatdeathTimer += Time.deltaTime;
        }
        else
        {
            heatdeathTimer = 0f;
        }

        if(heatdeathTimer >= heatdeathTimeout)
        {
            OnDeath("Heatdeath");
        }
    }

    void CheckFallDeath()
    {
        if(transform.position.y <= mapLowerBounds)
        {
            OnDeath("Out of map bounds");
        }
    }

    void UpdateTimerPercentage()
    {
        player.heatdeathTimerPercentage =  Mathf.Clamp(heatdeathTimer / heatdeathTimeout, 0f, 1f);
    }

}
