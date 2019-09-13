using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public enum SpeedMode
    {
        min, additive, combiboost
    }
    public enum MovementMode
    {
        wheelsteering, hoversteering, omnisteering
    }

    //public float movementSpeed;
    public float turnSpeed;
    public float minSpeed;
    public float maxSpeed;
    
    public SpeedMode speedMode;
    public MovementMode movementMode;
    public float overFuelBoost; // percentage how much "unused/leftover" fuel gives in boost mode
    public float heatValue; // heat value between 0 to 100
    public float coolingValue; // cooling value between 0 to 100
    public bool enableHover;
    public float hoverHeight;
    public float hoverMaxG;
    public float currentHeight;

    //input variables
    private string verticalAxis;
    private string horizontalAxis;
    private string sidewaysAxis;
    private float verticalValue;
    private float horizontalValue;
    private float sidewaysValue;

    //speed limit
    private Vector3 lastposition;
    public float speed;
    public float speedlimit;

    //components
    Rigidbody rbody;

    // Use this for initialization
    void Start ()
    {
        verticalAxis = "Vertical";
        horizontalAxis = "Horizontal";
        sidewaysAxis = "Sideways";
        rbody = this.GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        verticalValue = Input.GetAxis(verticalAxis);
        horizontalValue = Input.GetAxis(horizontalAxis);
        sidewaysValue = Input.GetAxis(sidewaysAxis);

        switch(movementMode)
        {
            case MovementMode.wheelsteering:
                if(CurrentHorizontalSpeed() > 0.1)
                    WheeledTurn();
                CorrectVelocityDirection();
                Move1Axis();
                break;

            case MovementMode.hoversteering:
                HoverTurn();
                Move1Axis();
                break;

            case MovementMode.omnisteering:
                HoverTurn();
                MoveOmniAxis();
                break;
        }

        if(enableHover)
        {
            Hover();
        }
    }

    public float CalculateEnginePower()
    {
        float speedPercentage;
        switch(speedMode)
        {
            case SpeedMode.min: // min calculation
                speedPercentage = Mathf.Min(heatValue, coolingValue) / 100;
                break;
            case SpeedMode.additive: // additive
                speedPercentage = (heatValue + coolingValue) / (100*2);
                break;
            case SpeedMode.combiboost: // combined values give big boost
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

    private void WheeledTurn()
    {
        float velocityAngle = Vector3.Angle(rbody.velocity, transform.forward);
        if(velocityAngle >= 90)
        {
            horizontalValue *= -1;
        }

        float turn = horizontalValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn,  0f);
        gameObject.transform.rotation *= turnRotation;
    }

    private void HoverTurn()
    {
        float turn = horizontalValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        gameObject.transform.rotation *= turnRotation;
    }

    private void CorrectVelocityDirection()
    {
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
    }

    private void Move1Axis()
    {
        Vector3 direction = transform.forward * verticalValue;
        PhysicsMovement(direction.normalized);
    }

    private void MoveOmniAxis()
    {
        Vector3 direction = (transform.forward * verticalValue) + (transform.right * sidewaysValue);
        PhysicsMovement(direction.normalized);
    }

    private void PhysicsMovement(Vector3 direction)
    {
        Vector3 force;

        if (CurrentHorizontalSpeed() > speedlimit)
        {
            force = Vector3.zero;
        }
        else
        {
            force = direction.normalized * CalculateEnginePower(); // * Time.deltaTime;   
        }
        rbody.AddForce(force);
        rbody.angularVelocity = new Vector3(0, 0, 0);
    }

    private void Hover()
    {
        Vector3 hoverVector = new Vector3(0f, -1f, 0f);

        RaycastHit rayhit;
        if(Physics.Raycast(transform.position, hoverVector, out rayhit))
        {
            currentHeight = rayhit.distance;
            if(currentHeight < hoverHeight * 0.1) // stop if hitting floor, might not be needed
            {
                rbody.velocity = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z);
            }

            if(currentHeight <= hoverHeight) // apply force to slow down
            {
                float deltaHeightPercentage = (hoverHeight - currentHeight) / hoverHeight;
                float forceMultiplier = deltaHeightPercentage * hoverMaxG;
                Vector3 upwardForce = (Physics.gravity + Physics.gravity * forceMultiplier) * -1;

                rbody.AddForce(upwardForce, ForceMode.Acceleration);
            }
            else if(currentHeight > hoverHeight && currentHeight <= hoverHeight * 2) //apply weak antigravity when above hover height
            {
                float deltaHeightPercentage = (hoverHeight *2 - currentHeight) / hoverHeight;
                Vector3 upwardForce = (Physics.gravity * deltaHeightPercentage);

                rbody.AddForce(upwardForce, ForceMode.Acceleration);
            }
        }

    }

    private float CurrentHorizontalSpeed()
    {
        float speed = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z).magnitude;
        return speed;
    }



    //outdated
    private void Move()
    {
        Vector3 move = transform.forward * verticalValue * CalculateEnginePower() * Time.deltaTime;

        gameObject.transform.position += move;
    }
}
