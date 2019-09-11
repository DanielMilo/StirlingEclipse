using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

    public float movementSpeed;
    public float turnSpeed;

    private string verticalAxis;
    private string horizontalAxis;
    private float verticalValue;
    private float horizontalValue;

	// Use this for initialization
	void Start () {
        verticalAxis = "Vertical";
        horizontalAxis = "Horizontal";
    }
	
	// Update is called once per frame
	void FixedUpdate () {
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

    private void Turn()
    {
        float turn = horizontalValue * turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn,  0f);
        gameObject.transform.rotation *= turnRotation;
    }

    private void Move()
    {
        Vector3 move = transform.forward * verticalValue * movementSpeed * Time.deltaTime;

        gameObject.transform.position += move;
    }
}
