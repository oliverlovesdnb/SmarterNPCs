/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.
*/
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform Target;
    public NavMeshAgent agent;

    //Timer for measuring travel time
    Stopwatch stopwatch = new Stopwatch();
    void Awake()
    {
        stopwatch.Start();
    }
    void Update()
    {
        MoveNavMeshAgent();
    }

    //Move function for NavMesh agent, waits until grid & path !null
    void MoveNavMeshAgent()
    {
        if (Target != null)
        {
            Vector3 targetPos = Target.position;
        }
        if (Target.position != null && transform.position != null)
        {
            //Sets destination as final node rather than next node in list
            agent.SetDestination(Target.position);
            long elapsedTime = stopwatch.ElapsedMilliseconds;
            //UnityEngine.Debug.Log("NavMesh NPC reached target, time: " + elapsedTime + "ms");
        }
    }
}
