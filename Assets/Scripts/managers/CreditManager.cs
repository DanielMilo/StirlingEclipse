using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditManager : MonoBehaviour
{
    GameController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        controller.gameState = GameState.tutorial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
