using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driver : MonoBehaviour
{

    public bool steeringEnabled;
    Craft player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
    }

    // Update is called once per frame
    void Update()
    {
        if(steeringEnabled)
        {
            player.verticalValue = Input.GetAxis("Vertical");
            player.horizontalValue = Input.GetAxis("Horizontal");
            player.sidewaysValue = Input.GetAxis("Sideways");
        }
        else
        {
            player.verticalValue = 0f;
            player.horizontalValue = 0f;
            player.sidewaysValue = 0f;
        }
    }
}
