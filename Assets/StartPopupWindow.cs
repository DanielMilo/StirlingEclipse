using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPopupWindow : MonoBehaviour
{
    [SerializeField] Text inputText;
    [SerializeField] GameObject window;
    [SerializeField] InputField field;

    GameController controller;
    Craft player; 

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Craft>();
        field.Select();
    }

    // Update is called once per frame
    void Update()
    {
        window.SetActive(controller.gameState == GameState.startup);
    }

    public void Confirm()
    {
        if(!string.IsNullOrEmpty(inputText.text))
        {
            player.name = inputText.text;
            controller.FinishStartup();
        }
    }
}
