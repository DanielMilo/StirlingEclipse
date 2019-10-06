using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GhostTextfield : MonoBehaviour
{

    public Text textfield;
    GameObject mainCamera;
    Canvas canvas;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        canvas = GetComponentInChildren<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        textfield.text = this.name;

        canvas.transform.rotation = mainCamera.transform.rotation;
    }
}
