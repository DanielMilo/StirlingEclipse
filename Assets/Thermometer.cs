using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thermometer : MonoBehaviour
{
    public float currentTemp;


    // maybe it would be better to keep this somewhere else (like on the player object so there is easy access from the UI
    [Range(0, 1)]
    public float heatDecay = 1;

    private GameObject[] tempSources;
    

    // Start is called before the first frame update
    void Start()
    {
        // this implies that heat sources must be tagged
        tempSources = GameObject.FindGameObjectsWithTag("heatsource");
    }

    // Update is called once per frame
    void Update()
    {
        currentTemp = localTemp();

        //Debug.Log(currentTemp);
        
    }

   public float localTemp()
    {
        float cummulativeTemp = 0;

        foreach (GameObject source in tempSources)
            {
            //find out where things are
            Vector3 sourcePosition = source.transform.position;
            Vector3 directiontoSource = sourcePosition - transform.position;
            float distance = Vector3.Distance(transform.position, sourcePosition);

            // see if anything is sitting between this and the source
            if (!Physics.Raycast(transform.position, directiontoSource, distance))
                {
                // calculate our temperture radiation
                int sourceTemp = source.GetComponent<TemperatureSource>().TempIntensity;

                cummulativeTemp += ( 1 / Mathf.Pow(distance,(1+heatDecay)) ) * sourceTemp; 
                }
            else
                {
                    cummulativeTemp += 0;
                }

            }

        return cummulativeTemp;
    }

}
