using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speedometer : MonoBehaviour
{

    Craft player;
    Rotor rotor;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        rotor = GetComponent<Rotor>();
        rotor.minValue = 0;
        rotor.maxValue = player.speedlimit;
    }

    // Update is called once per frame
    void Update()
    {
        rotor.value = player.currentHorizontalSpeed;
    }
}
