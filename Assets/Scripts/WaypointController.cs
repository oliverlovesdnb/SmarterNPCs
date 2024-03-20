/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Some code sampled from Unity Documentation: https://docs.unity3d.com/560/Documentation/Manual/nav-AgentPatrol.html
*/

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WaypointController : MonoBehaviour
{
    public float lookDistance;

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
            if (pathIndex > 1 && IsRoadCheck(pathIndex - 2 ) && IsRoadCheck(pathIndex))
            {
                int randomNumber = Random.Range(0, 2);

                pathIndex -= randomNumber * 2;
                roadCheckNext = true;
            }
            else if (IsRoadCheck(pathIndex))
            {
                roadCheckNext=true;
            }
            
            else if(pathIndex > 1 && IsRoadCheck(pathIndex - 1))
            {
                pathIndex -= 2;
                roadCheckNext = true;
            }
            NextPoint();
        }
    }

    bool CheckCarHit()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, lookDistance, Car))
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

        roadCheckID = roadCheckID.Substring(roadCheckID.Length - 2, 2);
        roadCheckCount = RoadCheckCounter(roadCheckID);

        //Variables relating to each crossing
        switch (roadCheckID)
        {
            case "C1":
                turnAngleL = 80;
                turnAngleR = -80;
                
                break;
            case "C2":
                turnAngleL = -100;
                turnAngleR = 100;
                break;
            case "C3":
                turnAngleL = 170;
                turnAngleR = 10;
                break;
            case "C4":
                turnAngleL = 350;
                turnAngleR = 190;
                break;

            default:
                Debug.Log("Err: Unsorted RoadCheck");
                break;
        }
        Vector3 direction = waypoints[pathIndex + roadCheckCount].position - transform.position;
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

    bool IsRoadCheck(int _pathIndex)
    {
        return waypoints[_pathIndex].transform.name.ToString().Contains("RoadCheck");
    }

    int RoadCheckCounter(string id)
    {
        int count = 0;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i].transform.name.ToString().Contains(id)) count++;

        }
        return count;
    }
}