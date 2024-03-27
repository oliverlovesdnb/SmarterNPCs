/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Some code sampled from Unity Documentation: https://docs.unity3d.com/560/Documentation/Manual/nav-AgentPatrol.html

    Code is simplified version of WaypointController.cs
*/

using UnityEngine;
using UnityEngine.AI;

public class CarNPCController : MonoBehaviour
{
    public Transform[] waypoints; //Set in Unity Editor
    private NavMeshAgent agent;

    private int pathIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; //Disable autobraking for smoother movement when looping
        NextPoint();
    }

    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            NextPoint();
    }

    //Tells Car NPC to go to next waypoint in array
    void NextPoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[pathIndex].position);
        pathIndex = (pathIndex + 1) % waypoints.Length;
    }
}
