using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlattfrom : MonoBehaviour
{
    public enum MovementMode
    {
        loop, backAndForth, randomized
    }

    [SerializeField] Transform plattform;
    [SerializeField] float movementSpeed;
    [SerializeField] GameObject pointCollection;
    
    int currentIndex;
    int currentIndexModifyer;
    List<Transform> ralleyPoints;

    // Start is called before the first frame update
    void Start()
    {
        GetRalleyPoints();

        if(ralleyPoints.Count > 0)
        {
            plattform.position = ralleyPoints[0].position;
            plattform.rotation = ralleyPoints[0].rotation;
        }

        currentIndex = 1;
        currentIndexModifyer = 1;

        
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = Vector3.Distance(plattform.transform.position, ralleyPoints[currentIndex].position);

        if(distanceToTarget > 0.1f)
        {
            Move();
        }
        else
        {
            ApplyIndexModifyer();
            Move();
        }
    }

    void GetRalleyPoints()
    {
        ralleyPoints = new List<Transform>();

        for(int index = 0; index < pointCollection.transform.childCount; index++)
        {
            ralleyPoints.Add(pointCollection.transform.GetChild(index));
            Debug.Log(pointCollection.transform.GetChild(index).gameObject.name);
        }
    }

    void ApplyIndexModifyer()
    {
        currentIndex += currentIndexModifyer;
        if(currentIndex >= ralleyPoints.Count)
        {
            currentIndex = 0;
        }
    }

    void Move()
    {
        float distanceToTarget = Vector3.Distance(plattform.transform.position, ralleyPoints[currentIndex].position);
        float angleToTarget = Quaternion.Angle(plattform.transform.rotation, ralleyPoints[currentIndex].rotation);

        float movementFactor = distanceToTarget / movementSpeed;

        plattform.transform.position = Vector3.MoveTowards(plattform.transform.position, ralleyPoints[currentIndex].position, movementSpeed);
        plattform.transform.rotation = Quaternion.RotateTowards(plattform.transform.rotation, ralleyPoints[currentIndex].rotation, angleToTarget/movementFactor);
    }

}
