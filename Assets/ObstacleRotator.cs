using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleRotator : MonoBehaviour
{

    [SerializeField] float turnSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Turn();
    }

    void Turn()
    {
        float turn = turnSpeed * Time.deltaTime;
        Quaternion turnRotation = Quaternion.Euler(turn, 0f, 0f);
        transform.rotation *= turnRotation;
    }
}
