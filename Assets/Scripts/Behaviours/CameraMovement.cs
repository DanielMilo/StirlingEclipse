using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    [SerializeField] GameObject camera;
    [SerializeField] float baseDistance;
    [SerializeField] float distanceRange;


    Craft player;
    Vector3 baseVector;

    // Use this for initialization
    void Start ()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        baseVector = camera.transform.localPosition;
    }
	
	// Update is called once per frame
	void Update ()
    {
        Move();
        Turn();
        MoveCamera();

    }

    void Move()
    {
        transform.position = player.transform.position;
    }

    void MoveCamera()
    {
        float speedPercentage = player.currentHorizontalSpeed / player.speedlimit;
        float distanceModifyer = baseDistance + distanceRange* speedPercentage;
        camera.transform.localPosition = (baseVector.normalized * distanceModifyer);
    }

    void Turn()
    {
        Vector3 oldRotation = player.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, oldRotation.y, 0f);
    }
    
}
