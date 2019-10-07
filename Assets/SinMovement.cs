using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] Vector3 intensity;

    Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = intensity * Mathf.Sin(Time.time * speed);
        transform.position = originalPosition + offset;
    }
}
