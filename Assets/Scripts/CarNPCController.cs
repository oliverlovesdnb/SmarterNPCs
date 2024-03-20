/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    Some code sampled from Unity Documentation: https://docs.unity3d.com/560/Documentation/Manual/nav-AgentPatrol.html
*/

using UnityEngine;
using UnityEngine.AI;

public class CarNPCController : MonoBehaviour
{
    public Transform[] waypoints;
    private NavMeshAgent agent;

    private int pathIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;
        NextPoint();
        //Application.targetFrameRate = 30;
    }

    // Update is called once per frame
    void Update()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
            NextPoint();
    }

    void NextPoint()
    {
        if (waypoints.Length == 0) return;

        agent.SetDestination(waypoints[pathIndex].position);
        pathIndex = (pathIndex + 1) % waypoints.Length;
    }
}
