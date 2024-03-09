using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CrosswalkManager : MonoBehaviour
{
    private NavMeshModifier navMeshModifier;

    void Start()
    {
        navMeshModifier = GetComponent<NavMeshModifier>();
        navMeshModifier.ignoreFromBuild = true; 
    }

    void Update()
    {
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
        navMeshModifier.ignoreFromBuild = false; 
        NavMeshSurface[] surfaces = FindObjectsOfType<NavMeshSurface>();

        foreach (NavMeshSurface surface in surfaces)
        {
            surface.BuildNavMesh();
        }
    }
}