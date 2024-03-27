/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Some code sampled from Unity Documentation: https://docs.unity3d.com/560/Documentation/Manual/nav-AgentPatrol.html
    Raycat Documentation: https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
    Coroutines Documentation: https://docs.unity3d.com/Manual/Coroutines.html
*/

using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WaypointController : MonoBehaviour
{
    public float lookDistance; //Set in Unity Editor

    public Transform[] waypoints; //Set in Unity Editor
    private NavMeshAgent agent;

    RaycastHit hit;
    public LayerMask Car;

    private int pathIndex = 0;
    private int roadCheckCount = 0;
    private int turnAngleL, turnAngleR = 0;

    private bool roadCheckNext = false;
    private bool carHit = false;
    private bool currentlyRotating = false;

    //Method that executes on initialisation of script, before Update() runs
    void Start()
    {
        agent = GetComponent<NavMeshAgent>(); //Gets agent component from Unity
        agent.speed += Random.Range(0,2); //Gives NPC speeds slight variation randomly
        NextPoint();
       // Application.targetFrameRate = 60;  // **For performance issues while running this can be enabled**
    }

    //Method that runs every frame
    void Update()
    {
        //Stops NPC from moving while performing roadCheck
        if (currentlyRotating == true)
        {
            return;
        }

        //Code that runs when NPC reaches waypoint
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            //If waypoint is a RoadCheck, call RoadCheckNow() to prepare variables for RoadCheck()
            if (roadCheckNext)
            {
                RoadCheckNow();
                return;
                

            }
            //If waypoints either side are RoadChecks, randomly choose either
            if (pathIndex > 1 && IsRoadCheck(pathIndex - 2 ) && IsRoadCheck(pathIndex))
            {
                int randomNumber = Random.Range(0, 2);

                pathIndex -= randomNumber * 2;
                roadCheckNext = true;
            }
            //If RoadCheck next, prepare for RoadCheckNow()
            else if (IsRoadCheck(pathIndex))
            {
                roadCheckNext=true;
            }
            //If RoadCheck previous and NOT next, go back and prepare for RoadCheckNow()
            else if(pathIndex > 1 && IsRoadCheck(pathIndex - 1))
            {
                pathIndex -= 2;
                roadCheckNext = true;
            }
            //Go to next waypoint in list
            NextPoint();
        }
    }

    //Checks once if ray cast sent in the forward direction of NPC hits car
    bool CheckCarHit()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, lookDistance, Car))
        {
            if (hit.collider.CompareTag("Car")) carHit = true;
        }
        return carHit;
    }

    //Sets up variables for RoadCheck
    void RoadCheckNow()
    {
        agent.isStopped = true;

        //Getting crossing ID from object name
        string roadCheckID = waypoints[pathIndex - 1].transform.name.ToString();
        roadCheckID = roadCheckID.Substring(roadCheckID.Length - 2, 2);

        //Counts how many road checks are together to know how much to increment index when crossing road
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
        //Turns NPC towards waypoint directly across road before RoadCheck()
        Vector3 direction = waypoints[pathIndex + roadCheckCount].position - transform.position;
        transform.rotation = Quaternion.LookRotation(direction.normalized, Vector3.up);

        RoadCheck();

        //Unfreeze NPC
        agent.isStopped = false;
        roadCheckNext = false;
    }


    //Set agent destination as next point in waypoints array, loops pathIndex
    void NextPoint()
    {
        if (waypoints.Length == 0) return;
        
        
        agent.SetDestination(waypoints[pathIndex].position);
        pathIndex = (pathIndex + 1) % waypoints.Length;
    }

    //Shorthand method to initiate coroutine
    void RoadCheck()
    {
        StartCoroutine(RotateLeftAndRight());
    }

    //Coroutine in charge of entire method to check road
    //Tells agent if car is detected
    IEnumerator RotateLeftAndRight()
    {
        //Waits for first coroutine to complete for calling second coroutine
        yield return StartCoroutine(RotateToTarget(Quaternion.Euler(0, turnAngleL, 0)));
        yield return StartCoroutine(RotateToTarget(Quaternion.Euler(0, turnAngleR, 0)));

        //Allows agent to move after car is detected to choose alt crossing point
        if (carHit)
        {
            currentlyRotating = false;
        }

        //Cross to point on the opposite side of the road
        else
        {
            pathIndex += roadCheckCount-1;
            currentlyRotating = false;
            NextPoint();
        }
        
    }

    //Rotates NPC to a specific angle
    IEnumerator RotateToTarget(Quaternion finalRotation)
    {
        carHit = false;
        currentlyRotating = true;
        Quaternion initialRotation = transform.rotation;

        float rotationSpeed = 0.9f;
        float counter = 0;
        
        //Turns NPC gradually while checking for car with CheckCarHit() ray case method
        while (counter < rotationSpeed)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, finalRotation, counter / rotationSpeed);
            counter += Time.deltaTime;
            CheckCarHit();
            yield return null;
        }
        transform.rotation = finalRotation;
        
    }

    //Shorthand method for checking if waypoint is RoadCheck
    bool IsRoadCheck(int _pathIndex)
    {
        return waypoints[_pathIndex].transform.name.ToString().Contains("RoadCheck");
    }

    //Returns count of RoadChecks in set
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