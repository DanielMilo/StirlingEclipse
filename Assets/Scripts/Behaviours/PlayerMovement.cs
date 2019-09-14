﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StirlingEngine))]
public class PlayerMovement : MonoBehaviour
{

    public enum MovementMode
    {
        wheelsteering, hoversteering, omnisteering
    }

    // states
    public MovementMode movementMode;

    //hover
    public bool enableHover;
    public float hoverHeight;
    public float hoverMaxG;
    public float currentHeight;

    //speed limit
    public float turnSpeed;
    public float speedlimit;

    //components
    Rigidbody rbody;
    StirlingEngine engine;
    public GameObject playerModel;
    public float tilt;

    //input variables
    private float verticalValue;
    private float horizontalValue;
    private float sidewaysValue;

    // Use this for initialization
    void Start()
    {
        rbody = this.GetComponent<Rigidbody>();
        engine = this.GetComponent<StirlingEngine>();
    }

    void Update()
    {
        UpdateAxisValues();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();

        if(enableHover)
        {
            Hover();
        }
        //TiltModel();
    }

    private void TiltModel()
    {
        Quaternion playerRotation = playerModel.transform.rotation;
        //Vector3.Angle(Vector3.up, playerModel.transform.up);
        Quaternion targetRotation = Quaternion.Euler(0f, transform.rotation.y, 50f);
        playerModel.transform.rotation = Quaternion.RotateTowards(playerRotation, targetRotation, 10 * Time.deltaTime);
    }

    private void Move()
    {
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
    }

    void UpdateAxisValues()
    {
        verticalValue = Input.GetAxis("Vertical");
        horizontalValue = Input.GetAxis("Horizontal");
        sidewaysValue = Input.GetAxis("Sideways");
    }

    private void WheeledTurn()
    {
        float velocityAngle = Vector3.Angle(rbody.velocity, transform.forward);
        if(velocityAngle >= 90)
        {
            horizontalValue *= -1;
        }

        float turn = horizontalValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
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

        if(CurrentHorizontalSpeed() > speedlimit)
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

    private void Hover()
    {
        currentHeight = GetDistanceToFloor();

        if(currentHeight <= hoverHeight) // apply force to slow down
        {
            float deltaHeightPercentage = (hoverHeight - currentHeight) / hoverHeight;
            float forceMultiplier = deltaHeightPercentage * hoverMaxG;
            Vector3 upwardForce = (Physics.gravity + Physics.gravity * forceMultiplier) * -1;

            rbody.AddForce(upwardForce, ForceMode.Acceleration);
        }
        else if(currentHeight > hoverHeight && currentHeight <= hoverHeight * 2) //apply weak antigravity when above hover height
        {
            float deltaHeightPercentage = (hoverHeight * 2 - currentHeight) / hoverHeight;
            Vector3 upwardForce = (Physics.gravity * deltaHeightPercentage);

            rbody.AddForce(upwardForce, ForceMode.Acceleration);
        }
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

    private float CurrentHorizontalSpeed()
    {
        float speed = new Vector3(rbody.velocity.x, 0f, rbody.velocity.z).magnitude;
        return speed;
    }
}
