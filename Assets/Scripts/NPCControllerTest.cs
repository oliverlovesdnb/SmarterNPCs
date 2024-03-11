/*
    Code created by Oliver Fiedot-Davies, 2024
    For Maynooth University Computer Science and Software Engineering Final Year Project.
*/
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;

public class NPCControllerTest : MonoBehaviour
{
    public Transform Target;
    public NavMeshAgent agent;
    public CrosswalkManager crosswalkManager;

    void Start()
    {
        crosswalkManager = FindAnyObjectByType<CrosswalkManager>();
    }
    void Update()
    {
        MoveNavMeshAgent();
        OnRoadEnter();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Road"))
        {
            int NPC = Int32.Parse(transform.name);
            Debug.Log("Touched Road: " + NPC);
            crosswalkManager.EnableCrosswalk(NPC);
        }
        
    }

    public void OnRoadEnter()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            crosswalkManager.EnableCrosswalk(Int32.Parse(transform.name));
        }
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
