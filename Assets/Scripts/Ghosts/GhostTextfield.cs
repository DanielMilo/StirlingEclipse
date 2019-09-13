using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostTextfield : MonoBehaviour
{

    public Text textfield;
    GameObject camera;
    Canvas canvas;

    void Start()
    {
        camera = FindObjectOfType<Camera>().gameObject;
        canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        textfield.text = this.name;

        canvas.transform.rotation = camera.transform.rotation;
    }
}
