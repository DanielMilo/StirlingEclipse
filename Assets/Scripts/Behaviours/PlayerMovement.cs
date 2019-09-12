using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public enum MovementMode
    {
        min, additive, combiboost
    }

    //public float movementSpeed;
    public float turnSpeed;
    public float minSpeed;
    public float maxSpeed;
    
    public MovementMode movMode;
    public float overFuelBoost; // percentage how much "unused/leftover" fuel gives in boost mode
    public float heatValue; // heat value between 0 to 100
    public float coolingValue; // cooling value between 0 to 100
    public bool enablePhysicsMovement;

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

        /*
        if(verticalValue < 0)
        {
            horizontalValue *= -1;
        }
        if(verticalValue != 0f)
        {
            Turn();
        }
        */
        if(verticalValue < 0)
        {
            horizontalValue *= -1;
        }
        Turn();

        if(enablePhysicsMovement)
        {
            ExperimentalMove();
        }
        else
        {
            Move();
        }
    }

    public float CalculateSpeed()
    {
        float speedPercentage;
        switch(movMode)
        {
            case MovementMode.min: // min calculation
                speedPercentage = Mathf.Min(heatValue, coolingValue) / 100;
                break;
            case MovementMode.additive: // additive
                speedPercentage = (heatValue + coolingValue) / (100*2);
                break;
            case MovementMode.combiboost: // combined values give big boost
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
        //Debug.Log(CalculateSpeed());
        Vector3 move = transform.forward * verticalValue * CalculateSpeed() * Time.deltaTime;

        gameObject.transform.position += move;
    }

    private void ExperimentalMove()
    {
        Rigidbody rbody = this.GetComponent<Rigidbody>();
        float velocityAngle = Vector3.Angle(rbody.velocity, transform.forward);
        float oldSpeed = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z).magnitude;

        Vector3 movement;
        if(velocityAngle < 90)
        {
            movement = transform.forward * oldSpeed;
        }
        else
        {
            movement = transform.forward * -1 * oldSpeed;
        }
        
        rbody.velocity = new Vector3(movement.x, rbody.velocity.y, movement.z);

        Vector3 force = transform.forward * verticalValue * CalculateSpeed(); // * Time.deltaTime;
        rbody.AddForce(force);
        rbody.angularVelocity = new Vector3(0, 0, 0);
    }
}
