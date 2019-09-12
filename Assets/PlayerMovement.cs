using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    //public float movementSpeed;
    public float turnSpeed;
    public float minSpeed;
    public float maxSpeed;
    public int movementMode; // 0 = min, 1 = additive, 2 = combination boost
    public float overFuelBoost; // percentage how much "unused/leftover" fuel gives in boost mode
    public float heatValue; // heat value between 0 to 100
    public float coolingValue; // cooling value between 0 to 100

    //input variables
    private string verticalAxis;
    private string horizontalAxis;
    private float verticalValue;
    private float horizontalValue;

	// Use this for initialization
	void Start ()
    {
        verticalAxis = "Vertical";
        horizontalAxis = "Horizontal";
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        verticalValue = Input.GetAxis(verticalAxis);
        horizontalValue = Input.GetAxis(horizontalAxis);

        if(verticalValue < 0)
        {
            horizontalValue *= -1;
        }
        if(verticalValue != 0f)
        {
            Turn();
        }
        Move();
	}

    private float CalculateSpeed()
    {
        float speedPercentage;
        switch(movementMode)
        {
            case 0: // min calculation
                speedPercentage = Mathf.Min(heatValue, coolingValue) / 100;
                break;
            case 1: // additive
                speedPercentage = (heatValue + coolingValue) / (100*2);
                break;
            case 2: // combined values give big boost
                float deltaFuel = Mathf.Abs(heatValue - coolingValue);
                speedPercentage = (Mathf.Min(heatValue, coolingValue) + deltaFuel * overFuelBoost) / 100;
                break;
            default:
                speedPercentage = 0f;
                break;
        }

        speedPercentage = Mathf.Clamp(speedPercentage, 0f, 1f);
        return minSpeed + (maxSpeed - minSpeed) * speedPercentage;
    }

    private void Turn()
    {
        float turn = horizontalValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn,  0f);
        gameObject.transform.rotation *= turnRotation;
    }

    private void Move()
    {
        Debug.Log(CalculateSpeed());
        Vector3 move = transform.forward * verticalValue * CalculateSpeed() * Time.deltaTime;

        gameObject.transform.position += move;
    }
}
