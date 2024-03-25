/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.

    * Old test script, not used in final build. For final build NPC script, please check WaypointController.cs *
*/
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public Transform Target;
    public NavMeshAgent agent;

    void Awake()
    {
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
        }
    }
}
