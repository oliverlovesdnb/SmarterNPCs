using UnityEngine;
using UnityEngine.AI;

public class NPCAreaCost : MonoBehaviour
{
    public int roadAreaCost = 1;   // Adjust the cost values as needed
    public int footpathAreaCost = 2;

    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetAgentAreaCosts();
    }

    void SetAgentAreaCosts()
    {
        // Set up area costs for this specific agent
        navMeshAgent.areaMask = NavMesh.AllAreas;  // Start with all areas enabled
        SetAreaCost("Road", roadAreaCost);
        SetAreaCost("Footpath", footpathAreaCost);
    }

    void SetAreaCost(string areaName, int cost)
    {
        int areaIndex = NavMesh.GetAreaFromName(areaName);
        if (areaIndex != -1)
        {
            navMeshAgent.SetAreaCost(areaIndex, cost);
        }
        else
        {
            Debug.LogError("Area not found: " + areaName);
        }
    }
}