using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{

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
            elements[index].transform.parent = transform;
        }
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        for(int index = 0; index < transform.childCount; index++)
        {
            transform.GetChild(index).gameObject.SetActive(controller.isGameOver);
        }
    }
}
