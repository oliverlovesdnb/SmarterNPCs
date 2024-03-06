using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CrosswalkManager : MonoBehaviour
{
    private NavMeshModifier navMeshModifier;

    void Start()
    {
        navMeshModifier = GetComponent<NavMeshModifier>();
        navMeshModifier.ignoreFromBuild = true;  // Initially, ignore from build
    }

    void Update()
    {
        // Assuming there's some condition for deciding it's safe to cross
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("space key was pressed");
            if (CanCrossSafely())
            {
                EnableCrosswalk();
            }
        }
    }

    bool CanCrossSafely()
    {
        return true;
    }

    void EnableCrosswalk()
    {
        navMeshModifier.ignoreFromBuild = false;  // Enable the crosswalk area
        NavMeshSurface[] surfaces = FindObjectsOfType<NavMeshSurface>();

        // Update the NavMesh surfaces to reflect the changes
        foreach (NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();
        }
    }
}