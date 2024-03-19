/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Some code sampled from Unity Documentation: https://docs.unity3d.com/560/Documentation/Manual/nav-AgentPatrol.html
*/

using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEditor.Experimental.GraphView;
using System;

public class WaypointController : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent agent;

    RaycastHit hit;
    public LayerMask Car;

    private int pathIndex = 0;
    private int roadCheckCount = 0;
    private int turnAngleL, turnAngleR = 0;

    private bool roadCheckNext = false;
    private bool carHit = false;
    private bool currentlyRotating = false;
    private bool firstPoint = true;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextPoint();
        //Application.targetFrameRate = 30;
    }

    void Update()
    {
        if (currentlyRotating == true)
        {
            return;
        }

        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            if (firstPoint)
            {
                pathIndex--;
                firstPoint = false;
            }
            if (roadCheckNext)
            {
                RoadCheckNow();
                return;
                

            }
            if (pathIndex > 1 && isRoadCheck(pathIndex - 2 ) && isRoadCheck(pathIndex))
            {
                int randomNumber = UnityEngine.Random.Range(0, 2);

                pathIndex -= randomNumber * 2;
                roadCheckNext = true;
            }
            else if (isRoadCheck(pathIndex))
            {
                roadCheckNext=true;
            }
            
            else if(pathIndex > 1 && isRoadCheck(pathIndex - 1))
            {
                pathIndex -= 2;
                roadCheckNext = true;
            }
            NextPoint();
        }
    }

    bool CheckCarHit()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, 30f, Car))
        {
            if (hit.collider.CompareTag("Car")) carHit = true;
        }
        return carHit;
    }


    void RoadCheckNow()
    {
        agent.isStopped = true;

        //Getting crossing ID from object name
        string roadCheckID = waypoints[pathIndex - 1].transform.name.ToString();

        //Variables relating to each crossing
        turnAngleL = Int32.Parse(roadCheckID.Substring(roadCheckID.Length - 4, 4));
        turnAngleR = -turnAngleL;

        roadCheckCount = roadCheckCounter(roadCheckID.Substring(roadCheckID.Length - 7, 2));


        Vector3 direction = waypoints[pathIndex+roadCheckCount].position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        RoadCheck();

        agent.isStopped = false;
        roadCheckNext = false;
    }

    void NextPoint()
    {
        if (waypoints.Length == 0) return;
        
        
        agent.SetDestination(waypoints[pathIndex].position);
        pathIndex = (pathIndex + 1) % waypoints.Length;
    }

    void RoadCheck()
    {
        StartCoroutine(RotateLeftAndRight());
    }
    IEnumerator RotateLeftAndRight()
    {
        yield return StartCoroutine(RotateToTarget(Quaternion.Euler(0, turnAngleL, 0)));
        yield return StartCoroutine(RotateToTarget(Quaternion.Euler(0, turnAngleR, 0)));

        if (carHit)
        {
            Debug.Log("AAH CAR!");
            currentlyRotating = false;
        }

        else
        {
            //string crossingPoint
            pathIndex += roadCheckCount-1;
            currentlyRotating = false;
            NextPoint();
        }
        
    }

    IEnumerator RotateToTarget(Quaternion finalRotation)
    {
        carHit = false;
        currentlyRotating = true;
        Quaternion initialRotation = transform.rotation;

        float rotationSpeed = 0.9f;
        float counter = 0;
        

        while (counter < rotationSpeed)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, counter / rotationSpeed);
            counter += Time.deltaTime;
            CheckCarHit();
            yield return null;
        }

        transform.rotation = finalRotation;
        
    }

    bool isRoadCheck(int _pathIndex)
    {
        return waypoints[_pathIndex].transform.name.ToString().Contains("RoadCheck");
    }

    int roadCheckCounter(string id)
    {
        int count = 0;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i].transform.name.ToString().Contains(id)) count++;

        }
        return count;
    }
}