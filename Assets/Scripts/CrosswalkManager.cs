using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class CrosswalkManager : MonoBehaviour
{
    Transform Road;
    public NavMeshModifier navMeshModifier;
    void Start()
    {
        navMeshModifier = GetComponent<NavMeshModifier>();
        navMeshModifier.ignoreFromBuild = true; 
    }

    /*void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            
            if (CanCrossSafely())
            {
                EnableCrosswalk();
            }
        }
    }*/

    bool CanCrossSafely()
    {
        Debug.Log("can cross safely");
        return true;
    }

    public void EnableCrosswalk(int id)
    {
        Debug.Log("enabling navmesh for: "+id);
        navMeshModifier.ignoreFromBuild = false; 
        NavMeshSurface[] surfaces = GetComponents<NavMeshSurface>();
        //Debug.Log(surfaces.Count());
        /*foreach (NavMeshSurface surface in surfaces){
            
            surface.BuildNavMesh();
        }*/
        surfaces[id].BuildNavMesh();
            
    }
}