using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{

    [SerializeField] GameObject prefab;
    [SerializeField] float interval;
    [SerializeField] float timerOffset;
    float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        timer = timerOffset;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= interval)
        {
            timer -= interval;
            SpawnObject();
        }
    }

    void SpawnObject()
    {
        GameObject.Instantiate(prefab, transform);
    }
}
