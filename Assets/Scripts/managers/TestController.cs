using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestController : MonoBehaviour {

    public float changeSpeed;
    StirlingEngine playerEngine;

    string heatAxis;
    string coolingAxis;

	// Use this for initialization
	void Start () {
        playerEngine = GameObject.FindGameObjectWithTag("Player").GetComponent<StirlingEngine>();
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
