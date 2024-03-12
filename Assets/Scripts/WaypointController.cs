using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WaypointController : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent agent;

    private bool roadCheckNext = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        NextPoint();
    }

    void Update()
    {
        


        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            if (roadCheckNext)
            {
                Debug.Log("Road Check Now!");
                roadCheckNext = false;
            }
            if (waypoints[currentWaypointIndex].transform.name.ToString().Contains("RoadCheck"))
            {
                Debug.Log(currentWaypointIndex);
                Debug.Log("Road Check Next!");
                roadCheckNext=true;

            }
            NextPoint();
        }
    }

    void NextPoint()
    {
        if (waypoints.Length == 0) return;
        
        
        agent.SetDestination(waypoints[currentWaypointIndex].position);
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

       

    }
}