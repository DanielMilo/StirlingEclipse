using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public Resource type;
    public float amount;
    public bool isActive;
    [SerializeField] float respawnTime;
    [SerializeField] GameObject pickupObject;
    float respawnProgress;

    // Start is called before the first frame update
    void Start()
    {
        isActive = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isActive)
        {
            respawnProgress += Time.deltaTime;
            if(respawnProgress >= respawnTime)
            {
                SetPickupActive(true);
                respawnProgress = 0f;
            }
        }
    }

    public void SetPickupActive(bool b)
    {
        isActive = b;
        pickupObject.SetActive(b);
    }
}

public enum Resource
{
    heat, cold
}
