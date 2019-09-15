using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Craft : MonoBehaviour
{
    // PARAMETERS
    [Header("Main")]
    [SerializeField] GameObject playerModel;
    [SerializeField] MovementMode movementMode;

    [Header("Movement")]
    [SerializeField] float turnSpeed;
    [SerializeField] public float speedlimit;
    [SerializeField] float antidrift;
    [SerializeField] bool enableTilt;
    [SerializeField] float tilt;
    [SerializeField] float tiltSpeed;

    [Header("Hover")]
    [SerializeField] bool enableHover;
    [SerializeField] float hoverHeight;
    [SerializeField] float hoverMaxG;

    // BLACKBOARD
    [HideInInspector] public float currentHeight;
    [HideInInspector] public float currentHorizontalSpeed;
    [HideInInspector] public StirlingEngine engine;

    // COMPONENTS
    Rigidbody rbody;

    // PRIVATE
    private float verticalValue;
    private float horizontalValue;
    private float sidewaysValue;

    // ENUMS
    public enum MovementMode
    {
        wheelsteering, wheeldrift, hoversteering, omnisteering
    }

    // MONOBEHAVIOUR
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        engine = this.GetComponent<StirlingEngine>();
    }

    void Update()
    {
        UpdateAxisValues();
    }

    void FixedUpdate()
    {
        UpdatePhysicsValue();
        ExecuteMovement();
    }

    void UpdatePhysicsValue()
    {
        currentHeight = GetDistanceToFloor();
        currentHorizontalSpeed = CurrentHorizontalSpeed();
    }

    void UpdateAxisValues()
    {
        verticalValue = Input.GetAxis("Vertical");
        horizontalValue = Input.GetAxis("Horizontal");
        sidewaysValue = Input.GetAxis("Sideways");
    }

    void ExecuteMovement()
    {
        Move();

        if(enableHover)
        {
            Hover();
        }

        if(enableTilt)
        {
            TiltModel();
        }
    }

    // HOVER
    private void Hover()
    {
        if(currentHeight <= hoverHeight) // apply force to slow down
        {
            float deltaHeightPercentage = (hoverHeight - currentHeight) / hoverHeight;
            float forceMultiplier = deltaHeightPercentage * hoverMaxG;
            Vector3 upwardForce = -1 * (Physics.gravity + Physics.gravity * forceMultiplier); // *-1 because counterforce to gravity

            rbody.AddForce(upwardForce, ForceMode.Acceleration);
        }
        else if(currentHeight > hoverHeight && currentHeight <= hoverHeight * 2) //apply weak antigravity when above hover height
        {
            float deltaHeightPercentage = (hoverHeight * 2 - currentHeight) / hoverHeight; // calculate percentage between hoverheight, and twice hoverheight
            Vector3 upwardForce = (Physics.gravity * deltaHeightPercentage);

            rbody.AddForce(upwardForce, ForceMode.Acceleration);
        }
    }

    // MOVEMENT
    private void Move()
    {
        switch(movementMode)
        {
            case MovementMode.wheelsteering:
                Turn();
                AdjustVelocityDirectionByDegrees(float.MaxValue);
                Move1Axis();
                break;

            case MovementMode.wheeldrift:
                if(currentHorizontalSpeed > 0.1)
                    Turn();
                AdjustVelocityDirectionByDegrees(antidrift);
                Move1Axis();
                break;

            case MovementMode.hoversteering:
                Turn();
                Move1Axis();
                break;

            case MovementMode.omnisteering:
                Turn();
                MoveOmniAxis();
                break;
        }
    }

    private void Turn()
    {
        float turn = horizontalValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        gameObject.transform.rotation *= turnRotation;
    }

    private void AdjustVelocityDirectionByDegrees(float factor)
    {
        float velocityAngle = Vector3.Angle(rbody.velocity, transform.forward);
        Vector3 horizontalVelocity = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z);

        Vector3 movement;
        if(velocityAngle < 90)
        {
            movement = Vector3.RotateTowards(horizontalVelocity, transform.forward * horizontalVelocity.magnitude, factor, horizontalVelocity.magnitude);
        }
        else
        {
            movement = Vector3.RotateTowards(horizontalVelocity, -transform.forward * horizontalVelocity.magnitude, factor, horizontalVelocity.magnitude);
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

        if(currentHorizontalSpeed > speedlimit)
        {
            force = Vector3.zero;
        }
        else
        {
            force = direction.normalized * engine.CalculateEnginePower(); // * Time.deltaTime;   
        }
        rbody.AddForce(force);
        rbody.angularVelocity = new Vector3(0, 0, 0);
    }

    // TILT
    private void TiltModel()
    {
        Quaternion playerRotation = playerModel.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, playerRotation.eulerAngles.y, -(tilt * horizontalValue));
        playerModel.transform.rotation = Quaternion.RotateTowards(playerRotation, targetRotation, tiltSpeed * Time.deltaTime);
    }


    // PHYSICS CALCULATIONS
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

    private float CurrentHorizontalSpeed()
    {
        float speed = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z).magnitude;
        return speed;
    }
}
