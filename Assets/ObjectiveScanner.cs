using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectiveScanner : MonoBehaviour
{

    TutorialManager tutorialManager;


    // Start is called before the first frame update
    void Start()
    {
        tutorialManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<TutorialManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other)
    {
        tutorialManager.OnPlayerTrigger(other);
    }
}
