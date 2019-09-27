using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager:MonoBehaviour
{
    [SerializeField] InGameMenu menu;
    [SerializeField] GameObject window;
    [SerializeField] GameObject elementPrefab;
    [SerializeField] int elementCount = 10;
    [SerializeField] Vector3 boardOffset;
    [SerializeField] float elementHeight;

    GameObject[] elements;
    GameController controller;

    // Start is called before the first frame update
    void Start()
    {
        elements = new GameObject[elementCount];
        for(int index = 0; index < elements.Length; index++)
        {
            Vector3 targetPosition = boardOffset;
            targetPosition.y = boardOffset.y - (elementHeight * index);
            elements[index] = GameObject.Instantiate(elementPrefab, transform.position + targetPosition, transform.rotation);
            elements[index].transform.parent = window.transform;
        }
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        window.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(controller.gameState == GameState.victory)
        {
            UpdateValues();
            window.SetActive(true);
        }
        else if(controller.gameState == GameState.death)
        {
            UpdateValues();
            window.SetActive(true);
        }
        else if(menu != null && menu.showLeaderboard)
        {
            UpdateValues();
            window.SetActive(true);
        }
        else
        {
            window.SetActive(false);
        }
    }

    void UpdateValues()
    {
        List<Score> scoreList = controller.networking.scoreList;

        //set elements 0 -> (elementCount-2)
        for(int index = 0; index < elements.Length && index < elementCount - 1; index++)
        {
            if(index < scoreList.Count)
            {
                Score s = scoreList[index];
                elements[index].GetComponent<LeaderboardElement>().SetData(index + 1, s.inserterID, s.scoreData.time);
            }
            else
            {
                elements[index].GetComponent<LeaderboardElement>().SetData(index + 1, " - - - - - - - - - - - - - - ", -1f);
            }
        }

        //set last element
        if(scoreList.Contains(controller.networking.playerScore))
        {
            int playerPlace = scoreList.IndexOf(controller.networking.playerScore);
            if(playerPlace >= elementCount - 1)
            {
                Score s = controller.networking.playerScore;
                elements[elementCount - 1].GetComponent<LeaderboardElement>().SetData(playerPlace, s.inserterID, s.scoreData.time);
            }
        }
        else
        {
            if(elementCount - 1 < scoreList.Count)
            {
                Score s = scoreList[elementCount - 1];
                elements[elementCount - 1].GetComponent<LeaderboardElement>().SetData(elementCount - 1, s.inserterID, s.scoreData.time);
            }
            else
            {
                elements[elementCount - 1].GetComponent<LeaderboardElement>().SetData(elementCount - 1, " - - - - - - - - - - - - - - ", -1f);
            }
        }
    }

    public void OnMainMenuButton()
    {
        controller.LoadMainMenu();
    }

    public void OnRestartButton()
    {
        controller.ReloadLevel();
    }

    public void OnNextButton()
    {
        controller.LoadNextLevel();
    }
}
