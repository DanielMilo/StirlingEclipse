using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotor : MonoBehaviour
{
    public GameObject rotator;
    public float angle;
    public float minValue;
    public float maxValue;
    public float value;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        value = Mathf.Clamp(value, minValue, maxValue);

        Rotate();
    }

    void Rotate()
    {
        float percentage = value / Mathf.Abs(maxValue - minValue);
        float rotation = (angle * percentage) - (angle / 2);


        Vector3 rotatorEuler = rotator.transform.rotation.eulerAngles;
        rotator.transform.rotation = Quaternion.Euler(rotatorEuler.x, rotatorEuler.y, -rotation);
    }
}
