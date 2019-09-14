using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    public float changeSpeed;
    public StirlingEngine playerEngine;

    string heatAxis;
    string coolingAxis;
	// Use this for initialization
	void Start () {
        heatAxis = "Heat";
        coolingAxis = "Cooling";
	}
	
	// Update is called once per frame
	void Update () {
        float heatchange = Input.GetAxis(heatAxis);
        float coolingchange = Input.GetAxis(coolingAxis);

        playerEngine.heatValue += heatchange * Time.deltaTime * changeSpeed;
        playerEngine.coolingValue += coolingchange * Time.deltaTime * changeSpeed;
    }
}
