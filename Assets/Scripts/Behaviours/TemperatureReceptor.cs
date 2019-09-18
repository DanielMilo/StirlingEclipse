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
            OnPickupCollision(other);
        }
        if(other.tag == "temperatureZone")
        {
            TempZone zone = other.GetComponentInParent<TempZone>();
            OnZoneCollision(zone);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "temperatureZone")
        {
            TempZone zone = other.GetComponentInParent<TempZone>();
            OnZoneCollision(zone);
        }
    }

    private void OnPickupCollision(Collider other)
    {
        PickupManager pickup = other.GetComponentInParent<PickupManager>();
        if(pickup.isActive)
        {
            ChangeRessource(pickup.type, pickup.amount);
            pickup.SetPickupActive(false);
        }
    }

    private void OnZoneCollision(TempZone zone)
    {
        if(IsInDrawbackZone(zone))
        {
            float amount = zone.maxAmount * Time.deltaTime; // player gets max amount in drawback zone
            float drawbackAmount = zone.maxDrawbackAmount * Time.deltaTime * -1; // player also gets negative ressource for drawback
            ChangeRessource(zone.type, amount);
            ChangeRessource(zone.drawbackType, drawbackAmount);
        }
        else
        {
            float distancePercentage = GetDistancePercentageFromCollectionZone(zone);
            float amount = zone.maxAmount * distancePercentage * Time.deltaTime;
            ChangeRessource(zone.type, amount);
        }
    }

    private void ChangeRessource(Resource type, float amount)
    {
        switch(type)
        {
            case Resource.heat:
                player.engine.heatValue = Mathf.Clamp(player.engine.heatValue + amount, 0f, 100f);
                break;
            case Resource.cold:
                player.engine.coolingValue = Mathf.Clamp(player.engine.coolingValue + amount, 0f, 100f);
                break;
        }
    }

    private bool IsInDrawbackZone(TempZone zone)
    {
        Vector3 closestDrawback = zone.drawbackZone.ClosestPointOnBounds(transform.position);
        Vector3 objToDrawback = closestDrawback - transform.position;
        return objToDrawback.magnitude <= 0f;
    }

    private float GetDistancePercentageFromCollectionZone(TempZone zone)
    {
        // get vector to drawbackzone
        Vector3 closestDrawback = zone.drawbackZone.ClosestPointOnBounds(transform.position);
        Vector3 objToDrawback = closestDrawback - transform.position;

        // extend vector in the other way
        Vector3 pointOutsideOfZone = objToDrawback.normalized * -1; // outside direction
        pointOutsideOfZone = pointOutsideOfZone * zone.collectionZone.bounds.extents.magnitude * 2; //extend that direction to outside of box
        pointOutsideOfZone = pointOutsideOfZone + transform.position; // apply vector to current transform to get point outside of box

        // raycast to collection zone
        Vector3 closestCollection = zone.collectionZone.ClosestPointOnBounds(pointOutsideOfZone);

        // get total distance
        float totalDistance = (closestCollection - closestDrawback).magnitude;

        // get how deep we are in the collection zone
        float distancePercentage = (totalDistance - objToDrawback.magnitude) / totalDistance;
        return Mathf.Clamp(distancePercentage, 0f, 1f);
    }
}
