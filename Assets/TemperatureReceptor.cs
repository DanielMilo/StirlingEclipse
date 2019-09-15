using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureReceptor : MonoBehaviour
{
    Craft player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Craft>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "pickup")
        {
            PickupManager pickup = other.GetComponentInParent<PickupManager>();
            switch(pickup.type)
            {
                case Resource.heat:
                    player.engine.heatValue = Mathf.Clamp(player.engine.heatValue + pickup.amount, 0f, 100f);
                    break;
                case Resource.cold:
                    player.engine.coolingValue = Mathf.Clamp(player.engine.coolingValue + pickup.amount, 0f, 100f);
                    break;
            }
            pickup.SetPickupActive(false);
        }
    }
}
