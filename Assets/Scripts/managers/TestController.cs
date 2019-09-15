using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    public float changeSpeed;
    Craft player;

    string heatAxis;
    string coolingAxis;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        heatAxis = "Heat";
        coolingAxis = "Cooling";
	}
	
	// Update is called once per frame
	void Update () {
        float heatchange = Input.GetAxis(heatAxis);
        float coolingchange = Input.GetAxis(coolingAxis);

        player.engine.heatValue += heatchange * Time.deltaTime * changeSpeed;
        player.engine.coolingValue += coolingchange * Time.deltaTime * changeSpeed;
    }
}
