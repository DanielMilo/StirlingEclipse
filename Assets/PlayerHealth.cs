using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth:MonoBehaviour
{
    // Start is called before the first frame update

    public GhostTracer ghostTracer;
    public float heightThreshhold;
    Vector3 lastPositionAboveGround;
    Quaternion lastRotationAboveGround;
    StirlingEngine playerEngine;
    bool isAlive;

    void Start()
    {
        playerEngine = GetComponent<StirlingEngine>();
        lastPositionAboveGround = transform.position;
        lastRotationAboveGround = transform.rotation;
        isAlive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive)
        {
            UpdateLastPosition();

            if(IsHeatDeath() || IsFallDeath())
            {
                Debug.Log("YOU DIED!");
                CreateGhost();
                isAlive = false;
            }
        }
    }

    void UpdateLastPosition()
    {
        if(GetDistanceToFloor() <= heightThreshhold)
        {
            lastPositionAboveGround = transform.position;
            lastRotationAboveGround = transform.rotation;
        } 
    }

    bool IsHeatDeath()
    {
        return playerEngine.coolingValue == 0 && playerEngine.heatValue == 0;
    }

    bool IsFallDeath()
    {
        return transform.position.y < -100;
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
