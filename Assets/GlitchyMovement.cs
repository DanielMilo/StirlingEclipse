using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchyMovement : MonoBehaviour
{
    [SerializeField] GameObject obj;

    float maxBigMovement;
    float maxSmallMovement;
    Vector3 originalPosition;
    Vector3 bigMove;
    Vector3 smallMove;

    float enableTimer = 0;
    float enableTimerEnd = 0;

    float moveBigTimer = 0;
    float moveBigTimerEnd = 0;

    float moveSmallTimer = 0;
    float moveSmallTimerEnd = 0;

    float rotateTimer = 0;
    float rotateTimerEnd = 0;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = obj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveBig();
        MoveSmall();
        Rotate();
        EnableDisable();

        //Debug.Log(bigMove + " " + smallMove);
        obj.transform.position = originalPosition + bigMove + smallMove;
    }

    void MoveBig()
    {
        moveBigTimer += Time.deltaTime;
        if(moveBigTimer >= moveBigTimerEnd)
        {
            moveBigTimerEnd = Random.Range(0.5f, 2);
            moveBigTimer = 0;

            Vector3 direction = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10));
            Debug.Log("Big direction" + direction);
            bigMove = direction.normalized * Random.Range(1, 3);
        }
    }

    void MoveSmall()
    {
        moveSmallTimer += Time.deltaTime;
        if(moveSmallTimer >= moveSmallTimerEnd)
        {
            moveSmallTimerEnd = Random.Range(0.1f, 0.3f);
            moveSmallTimer = 0;

            Vector3 direction = new Vector3(Random.Range(0, 10), Random.Range(0, 10), Random.Range(0, 10));
            smallMove = direction.normalized * Random.Range(0.1f, 0.5f);
        }
    }

    void EnableDisable()
    {
        enableTimer += Time.deltaTime;
        if(enableTimer >= enableTimerEnd)
        {
            enableTimerEnd = Random.Range(0.1f, 0.4f);
            enableTimer = 0;

            obj.SetActive(!obj.activeSelf);
        }
    }

    void Rotate()
    {
        rotateTimer += Time.deltaTime;
        if(rotateTimer >= rotateTimerEnd)
        {
            rotateTimerEnd = Random.Range(0.5f, 3);
            rotateTimer = 0;

            obj.transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
        }
    }
}
