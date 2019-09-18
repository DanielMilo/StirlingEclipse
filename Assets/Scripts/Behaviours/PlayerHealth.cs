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
    Vector3 lastPositionAboveGround;
    Quaternion lastRotationAboveGround;
    Craft player;

    void Start()
    {
        player = GetComponent<Craft>();
        lastPositionAboveGround = transform.position;
        lastRotationAboveGround = transform.rotation;
        player.isAlive = true;
        player.hasWon = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.isAlive)
        {
            UpdateLastPosition();

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
            lastPositionAboveGround = transform.position;
            lastRotationAboveGround = transform.rotation;
        } 
    }

    void CheckHeatDeath()
    {
        if(player.engine.coolingValue == 0 && player.engine.heatValue == 0)
        {
            heatdeathTimer += Time.deltaTime;
            if(heatdeathTimer >= heatdeathTimeout)
            {
                OnDeath("Heatdeath");
            }
        }
        else
        {
            heatdeathTimer = 0f;
        }
    }

    void CheckFallDeath()
    {
        if(transform.position.y <= mapLowerBounds)
        {
            OnDeath("Out of map bounds");
        }
    }

    void CreateGhost()
    {
        ghostTracer.SubmitNewGhost(name, lastPositionAboveGround, lastRotationAboveGround);
    }

    private float GetDistanceToFloor()
    {
        Vector3 downVector = new Vector3(0f, -1f, 0f);

        RaycastHit rayhit;
        if(Physics.Raycast(transform.position, downVector, out rayhit))
        {
            return rayhit.distance;
        }
        else
        {
            return float.MaxValue;
        }
    }
}
