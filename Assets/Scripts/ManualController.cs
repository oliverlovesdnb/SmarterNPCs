using UnityEngine;
using UnityEngine.AI;

public class ManualController : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool isCrossing = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CrossRoadManually();
        
    }

    bool CanCrossSafely()
    {
        // Implement your logic to determine if it's safe to cross
        // For example, check for oncoming traffic, car speed, etc.
        return true;
    }

    void StartCrossing()
    {
        isCrossing = true;

        // Temporarily disable the global NavMeshAgent
        navMeshAgent.enabled = false;
    }

    void CrossRoadManually()
    {
        navMeshAgent.enabled = false;
        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            // Move the NPC manually across the road using Translate
        }

            // Check if the NPC has crossed the road
            /*if (HasCrossedRoad())
            {
                // Re-enable the global NavMeshAgent
                navMeshAgent.enabled = true;

                // Reset the crossing state
                isCrossing = false;
            }*/
        }

    bool HasCrossedRoad()
    {
        // Implement your logic to check if the NPC has crossed the road
        // You might use raycasting, trigger zones, or other methods depending on your game setup
        return true;
    }
}