using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

    public Transform objectFocus;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        Move();
        Turn();
	}

    void Move()
    {
        transform.position = objectFocus.transform.position;
    }

    void Turn()
    {
        Vector3 oldRotation = objectFocus.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0f, oldRotation.y, 0f);
    }
    
}
