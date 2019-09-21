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
    [SerializeField] float maxTiltCorrection;
    [SerializeField] float tiltCorrectionSpeed;
    [SerializeField] float tiltScanDistance;
    [SerializeField] public float bodyHeight; // estimate of the height of the rigid body for checking distance to ground

    [Header("Effects")]
    [SerializeField] bool enableModelTilt;
    [SerializeField] float modelMaxTilt;
    [SerializeField] float modelTiltSpeed;

    [Header("Hover")]
    [SerializeField] bool enableHover;
    [SerializeField] float hoverHeight;
    [SerializeField] float hoverMaxG;

    // BLACKBOARD
    [HideInInspector] public float currentHeight;
    [HideInInspector] public float currentHorizontalSpeed;
    [HideInInspector] public float currentTilt;
    [HideInInspector] public StirlingEngine engine;
    [HideInInspector] public bool isAlive;
    [HideInInspector] public bool hasWon;

    // COMPONENTS
    Rigidbody rbody;

    // PRIVATE
    [HideInInspector] public float verticalValue;
    [HideInInspector] public float horizontalValue;
    [HideInInspector] public float sidewaysValue;

    // ENUMS
    public enum MovementMode
    {
        wheelsteering, wheeldrift, hoversteering, omnisteering
    }

    // MONOBEHAVIOUR
    void Awake()
    {
        engine = this.GetComponent<StirlingEngine>();
        rbody = this.GetComponent<Rigidbody>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        UpdatePhysicsValue();
        TiltCorrection();
        ExecuteMovement();
     
    }

    void UpdatePhysicsValue()
    {
        currentHeight = GetDistanceToFloor();
        currentHorizontalSpeed = CurrentHorizontalSpeed();
    }

    void ExecuteMovement()
    {
        Move();

        if(enableHover)
        {
            Hover();
        }

        if(enableModelTilt)
        {
            TiltModel();
        }
    }

    // HOVER
    public void PutOnHoverHeight()
    {
        Vector3 floor;
        GetPointOnGround(transform.position, out floor);
        Vector3 newPosition = transform.position;
        newPosition.y = floor.y + hoverHeight;
        transform.position = newPosition;
    }

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
            movement = Vector3.RotateTowards(horizontalVelocity, GetHorizontalDirection(transform.forward) * horizontalVelocity.magnitude, factor, horizontalVelocity.magnitude);
        }
        else
        {
            movement = Vector3.RotateTowards(horizontalVelocity, GetHorizontalDirection(-transform.forward) * horizontalVelocity.magnitude, factor, horizontalVelocity.magnitude);
        }

        rbody.velocity = new Vector3(movement.x, rbody.velocity.y, movement.z);
    }

    private void Move1Axis()
    {
        // zero out the y axis vector so that the player can only accelerate on the zx plane
        Vector3 direction = GetHorizontalDirection(transform.forward) * verticalValue;
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

        if (GetAngleonAxis(transform.forward) > 30)
        {
            force = force / GetAngleonAxis(transform.forward);
        }

        //test no y direction acceration
        //force.y = 0;
        //Debug.Log("Force applied = " + force);
        rbody.AddForce(force);
        rbody.angularVelocity = new Vector3(0, 0, 0);
    }

    // TILT
    private void TiltModel()
    {
        float targetTilt = (-1 * modelMaxTilt * horizontalValue);
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, modelTiltSpeed * Time.deltaTime);
        playerModel.transform.localRotation = Quaternion.Euler(0f, 0f, currentTilt);
    }

    private void TiltCorrection()
    {
        //Vector3 offsetForwards = GetHorizontalDirection(transform.forward);
        //Vector3 offsetSideways = GetHorizontalDirection(transform.right);

        float angleForwards = GetAngleonAxis(transform.forward); // note that it will need to rotate along z axis
        float angleSideways = GetAngleonAxis(transform.right); // note that it will need to rotate along x axis
        
        //added a condition for checking hoverheight
        if(angleForwards <= maxTiltCorrection && angleSideways <= maxTiltCorrection && currentHeight <= hoverHeight)
        {
            Vector3 previousRotation = transform.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(-angleForwards, previousRotation.y, angleSideways);
            transform.localRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, tiltCorrectionSpeed * Time.deltaTime);
        }
    }

    private float GetAngleonAxis(Vector3 axis)
    {
        Vector3 offset = GetHorizontalDirection(axis);
        float angle = GetTerrainAngle(offset);

        return angle;
    }

    private void TiltModelOld()
    {
        Quaternion playerRotation = playerModel.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0f, playerRotation.eulerAngles.y, (-1 * modelMaxTilt * horizontalValue)); //tilt was the wrong way
        playerModel.transform.rotation = Quaternion.RotateTowards(playerRotation, targetRotation, modelTiltSpeed * Time.deltaTime);
        currentTilt = playerModel.transform.rotation.eulerAngles.z;
    }

    // PHYSICS CALCULATIONS
    private float GetDistanceToFloor()
    {
       
        Vector3 floorPoint;
        if(GetPointOnGround(transform.position, out floorPoint))
        {
            return (transform.position - floorPoint).magnitude - bodyHeight;
        }
        else
        {
            return float.MaxValue;
        }

        /*
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
        */
    }

    private bool GetPointOnGround(Vector3 origin, out Vector3 output)
    {
        Vector3 downVector = new Vector3(0f, -1f, 0f);

        RaycastHit rayhit;
        if(Physics.Raycast(origin, downVector, out rayhit))
        {
            output = rayhit.point;
            return true;
        }
        else
        {
            output = new Vector3(0,0,0);
            return false;
        }
    }

    private float GetTerrainAngle(Vector3 offset)
    {
        Vector3 positiveOffset;
        Vector3 negativeOffset;
        //get both vectors and check if viable
        if(GetPointOnGround(transform.position + offset, out positiveOffset) && GetPointOnGround(transform.position - offset, out negativeOffset))
        {
            Vector3 slope = positiveOffset - negativeOffset;
            float angle = Vector3.Angle(offset.normalized, slope.normalized);
            if(slope.y >= 0f)
            {
                return angle;
            }
            else
            {
                return -angle;
            }
        }
        return float.MaxValue;
    }

    // normalizing this vector means that the probe will always be the same length
    private Vector3 GetHorizontalDirection(Vector3 direction)
    {
        direction.y = 0;
        return direction.normalized;
    }

    private float CurrentHorizontalSpeed()
    {
        float speed = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z).magnitude;
        return speed;
    }
}
