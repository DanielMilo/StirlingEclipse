using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth:MonoBehaviour
{
    // Start is called before the first frame update

    public GhostTracer ghostTracer;
    [SerializeField] float heightThreshhold;
    [SerializeField] float mapLowerBounds;
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

            if(IsHeatDeath() || IsFallDeath())
            {
                OnDeath();
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "deathzone")
        {
            OnDeath();
        }
        else if(other.tag == "Finish")
        {
            OnWinning();
        }
    }

    void OnDeath()
    {
        Debug.Log("YOU DIED!");
        //CreateGhost();
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

    bool IsHeatDeath()
    {
        return player.engine.coolingValue == 0 && player.engine.heatValue == 0;
    }

    bool IsFallDeath()
    {
        return transform.position.y < mapLowerBounds;
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
