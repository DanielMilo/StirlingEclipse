using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpWindowManager:MonoBehaviour
{

    public GameObject panel;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        panel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("F1"))
        {
            panel.SetActive(!panel.activeSelf);
        }
    }

    public void SetPlayerName(Text t)
    {
        if(t.text != "")
            player.name = t.text;
    }
}
