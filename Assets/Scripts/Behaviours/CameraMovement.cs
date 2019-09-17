using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] GameObject camera;
    [SerializeField] float baseDistance;
    [SerializeField] float distanceRange;
    [SerializeField] float cameraTiltFactor;
    [SerializeField] float victoryTurnSpeed = 1;

    GameController controller;
    Craft player;
    Vector3 cameraDirection;
    float baseRotation;

    // Use this for initialization
    void Start ()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        cameraDirection = camera.transform.localPosition.normalized;
        baseRotation = camera.transform.localRotation.eulerAngles.x;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if(!controller.isGameOver)
        {
            Move();
            Turn();
            MoveCamera();
            TiltCamera();
        }
        else
        {
            Move();
            VictoryTurn();
            MoveCamera();
            TiltCamera();
        }
    }

    void Move()
    {
        transform.position = player.transform.position;
    }

    void Turn()
    {
        Vector3 playerRotation = player.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, playerRotation.y, 0f);
    }

    void VictoryTurn()
    {
        Vector3 playerRotation = player.transform.rotation.eulerAngles;
        Quaternion targetRotation = Quaternion.Euler(0f, playerRotation.y + 180, 0f);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, victoryTurnSpeed);
    }

    void MoveCamera()
    {
        float speedPercentage = player.currentHorizontalSpeed / player.speedlimit;
        float targetDistance = baseDistance + distanceRange * speedPercentage;
        targetDistance = getFurthestViableCameraDistance(targetDistance);
        camera.transform.localPosition = (cameraDirection * targetDistance);
    }

    void TiltCamera()
    {
        float targetTilt = player.currentTilt * cameraTiltFactor; //camera tilts half as much as player model
        camera.transform.localRotation = Quaternion.Euler(baseRotation, 0f , targetTilt);
    }

    float getFurthestViableCameraDistance(float distance)
    {
        RaycastHit hit;
        Vector3 globalDirection = transform.TransformDirection(cameraDirection);
        if(Physics.Raycast(transform.position, globalDirection, out hit))
        {
            return hit.distance * 0.95f; //slightly shorter distance so camera doesnt clip in object
        }
        else
        {
            return distance;
        }
    }
}
